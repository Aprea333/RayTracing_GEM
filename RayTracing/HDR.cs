
using System.Diagnostics;
using System.Globalization;
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
    public bool Valid_Coordinates(int x, int y)
        => x >= 0 && x < width && y < height && y >= 0;
    
    public void set_pixel(Colore c, int x, int y)
    {
        Debug.Assert(Valid_Coordinates(x, y), "Invalid coordinates");
        hdr_image.Insert(y*width+x,c); 
    }

    public Colore get_pixel(int x, int y)
    {
        Debug.Assert(Valid_Coordinates(x, y), "Invalid coordinates");
        return hdr_image[y * width + x];
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

        end = float.Parse(line3 ,CultureInfo.InvariantCulture.NumberFormat);
        if (end == 0)
        {
            throw new InvalidPfmFileFormatException("'0' invalid value, a positive or negative value is required");
        }

        return (end < 0);
    }
    
    private static (int, int) Parse_Img_Size(string line)
    {
        var elements = line.Split("");
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
}


