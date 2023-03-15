
using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace RayTracing;

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
    public List<Colore> hdr_image = new List<Colore> ();
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
        for (int i = 0; i < w*h; i++)
        {
            Colore c = new Colore();
            hdr_image.Insert(i, c);
        }
    }
    
    public void set_pixel(Colore c, int x, int y)
    {
        hdr_image.Insert(y*width+x,c); 
    }

    public Colore get_pixel(int x, int y)
    {
        return hdr_image[y * width + x];
    }

    public static float _read_float(Stream inputStream, bool islittleendian)
    {
        byte[] bytes = new byte[4];
        if (islittleendian) Array.Reverse(bytes);
        try
        {
            bytes[0] = (byte)inputStream.ReadByte(); // legge un singolo byte dello stream e lo assegna al primo elemento dell'array bytes.
            bytes[1] = (byte)inputStream.ReadByte();
            bytes[2] = (byte)inputStream.ReadByte();
            bytes[3] = (byte)inputStream.ReadByte();
        }
        catch
        {throw new InvalidPfmFileFormatException("impossible to read binary data from the file");
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
            throw new InvalidPfmFileFormatException("invalid width/height");
        }
    }
    
}


