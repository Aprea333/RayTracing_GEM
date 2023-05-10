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
        public DemoOption(string camera)
        {
            Camera = camera;
        }

        [Option("width", Default = 640, HelpText = "Width of the image to render")]
            public int Width { get; set; }
        [Option("heigh", Default = 480, HelpText = "Height of the image to render")]
            public int Height { get; set; }
        [Option("angle_deg", Default = 0.0, HelpText = "Angle of view")]
            public float Angle { get; set; }
        [Option("camera", Default = "perspective", HelpText = "Type of camera")]
            public string Camera { get; set; }
    }

    static void RunDemo(DemoOption opts)
    {
        HDR image = new HDR(opts.Width, opts.Height);
        World world = new World();
        Sphere sphere = new Sphere();
        if (opts.Camera != "perspective")
        {
            ImageTracer trace= new ImageTracer(image, new Orthogonal_Camera());
        }
        else
        {
            ImageTracer trace = new ImageTracer(image, new PerspectiveCamera()); }
        

    }

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
        CommandLine.Parser.Default.ParseArguments<pfm2png_option,DemoOption>(args)
            .WithParsed<pfm2png_option>(RunOptionPfm)
            .WithParsed<DemoOption>(RunDemo)

            .WithNotParsed(HandleError);

        var result = CommandLine.Parser.Default.ParseArguments<pfm2png_option>(args);
        result.WithParsed<pfm2png_option>(RunOptionPfm);
        result.WithNotParsed(errors => {
            var sentenceBuilder = SentenceBuilder.Create();
            foreach (var error in errors)
                Console.WriteLine(sentenceBuilder.FormatError(error));
        });

    }
}

