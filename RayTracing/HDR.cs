
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Text.Encoding;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace RayTracing;

using System;
using System.IO;

[Serializable]
public class InvalidPfmFileFormatException : Exception
{
    public InvalidPfmFileFormatException () {}
    public InvalidPfmFileFormatException(string message)
        : base(message) { }
    public InvalidPfmFileFormatException(string message, Exception inner)
        : base(message, inner) { }
}


public class HdrImage
{
    public int width;
    public int height;
    public List<Colour> hdr_image = new List<Colour>();

    public HdrImage()
    {
        width = 0;
        height = 0;
    }

    public HdrImage(int w, int h)
    {
        width = w;
        height = h;
        hdr_image.Capacity = width * height;
        for (int i = 0; i < w * h; i++)
        {
            Colour c = new Colour();
            hdr_image.Insert(i, c);
        }
    }

    public HdrImage(Stream input_stream)
    {
        hdr_image = new List<Colour>();
        read_pfm_image(input_stream);
    }

    public HdrImage(string file_name)
    {
        hdr_image = new List<Colour>();
        using Stream fileStream = File.OpenRead(file_name);
        read_pfm_image(fileStream);
    }
    public bool valid_coordinates(int x, int y)
        => x >= 0 && x < width && y < height && y >= 0;

    public void set_pixel(Colour c, int x, int y)
    {
        Debug.Assert(valid_coordinates(x, y), "Invalid coordinates");
        hdr_image[y * width + x] = c;
    }

    public Colour get_pixel(int x, int y)
    {
        Debug.Assert(valid_coordinates(x, y), "Invalid coordinates");
        return hdr_image[y * width + x];
    }


    public static string read_line(Stream myStream)
    {
        var result = "";
        int mybyte;
        while (true)
        {
            mybyte = myStream.ReadByte();
            if (mybyte is -1 or '\n')
                return result;
            result += (char)mybyte;
        }
    }

    public static bool parse_endianness_isLittle(string line3)
    {
        float end;
        try
        {
            float.Parse(line3, CultureInfo.InvariantCulture.NumberFormat);
        }
        catch (FormatException ex)
        {
            throw new InvalidPfmFileFormatException("Impossible parse " + line3 + " to float", ex);
        }

        end = float.Parse(line3, CultureInfo.InvariantCulture.NumberFormat);
        if (end == 0)
        {
            throw new InvalidPfmFileFormatException("'0' invalid value, a positive or negative value is required");
        }

        return (end < 0);
    }

    public static float read_float(Stream mystream, bool le)
    {
        byte[] bytes = new byte[4];
        try
        {
            bytes[0] = (byte)mystream.ReadByte();
            bytes[1] = (byte)mystream.ReadByte();
            bytes[2] = (byte)mystream.ReadByte();
            bytes[3] = (byte)mystream.ReadByte();
        }
        catch
        {
            throw new InvalidPfmFileFormatException("Impossible to read data from the file");
        }

        if (!le && BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        if (le && !BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }

        return BitConverter.ToSingle(bytes);
    }

    public static (int, int) parse_img_size(string line)
    {
        var elements = line.Split(" ");
        if (elements.Length != 2)
        {
            throw new InvalidPfmFileFormatException("Invalid Image Size Specification");
        }

        try
        {
            var dim = Array.ConvertAll(elements, int.Parse);
            if (dim[0] <= 0 || dim[1] <= 0)
            {
                throw new InvalidPfmFileFormatException("Number cannot be negative");
            }

            var dimension = (dim[0], dim[1]);
            return dimension;
        }
        catch (FormatException e)
        {
            throw new InvalidPfmFileFormatException("Invalid width/height", e);
        }
    }

    /// <summary>
    /// Function that read a pfm file
    /// </summary>
    /// <param name="myStream"></param>
    /// <exception cref="InvalidPfmFileFormatException"></exception>
    public void read_pfm_image(Stream myStream)
    {
        string magic = read_line(myStream);
        if (magic != "PF")
        {
            throw new InvalidPfmFileFormatException("Invalid magic in PFM file");
        }

        (width, height) = parse_img_size(read_line(myStream));
        bool endianness = parse_endianness_isLittle(read_line(myStream));
        hdr_image.Capacity = width * height;
        for (int i = 0; i < width * height; i++) 
        {
            Colour c = new Colour();
            hdr_image.Insert(i, c);
        }

        //HDR result = new HDR(width, height);         
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                float r = read_float(myStream, endianness);
                float g = read_float(myStream, endianness);
                float b = read_float(myStream, endianness);
                set_pixel(new Colour(r, g, b), x, y);
            }
        }
    }

    public void write_float(Stream mystream, float value, bool le)
    {
        var seq = BitConverter.GetBytes(value);
        if (!le && BitConverter.IsLittleEndian)
        {
            Array.Reverse(seq);
        }

        if (le && !BitConverter.IsLittleEndian)
        {
            Array.Reverse(seq);
        }

        mystream.Write(seq, 0, seq.Length);
    }

    public void write_pfm(Stream mystream, bool end)
    {
        var endString = end == BitConverter.IsLittleEndian ? "-1.0" : "1.0";
        string header = $"PF\n{width} {height}\n{endString}\n";
        //Convert header into a sequence of bytes
        mystream.Write(Encoding.ASCII.GetBytes(header));

        //Write the image
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                Colour col = get_pixel(x, y);
                write_float(mystream, col.r_c, end);
                write_float(mystream, col.g_c, end);
                write_float(mystream, col.b_c, end);
            }
        }

    }

    public float average_luminosity(float delta = 1e-5f)
    {
        float cumsum = 0;
        foreach (Colour pix in hdr_image)
        {
            cumsum += (float)Math.Log10(delta + pix.luminosity());
        }

        return (float)Math.Pow(10, cumsum / hdr_image.Capacity);
    }


    /// <summary>
    /// Tuning map of pixels
    /// </summary>
    /// <param name="factor"></param>
    /// <param name="luminosity"></param>
    public void normalize_image(float factor, float? luminosity = null)
    {
        var lum = luminosity ?? average_luminosity();
        float c = factor / lum;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var t = get_pixel(j, i);
                set_pixel(new Colour(t.r_c*c, t.g_c*c, t.b_c*c), j, i);
            }
        }
    }

    private float clamp(float x)
    {
        return x / (1 + x);
    }

    public void clamp_image()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var pix = get_pixel(j, i);
                set_pixel(new Colour(clamp(pix.r_c), clamp(pix.g_c), clamp(pix.b_c)), j, i);
            }
        }
    }

    /// <summary>
    /// Write a ldr file and saving in png
    /// </summary>
    /// <param name="mystream"></param>
    /// <param name="format"></param>
    /// <param name="gamma"></param>
    public void write_ldr_image(Stream mystream, string? format = null, float? gamma = 1)
    {
        var g = gamma ?? 1;
        var f = format ?? ".png";
        var bitmap = new Image<Rgb24>(Configuration.Default, width, height);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var curColor = get_pixel(j, i);
                bitmap[j, i] = new Rgb24((byte)(255*Math.Pow(curColor.r_c, 1 / g)),
                    (byte)(255*Math.Pow(curColor.g_c, 1 / g)), (byte)(255*Math.Pow(curColor.b_c, 1 / g)));

            }
        }
        
        bitmap.Save(mystream, new PngEncoder());
        
    }

}
    

