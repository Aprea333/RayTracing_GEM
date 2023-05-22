using System.Net;
using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace RayTracing;

public static partial class Program
{

    [Verb("demo", HelpText = "Demo")]
    private class DemoOption
    {
        public DemoOption()
        {
            
        }

        [Option("width", Default = 1024, HelpText = "Width of the image to render")]
        public int Width { get; set; }

        [Option("height", Default = 640, HelpText = "Height of the image to render")]
        public int Height { get; set; }

        [Option("angle_deg", Default = 0.0f, HelpText = "Angle of view")]
        public float Angle { get; set; }

        [Option("camera", Default = "perspective", HelpText = "Type of camera")]
        public string Camera { get; set; }
        
        [Option("output_file", Default = "image.png", HelpText = "path + output file name + .png")]
        public string output { get; set; } = null!;


    }



    static void RunDemo(DemoOption opts)
    {

        int w = opts.Width;
        int h = opts.Height;
        HdrImage image = new HdrImage(w, h);
        World world = new World();

        Colour c = new Colour(255f, 128f, 0f);
        Colour c5 = new Colour(0f, 255f, 1f);

        Colour c1 = new Colour(255f, 0f, 0f); //rosso
        // Colour c2 = new Colour(184f, 31f, 88f); //magenta
        Colour c3 = new Colour(0f, 0f, 255f);

        Material m1 = new Material(new DiffuseBrdf(new UniformPigment(c5)));

        Material m2 = new Material(new DiffuseBrdf(new CheckeredPigment(c1, c5)));

        Material m3 = new Material(new DiffuseBrdf(new CheckeredPigment(c, c5)));
        
        Material m4 = new Material(new DiffuseBrdf(new CheckeredPigment(c3, Colour.black)));
        
        Material m5 = new Material(new DiffuseBrdf(new UniformPigment(c1)));


        /*
         HdrImage sphere_texture = new HdrImage(2, 2);
        sphere_texture.set_pixel(new Colour(0f,255f,0f), 0,0); //verde
        sphere_texture.set_pixel(new Colour(255f,255f,30f), 0,1); //giallo
        sphere_texture.set_pixel(new Colour(145f,0f,255f), 1,0); //viola
        sphere_texture.set_pixel(new Colour(47f,227f,255f), 1,1); //azzurro

        Material m3 = new Material(new DiffuseBrdf(new ImagePigment(sphere_texture)));
        */


        //Add 8 spheres to the world
        Transformation sphereScale = Transformation.scaling(0.1f, 0.1f, 0.1f);
        Transformation sphereScale1 = Transformation.scaling(0.2f, 0.2f, 0.2f);


        for (float x = -0.5f; x <= 0.5f; x += 1f)
        {
            for (float y = -0.5f; y <= 0.5f; y += 1f)
            {
                for (float z = -0.5f; z <= 0.5f; z += 1f)
                {
                    world.add(new Sphere(Transformation.translation(new Vec(x, y, z)) * sphereScale, m4));
                }
            }
           

            //Add two spheres
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, 0.5f, 0.0f)) * sphereScale, m2));
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, 0.0f, -0.5f)) * sphereScale, m1));
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, -0.5f, 0.0f)) * sphereScale, m3));
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, 0.0f, +0.5f)) * sphereScale, m5));

            Transformation cam_tr = Transformation.rotation_z(opts.Angle) *
                                    Transformation.translation(new Vec(-1f, 0f, 0f));
            Camera cam = new PerspectiveCamera(aspect_ratio: opts.Width / opts.Height, tran: cam_tr);
            if (opts.Camera != "perspective")
            {
                cam = new OrthogonalCamera(aspect_ratio: opts.Width / opts.Height, transformation: cam_tr);
            }


            ImageTracer imageTracer = new ImageTracer(image, cam);

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {

                    Ray ray = imageTracer.fire_ray(i, j);
                    HitRecord? hit = world.ray_intersection(ray);
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

            Renderer rend1 = new OnOffRenderer(world);
            Renderer rend2 = new FlatRenderer(world);
            imageTracer.fire_all_rays(rend2);

            string root_directory = Environment.CurrentDirectory;
            string path = Path.Combine(root_directory, "image.pfm");
            File.CreateText(path).Close();
            Stream file_out = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None);
            imageTracer.Image.write_pfm(file_out, true);
            file_out.Close();

            HdrImage img = new HdrImage();

            using (FileStream in_pfm = File.Open("image.pfm", FileMode.Open))
            {
                img.read_pfm_image(in_pfm);
            }

            img.clamp_image();

            File.CreateText(opts.output).Close();
            Stream out_png = File.Open(opts.output, FileMode.Open, FileAccess.Write, FileShare.None);
            img.write_ldr_image(out_png, ".png", 1f);
            out_png.Close();
        }
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

        img.normalize_image(opts.Factor);
        img.clamp_image();

        File.CreateText(opts.output).Close();
        Stream out_png = File.Open(opts.output, FileMode.Open, FileAccess.Write, FileShare.None);
        img.write_ldr_image(out_png, ".png", opts.Gamma);
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
