
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
            throw new InvalidPfmFileFormatException("invalid width/height");
        }
    }
    
}


