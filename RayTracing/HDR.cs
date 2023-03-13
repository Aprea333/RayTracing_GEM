using System.Net.Mime;

namespace RayTracing;

public class HDR
{
    public int width;
    public int height;


    public HDR()
    {
        width = 0;
        height = 0;
    }

    public HDR(int w, int h)
    {
        width = w;
        height = h;
    }
        
        
    public static void self_pixels(HDR hdr_image)
    {
        int dim = hdr_image.height * hdr_image.width;
        Color [] image = new Color [dim];
        for (int i = 0; i < dim; i++)
        {
            Color c = new Color();
            image[i]= c ;
        }
    }
}

