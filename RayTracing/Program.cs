using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;
using RayTracing;
using SixLabors.ImageSharp.Formats.Png;
using CommandLine;

public class Program
{

    [Verb("demo", HelpText = "Demo")]
    class DemoOption
    {
        public DemoOption()
        {
            
        }

        [Option("width", Default = 640, HelpText = "Width of the image to render")]
        public int Width { get; set; }

        [Option("height", Default = 480, HelpText = "Height of the image to render")]
        public int Height { get; set; }

        [Option("angle_deg", Default = 0.0f, HelpText = "Angle of view")]
        public float Angle { get; set; }

        [Option("camera", Default = "perspective", HelpText = "Type of camera")]
        public string Camera { get; set; }
    }

    static void RunDemo(DemoOption opts)
    {
        int w = 640;
        int h = 480;
        HDR img = new HDR(w, h);
        World world = new World();
        
        world.add(new Sphere(Tran.Translation_matr(new Vec(-3f, 0f, 0f))));
        Tran cam_t = Tran.Translation_matr(new Vec(2f, 0f, 0f));
        camera cam = new Orthogonal_Camera();

        ImageTracer tracer = new ImageTracer(img, cam);
        //Ray ray = new Ray(new RayTracing.Point(0f, 0f, 0f), new Vec(-1f, 0f, 0f));
        
        //HitRecord? hit = world.ray_intersection(ray);
        //Console.WriteLine($"HitRecord: {hit == null}, has_value: {hit.HasValue}");
        int count_int = 0;
        int count_non = 0;
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                //imageTracer.Image.set_pixel(new Colore(100.0f, 100.0f, 100.0f), i,j);
                Ray ray = tracer.fire_ray(i, j);
                HitRecord? hit = world.ray_intersection(ray);
                //Console.WriteLine($"\nIter: {iter} Screen coord: ({i},{j}), Ray origin: {ray.Origin.ToString()}, Ray dir: {ray.Dir.ToString()}, Ray at 2: {ray.At(2f).ToString()}, Hit: {hit.HasValue}, hit_isnull: {hit == null}");
                if (hit != null)
                {
                    tracer.Image.set_pixel(new Colore(250.0f, 250.0f, 250.0f), i, j);
                    count_int++;
                }
                else
                {
                    tracer.Image.set_pixel(new Colore(1.0f, 1.0f, 1.0f), i, j);
                    count_non++;
                }
                //Console.WriteLine($"HitRecord: {hit == null}, has_value: {hit.HasValue}");
            }
        }
        
        Console.WriteLine($"\nEnd for. \nInt: {count_int}     non: {count_non}");
        File.CreateText(@"C:\Users\miche\RayTracing_GEM\image.pfm").Close();
        Stream file_out = File.Open(@"C:\Users\miche\RayTracing_GEM\image.pfm", FileMode.Open,
            FileAccess.Write, FileShare.None);
        tracer.Image.write_pfm(file_out, true);
        file_out.Close();
        
    }

    /*static void RunDemo(DemoOption opts)
    {
        int w = opts.Width;
        int h = opts.Height;
        HDR image = new HDR(w, h);
        World world = new World();
        
        //Add 8 spheres to the world
        for (float x = -0.5f; x <= 0.5f; x+=1f)
        {
            for (float y = -0.5f; y <= 0.5f; y += 1f)
            {
                for (float z = -0.5f; z <= 0.5f; z += 1f)
                {
                    Tran t0 = Tran.Translation_matr(new Vec(x, y, z))*Tran.scale_matrix(0.1f,0.1f,0.1f);
                    world.add(new Sphere(t0));
                }
            }
        }
        
        Tran t1 = Tran.Translation_matr(new Vec(0.0f,0.5f,0.0f))*Tran.scale_matrix(0.1f,0.1f,0.1f);
        Tran t2 = Tran.Translation_matr(new Vec(0.0f,0.0f,-0.05f))*Tran.scale_matrix(0.1f,0.1f,0.1f);

        world.add(new Sphere(t1));
        world.add(new Sphere(t2));

        Tran cam_tr = Tran.Translation_matr(new Vec(1f, 0f, 0f));
        camera cam = new PerspectiveCamera(Aspect_Ratio: opts.Width / opts.Height, tran: cam_tr);
        if (opts.Camera != "perspective")
        {
            cam = new Orthogonal_Camera(aspect_ratio: opts.Width / opts.Height, tran: cam_tr);
        }
        else
        {
            cam = new PerspectiveCamera(Aspect_Ratio: opts.Width / opts.Height, tran: cam_tr);
        }

        ImageTracer imageTracer = new ImageTracer(image, cam);
        int count_int = 0;
        int count_non = 0;
        int iter = 0;
        Console.WriteLine($"\nwidth: {opts.Width}    height: {opts.Height}");
        Console.WriteLine($"Sphere: {world.shapes[0].tr.m.ToString()}");
        
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                //imageTracer.Image.set_pixel(new Colore(100.0f, 100.0f, 100.0f), i,j);
                Ray ray = imageTracer.fire_ray(i, j);
                HitRecord? hit = world.ray_intersection(ray);
                //Console.WriteLine($"\nIter: {iter} Screen coord: ({i},{j}), Ray origin: {ray.Origin.ToString()}, Ray dir: {ray.Dir.ToString()}, Ray at 2: {ray.At(2f).ToString()}, Hit: {hit.HasValue}, hit_isnull: {hit == null}");
                if (hit != null)
                {
                    imageTracer.Image.set_pixel(new Colore(100.0f, 100.0f, 100.0f), i, j);
                    count_int++;
                }
                else
                {
                    imageTracer.Image.set_pixel(new Colore(232.0f, 2.0f, 1.0f), i, j);
                    count_non++;
                }
            }
        }

        Console.WriteLine($"\nEnd for. \nInt: {count_int}     non: {count_non}");
        File.CreateText(@"C:\Users\miche\RayTracing_GEM\image.pfm").Close();
        Stream file_out = File.Open(@"C:\Users\miche\RayTracing_GEM\image.pfm", FileMode.Open,
            FileAccess.Write, FileShare.None);
        imageTracer.Image.write_pfm(file_out, true);
        file_out.Close();
    }*/

    [Verb("pfm2png", HelpText = "Pfm image")]
    class pfm2png_option
    {
        [Option("factor", Default = 0.2f, HelpText = "Multiplicative factor")]
        public float Factor { get; set; }

        [Option("gamma", Default = 1.0f, HelpText = "value to be used for gamma correction")]
        public float Gamma { get; set; }

        [Option("input_file", Required = true, HelpText = "path + input file name + .pfm")]
        public string input { get; set; }

        [Option("output_file", Default = "image.png", HelpText = "path + output file name + .png")]
        public string output { get; set; }
    }

    static void RunOptionPfm(pfm2png_option opts)
    {
        
        Console.WriteLine($"\nfactor: {opts.Factor}");
        HDR img = new HDR();

        using (FileStream in_pfm = File.Open(opts.input, FileMode.Open))
        {
            img.read_pfm_image(in_pfm);
        }

        Console.WriteLine($"File {opts.input} has been read from disk");

        img.NormalizeImage(opts.Factor);
        img.clamp_image();

        File.CreateText(opts.output).Close();
        Stream out_png = File.Open(opts.output, FileMode.Open, FileAccess.Write, FileShare.None);
        img.write_ldr_image(out_png, ".png", opts.Gamma);
        Console.WriteLine($"File {opts.output} has been written to disk");
        out_png.Close();

    }

    static void HandleError(IEnumerable<Error> errors)
    {
        var sentenceBuilder = SentenceBuilder.Create();
        foreach (var error in errors)
            Console.WriteLine(sentenceBuilder.FormatError(error));
    }

    static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<pfm2png_option, DemoOption>(args)
            .WithParsed<pfm2png_option>(RunOptionPfm)
            .WithParsed<DemoOption>(RunDemo)

            .WithNotParsed(HandleError);

        /*var result = CommandLine.Parser.Default.ParseArguments<pfm2png_option, DemoOption>(args);

        result.WithParsed<pfm2png_option>(RunOptionPfm);
        result.WithParsed<DemoOption>(RunDemo);
        result.WithNotParsed(errors =>
        {
            var sentenceBuilder = SentenceBuilder.Create();
            foreach (var error in errors)
                Console.WriteLine(sentenceBuilder.FormatError(error));
        });
*/
    }
}


//pfm2png  --input_file image.pfm