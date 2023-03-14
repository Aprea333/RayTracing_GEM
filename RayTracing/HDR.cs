using System.Collections;
using System.Diagnostics;
using System.Net.Mime;

namespace RayTracing;

public class HDR
{
    public int width;
    public int height;
    public List<Colore> hdr_image;
    public HDR()
    {
        width = 0;
        height = 0;
    }

    public HDR(int w, int h)
    {
        width = w;
        height = h;
        hdr_image.Capacity = w * h;
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
}


