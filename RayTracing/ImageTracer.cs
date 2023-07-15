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
        float u = (col + u_pixel) / (Image.width);
        float v = 1 - (row + v_pixel) / (Image.height);
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
                Image.set_pixel(c, j, i);
            }
        }
    }


    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: RayTracing.Transformation; size: 585894MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Single[]; size: 158MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Single[]; size: 10401MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Single[]; size: 33906MB")]
    public void fire_all_rays(Renderer rend)
    {
        int totalTicks = Image.height;
        
        var options = new ProgressBarOptions
        {
            ProgressCharacter = '\u25A0',
            ForegroundColor = ConsoleColor.Yellow,
            ForegroundColorDone = ConsoleColor.Green,
            BackgroundColor = ConsoleColor.DarkBlue,
            ProgressBarOnBottom = true,
            CollapseWhenFinished = true,
            DisplayTimeInRealTime = true
        };
        //StringBuilder progressMessage = new StringBuilder();
        //var progressBar = new ConsoleProgressBar.ProgressBar();
        //using (var pbar = new ShellProgressBar.ProgressBar(totalTicks, "", options))
        int progressBarWidth = Console.WindowWidth - 20; // Larghezza della barra di progresso
        
        int rowCompleted = 0;
        char progressBarCharacter = '\u25A0';
        
        using (var pbar = new ConsoleProgressBar.ProgressBar())
        {

            try
            {

                Parallel.For((long)0, Image.height, i =>
                {
                    for (int j = 0; j < Image.width; j++)
                    {
                        Colour appo = new Colour(0, 0, 0);
                        if (sample_per_side > 0)
                        {
                            for (int iPixRow = 0; iPixRow < sample_per_side; iPixRow++)
                            {
                                for (int iPixCol = 0; iPixCol < sample_per_side; iPixCol++)
                                {
                                    float uPix = (iPixCol + pcg.random_float()) / sample_per_side;
                                    float vPix = (iPixRow + pcg.random_float()) / sample_per_side;
                                    Ray rr = fire_ray(col: j, row: (int)i, u_pixel: uPix, v_pixel: vPix);
                                    appo += rend.tracing(rr);
                                }
                            }

                            Image.set_pixel(appo * (1.0f / (float)Math.Pow(sample_per_side, 2)), j, (int)i);
                        }
                        else
                        {
                            Ray ray = fire_ray(j, (int)i);
                            Colour me_in = rend.tracing(ray);
                            Image.set_pixel(me_in, j, (int)i);
                        }
                    }

                    lock (pbar)
                    {
                        rowCompleted++;
                        float progress = (float)rowCompleted / totalTicks;
                        int progressBarFilledWidth = (int)(progressBarWidth * progress);

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
}






public abstract class function
{
    public static Colour BaseColour(Ray r)
    {
        Colour c = new Colour(0.1f,0.2f,0.3f);
        return c;
    }
}