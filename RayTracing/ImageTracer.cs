using System.Net.Mime;

namespace RayTracing;

public delegate Colour Function(Ray r);
public class ImageTracer
{
    public HdrImage Image;
    public Camera Camera;
    public int sample_per_side;
    public PCG pcg;

    public ImageTracer(HdrImage image, Camera cam, int sample_per_side = 0, PCG? pcg = null)
    {
        Image = image;
        Camera = cam;
        this.pcg = pcg ?? new PCG();
        this.sample_per_side = sample_per_side;
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
        
        
        var max_ticks = Image.height;
        
        for (int row = 0; row < Image.height; row++)
        {
            for (int col = 0; col < Image.width; col++)
            {
                Colour cum_color = new Colour(0,0,0);
                if (sample_per_side > 0)
                {
                    for (int inter_pixel_row = 0; inter_pixel_row < sample_per_side; inter_pixel_row++)
                    {
                        for (int inter_pixel_col = 0; inter_pixel_col < sample_per_side; inter_pixel_col++)
                        {
                            float u_pix = (inter_pixel_col + pcg.random_float()) / sample_per_side;
                            float v_pix = (inter_pixel_row + pcg.random_float()) / sample_per_side;
                            Ray r = fire_ray(col, row, u_pix, v_pix);
                            cum_color += func.tracing(r);
                        }
                    }

                    Image.set_pixel(cum_color * (1.0f / (sample_per_side * sample_per_side)), col, row);
                }
                else
                {
                    Ray r = fire_ray(col, row);
                    Image.set_pixel(func.tracing(r), col, row);
                }
                
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