using System.Net;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace RayTracing;

public static class Program
{

    [Verb("demo", HelpText = "Demo")]
    private class DemoOption
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
        
        int w = opts.Width;
        int h = opts.Height;
        HdrImage image = new HdrImage(w, h);
        World world = new World();
        
        //Add 8 spheres to the world
        Transformation sphereScale = Transformation.scaling(0.1f, 0.1f, 0.1f);
        for (float x = -0.5f; x <= 0.5f; x+=1f)
        {
            for (float y = -0.5f; y <= 0.5f; y += 1f)
            {
                for (float z = -0.5f; z <= 0.5f; z += 1f)
                {
                    world.add(new Sphere(Transformation.translation(new Vec(x, y, z))*sphereScale));
                }
            }
        }
        
        //Add two spheres
        world.add(new Sphere(Transformation.translation(new Vec(0.0f,0.5f,0.0f))*sphereScale));
        world.add(new Sphere(Transformation.translation(new Vec(0.0f,0.0f,-0.5f))*sphereScale));
        
        
        Transformation cam_tr = Transformation.rotation_z(opts.Angle)*Transformation.translation(new Vec(-1f, 0f, 0f));
        Camera cam= new PerspectiveCamera(aspect_ratio: opts.Width / opts.Height, tran: cam_tr);
        /*if (opts.Camera != "perspective")
        {
            cam = new Orthogonal_Camera(aspect_ratio: opts.Width / opts.Height, tran: cam_tr);
        }
        else
        {
            cam = new PerspectiveCamera(Aspect_Ratio: opts.Width / opts.Height, tran: cam_tr);
        }*/

        ImageTracer imageTracer = new ImageTracer(image, cam);
        
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
                    imageTracer.Image.set_pixel(new Colour(255.0f, 255.0f, 255.0f), i, j);
                }
                else
                {
                    imageTracer.Image.set_pixel(new Colour(0.0f, 0.0f, 0.0f), i, j);
                }
            }
        }

        string root_directory = Environment.CurrentDirectory;
        string path = Path.Combine(root_directory, "image.pfm");
        File.CreateText(path).Close();
        Stream file_out = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None);
        imageTracer.Image.write_pfm(file_out, true);
        file_out.Close();
    }
 
    [Verb("pfm2png", HelpText = "Pfm image")]
    class pfm2png_option
    {
        [Option("factor", Default = 0.2f, HelpText = "Multiplicative factor")]
        public float Factor { get; set; }

        [Option("gamma", Default = 1.0f, HelpText = "value to be used for gamma correction")]
        public float Gamma { get; set; }

        [Option("input_file", Required = true, HelpText = "path + input file name + .pfm")]
        public string input { get; set; } = null!;

        [Option("output_file", Default = "image.png", HelpText = "path + output file name + .png")]
        public string output { get; set; } = null!;
    }

    static void RunOptionPfm(pfm2png_option opts)
    {
        HdrImage img = new HdrImage();

        using (FileStream in_pfm = File.Open(opts.input, FileMode.Open))
        {
            img.read_pfm_image(in_pfm);
        }

        //Console.WriteLine($"File {opts.input} has been read from disk");

        img.normalize_image(opts.Factor);
        img.clamp_image();

        File.CreateText(opts.output).Close();
        Stream out_png = File.Open(opts.output, FileMode.Open, FileAccess.Write, FileShare.None);
        img.write_ldr_image(out_png, ".png", opts.Gamma);
        //Console.WriteLine($"File {opts.output} has been written to disk");
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
    }
}


//pfm2png  --input_file image.pfm
