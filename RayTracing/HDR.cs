
using System.Diagnostics;
using System.Globalization;

namespace RayTracing;

using System;
using System.IO;
using System.Text;


[Serializable]
public class InvalidPfmFileFormatException : Exception
{
    public InvalidPfmFileFormatException () {}
    public InvalidPfmFileFormatException(string message)
        : base(message) { }
    public InvalidPfmFileFormatException(string message, Exception inner)
        : base(message, inner) { }
}


public class HDR
{
    public int width;
    public int height;
    public List<Colore> hdr_image = new List<Colore>();

    public HDR()
    {
        width = 0;
        height = 0;
    }

    public HDR(int w, int h)
    {
        width = w;
        height = h;
        hdr_image.Capacity = width * height;
        for (int i = 0; i < w * h; i++)
        {
            Colore c = new Colore();
            hdr_image.Insert(i, c);
        }
    }

    public bool Valid_Coordinates(int x, int y)
        => x >= 0 && x < width && y < height && y >= 0;

    public void set_pixel(Colore c, int x, int y)
    {
        Debug.Assert(Valid_Coordinates(x, y), "Invalid coordinates");
        hdr_image.Insert(y * width + x, c);
    }

    public Colore get_pixel(int x, int y)
    {
        Debug.Assert(Valid_Coordinates(x, y), "Invalid coordinates");
        return hdr_image[y * width + x];
    }


    public static string read_line(Stream myStream)
    {
        var result = "";
        int my_byte;
        while (true)
        {
            my_byte = myStream.ReadByte();
            if (my_byte is -1 or '\n')
                return result;
            result += (char)my_byte;
        }
    }

    public bool parse_endianness_isLittle(string line3)
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

    public static float _read_float(Stream inputStream, bool le)
        {
            byte[] bytes = new byte[4];
            if (le) Array.Reverse(bytes);
            try
            {
                bytes[0] = (byte)inputStream.ReadByte(); // legge un singolo byte dello stream e lo assegna al primo elemento dell'array bytes.
                bytes[1] = (byte)inputStream.ReadByte();
                bytes[2] = (byte)inputStream.ReadByte();
                bytes[3] = (byte)inputStream.ReadByte();
            }
            catch
            {
                throw new InvalidPfmFileFormatException("impossible to read binary data from the file");
            }

            return BitConverter.ToSingle(bytes, 0);
        }


    public static (int, int) Parse_Img_Size(string line)
    {
        var elements = line.Split(" ");
        if (elements.Length != 2)
        {
            throw new InvalidPfmFileFormatException("Invalid Image Size Specification");
        }

        try
        {
            var dim = Array.ConvertAll(elements, int.Parse);
            if (dim[0] < 0 || dim[1] < 0)
            {
                throw new InvalidPfmFileFormatException("Number cannot be negative");
            }

            var dimension = (width: dim[0], height: dim[1]);
            return dimension;
        }
        catch (FormatException e)
        {
            throw new InvalidPfmFileFormatException("Invalid width/height", e);
        }
    }

    public HDR read_pfm_image(Stream myStream)
    {
        string magic = read_line(myStream);
        if (magic != "PF")
        {
            throw new InvalidPfmFileFormatException("Invalid magic in PFM file");
        }

        (int width, int height) = Parse_Img_Size(read_line(myStream));
        bool endianness = parse_endianness_isLittle(read_line(myStream));
        HDR result = new HDR(width, height);
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                float r = _read_float(myStream, endianness);
                float g = _read_float(myStream, endianness);
                float b = _read_float(myStream, endianness);
                result.set_pixel( new Colore(r,g,b), x, y);
            }
        }

        return result;

    }

}

