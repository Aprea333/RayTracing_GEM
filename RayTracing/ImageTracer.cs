using System.Diagnostics.CodeAnalysis;

namespace RayTracing;
using System.Threading.Tasks;
using ConsoleProgressBar;
using ShellProgressBar;
using System.Text;
using System.Threading.Tasks;
public delegate Colour Function(Ray r);

public class ImageTracer
{
    public HdrImage Image;
    private Camera Camera;
    private int sample_per_side;
    private PCG pcg;

    public ImageTracer(HdrImage image, Camera cam, int sample_per_side = 0, PCG? pcg = null)
    {
        Image = image;
        Camera = cam;
        this.pcg = pcg ?? new PCG();
        this.sample_per_side = sample_per_side;
    }

    public Ray fire_ray(int col, int row, float u_pixel = 0.5f, float v_pixel = 0.5f)
    {
        var u = (col + u_pixel) / (Image.width);
        var v = 1 - (row + v_pixel) / (Image.height);
        return Camera.fire_ray(u, v);
    }

    public void fire_all_rays(Function func)
    {
        for (var i = 0; i < Image.height; i++)
        {
            for (var j = 0; j < Image.width; j++)
            {
                var r = this.fire_ray(j, i);
                var c = func(r);
                Image.set_pixel(c, j, i);
            }
        }
    }


    /// <summary>
    /// Shoot several light rays crossing each of the pixels in the image.Use the sample_per_pixel (must be a perfect squared number) for a better resolution of the image
    /// </summary>
    /// <param name="rend">Type of renderer</param>
    public void fire_all_rays(Renderer rend)
    {
        var totalTicks = Image.height;
        
        //StringBuilder progressMessage = new StringBuilder();
        //var progressBar = new ConsoleProgressBar.ProgressBar();
        //using (var pbar = new ShellProgressBar.ProgressBar(totalTicks, "", options))
        var progressBarWidth = Console.WindowWidth - 20; // Larghezza della barra di progresso
        
        var rowCompleted = 0;
        var progressBarCharacter = '\u25A0';

        using var pbar = new ConsoleProgressBar.ProgressBar();
        try
        {

            Parallel.For((long)0, Image.height, i =>
            {
                for (int j = 0; j < Image.width; j++)
                {
                    Colour appo = new Colour(0, 0, 0);
                    if (sample_per_side > 0)
                    {
                        for (var iPixRow = 0; iPixRow < sample_per_side; iPixRow++)
                        {
                            for (var iPixCol = 0; iPixCol < sample_per_side; iPixCol++)
                            {
                                var uPix = (iPixCol + pcg.random_float()) / sample_per_side;
                                var vPix = (iPixRow + pcg.random_float()) / sample_per_side;
                                var rr = fire_ray(col: j, row: (int)i, u_pixel: uPix, v_pixel: vPix);
                                appo += rend.tracing(rr);
                            }
                        }

                        Image.set_pixel(appo * (1.0f / (float)Math.Pow(sample_per_side, 2)), j, (int)i);
                    }
                    else
                    {
                        var ray = fire_ray(j, (int)i);
                        var me_in = rend.tracing(ray);
                        Image.set_pixel(me_in, j, (int)i);
                    }
                }

                lock (pbar)
                {
                    rowCompleted++;
                    var progress = (float)rowCompleted / totalTicks;
                    var progressBarFilledWidth = (int)(progressBarWidth * progress);

                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write("[");
                    Console.Write(new string(progressBarCharacter, progressBarFilledWidth));
                    Console.Write(new string(' ', progressBarWidth - progressBarFilledWidth));
                    Console.Write("] {0}%   ", (int)(progress * 100));
                      
                }

            });
        }
        catch (AggregateException)
        {

        }

        finally
        {
            Console.CursorVisible = true;
            Console.WriteLine();
        }
    }
}

public abstract class function
{
    public static Colour BaseColour(Ray r)
    {
        var c = new Colour(0.1f,0.2f,0.3f);
        return c;
    }
}