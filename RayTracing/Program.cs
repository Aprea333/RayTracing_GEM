using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System;
using System.Globalization;
using CommandLine;
using CommandLine.Text;

namespace RayTracing;

public static partial class Program
{

    [Verb("demo", HelpText = "Demo")]
    private class DemoOption
    {
        [Option("width", Default = 600, HelpText = "Width of the image to render")]
        public int Width { get; set; }

        [Option("height", Default = 600, HelpText = "Height of the image to render")]

        public int Height { get; set; }

        [Option("angle_deg", Default = 90f, HelpText = "Angle of view")]
        public float Angle { get; set; }

        [Option("camera", Default = "perspective", HelpText = "Type of camera")]
        public string Camera { get; set; }
        
        [Option("output_file", Default = "image.png", HelpText = "path + output file name + .png")]
        public string output { get; set; } = null!;
    }

    static void RunDemo(DemoOption opts)
    {
        Console.WriteLine("DEMO");
        int w = opts.Width;
        int h = opts.Height;
        World world = new World();
        HdrImage image = new HdrImage(w,h);
        float min = Sphere.sphere_random_generation(50, world);
        Transformation cam_tr = Transformation.rotation_z(opts.Angle) * Transformation.translation(new Vec(min+7, 0, 1.3f));
        Camera cam;
        if (opts.Camera != "perspective")
        {
            cam = new OrthogonalCamera(aspect_ratio: (float)opts.Width / opts.Height, transformation: cam_tr);
        }
        else
        {
            cam = new PerspectiveCamera(aspect_ratio: (float)opts.Width / opts.Height, tran: cam_tr);
        }
        
        ImageTracer imageTracer = new ImageTracer(image, cam, sample_per_side: 16);
        Renderer rend = new PathTracer(world, new Colour(1, 1, 1), new PCG(), 10, 4, 3);
        imageTracer.fire_all_rays(rend);

        string root_directory = Environment.CurrentDirectory;
        Console.WriteLine($"Root Dir: {root_directory}");
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

        img.normalize_image(0.25f);
        img.clamp_image();

        File.CreateText(opts.output).Close();
        Stream out_png = File.Open(opts.output, FileMode.Open, FileAccess.Write, FileShare.None);
        img.write_ldr_image(out_png, ".png", 2f);
        out_png.Close();
    }
    
     
     [Verb("pfm2png", HelpText = "Pfm image")]
    class pfm2png_option
    {
        [Option("factor", Default = 0.5f, HelpText = "Multiplicative factor")]
        public float Factor { get; set; }

        [Option("gamma", Default = 0.5f, HelpText = "value to be used for gamma correction")]
        public float Gamma { get; set; }

        [Option("input_file", Default = "output.pfm", HelpText = "path + input file name + .pfm")]
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
    
    [Verb("render", HelpText = "Pfm image")]
    class render_option
    {
        [Option("width", Default = 640, HelpText = "Width of the image to render")] 
        public int Width { get; set; }

        [Option("height", Default = 480, HelpText = "Height of the image to render")]
        public int Height { get; set; }
        
        [Option("angle_deg", Default = 0.0f, HelpText = "Angle of view")]
        public float Angle { get; set; }

        [Option("gamma", Default = 2f, HelpText = "Value of gamma")]
        public float gamma { get; set; }
        
        [Option("factor", Default = 1f, HelpText = "Value of the factor for the clamp.image")]
        public float factor { get; set; }
        
        [Option("sample_per_pixel", Default = 1, HelpText = "Value for anti-aliasing")]
        public int spp { get; set; }
        
        [Option("renderer", Default = 'p', HelpText = "Type of render")]
        public char render { get; set; }
        
        [Option("max_depth", Default = 4 , HelpText = "Max depth for the rays")]
        public int max_depth { get; set; }
        
        [Option("n_rays", Default = 4, HelpText = "Numbers of rays")]
        public int n_rays { get; set; }
        
        [Option("rr_limit", Default = 3, HelpText = "Russian roulette limit")]
        public int rr_lim { get; set; }
        
        [Option("output_file", Default = "image.png", HelpText = "path + output file name + .png")]
        public string output { get; set; } = null!;
        
        [Option("output_pfm_file", Default = "output.pfm", HelpText = "path + output file name + .png")]
        public string output_pfm { get; set; } = null!;

        [Option("input_file", Default = "texture/FirstScene.txt", HelpText = "File .txt for the scene")]
        public string file_txt { get; set; } = null!;
    }

    static void RunOptionRender(render_option opts)
    {
        Console.WriteLine("RENDER");
        int w = opts.Width;
        int h = opts.Height;
        string file = opts.file_txt;
        int spp = opts.spp;
        char rend = opts.render;
        float factor = opts.factor;
        float gamma = opts.gamma;
        int nray = opts.n_rays;
        int max_depth = opts.max_depth;
        int rrlim = opts.rr_lim;
        string output_pfm = opts.output_pfm;
       
        Stream output_stream = File.OpenWrite(opts.output);
       
        IDictionary<string,float> myDic = ((EnumKeyword[])Enum.GetValues(typeof(EnumKeyword))).ToDictionary(k => k.ToString(), v => (float)v);

        RenderScene.ExecuteRender(file, w,h, output_pfm, output_stream, spp, rend, myDic,factor , gamma,max_depth,nray, rrlim );


    }
    static void HandleError(IEnumerable<Error> errors)
    {
        var sentenceBuilder = SentenceBuilder.Create();
        foreach (var error in errors)
            Console.WriteLine(sentenceBuilder.FormatError(error));
    }
    
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<DemoOption, render_option, pfm2png_option>(args)
            .WithParsed<DemoOption>(RunDemo)
            .WithParsed<render_option>(RunOptionRender)
            .WithParsed<pfm2png_option>(RunOptionPfm)
            .WithNotParsed(HandleError);
    }
}
