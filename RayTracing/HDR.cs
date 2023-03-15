
namespace RayTracing;

using System;
using System.IO;
using System.Text;


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



}


