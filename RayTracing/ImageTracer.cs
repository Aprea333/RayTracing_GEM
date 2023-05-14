using System.Net.Mime;

namespace RayTracing;

public delegate Colore Function(Ray r);
public class ImageTracer
{
    public HDR Image;
    public camera Camera;

    public ImageTracer(HDR image, camera cam)
    {
        Image = image;
        Camera = cam;
    }
    public Ray fire_ray(int col, int row, float u_pixel = 0.5f, float v_pixel = 0.5f)
    {
        float u = (col+u_pixel)/(Image.width-1);
        float v = (row+v_pixel)/(Image.height-1);
        return Camera.fire_ray(u, v);
    }

    public void fire_all_rays(Function func)
    {
        for (int i = 0; i < Image.height; i++)
        {
            for (int j = 0; j < Image.width; j++)
            {
                Ray r = this.fire_ray(j, i);
                Colore c = func(r);
                Image.set_pixel(c,j,i);
            }
        }
    }
    
}

public abstract class function
{
    public static Colore BaseColour(Ray r)
    {
        Colore c = new Colore(0.1f,0.2f,0.3f);
        return c;
    }
    
    
}