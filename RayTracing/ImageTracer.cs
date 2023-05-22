using System.Net.Mime;

namespace RayTracing;

public delegate Colour Function(Ray r);
public class ImageTracer
{
    public HdrImage Image;
    public Camera Camera;

    public ImageTracer(HdrImage image, Camera cam)
    {
        Image = image;
        Camera = cam;
    }
    public Ray fire_ray(int col, int row, float u_pixel = 0.5f, float v_pixel = 0.5f)
    {
        float u = (col+u_pixel)/(Image.width);
        float v = 1 - (row+v_pixel)/(Image.height);
        return Camera.fire_ray(u, v);
    }

    public void fire_all_rays(Function func)
    {
        for (int i = 0; i < Image.height; i++)
        {
            for (int j = 0; j < Image.width; j++)
            {
                Ray r = this.fire_ray(j, i);
                Colour c = func(r);
                Image.set_pixel(c,j,i);
            }
        }
    }
    
    
    public void fire_all_rays(Renderer func)
    {
        for (int i = 0; i < Image.height; i++)
        {
            for (int j = 0; j < Image.width; j++)
            {
                Ray r = this.fire_ray(j, i);
                Colour c = func.tracing(r);
                Image.set_pixel(c,j,i);
            }
        }
    }
    
}

public abstract class function
{
    public static Colour BaseColour(Ray r)
    {
        Colour c = new Colour(0.1f,0.2f,0.3f);
        return c;
    }
}