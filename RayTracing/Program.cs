using System.IO.Compression;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;
using RayTracing;
using SixLabors.ImageSharp.Formats.Png;
using CommandLine;


/*

class Program
{
    static void main(string[] args)
    {
        Parameters param = new Parameters();
        HDR img = new HDR();
        try
        {
            param.parse_command(args);
        }
        catch (Exception e)
        {
            throw new Exception("Error: ", e);
        }

        using (FileStream in_pfm = File.Open(param.input_pfm_file_name, FileMode.Open))
        {
            img.read_pfm_image(in_pfm);
        }

        Console.WriteLine($"File {param.input_pfm_file_name} has been read from disk");
        
        img.NormalizeImage(param.factor);
        img.clamp_image();
    
        File.CreateText(param.output_png_file_name).Close();
        Stream out_png = File.Open(param.output_png_file_name, FileMode.Open, FileAccess.Write, FileShare.None);
        img.write_ldr_image(out_png, ".png", param.gamma);
        Console.WriteLine($"File {param.output_png_file_name} has been written to disk");
        out_png.Close();
    
    
    }
}

main(args);
*/

public class Program
{

    [Verb("demo", HelpText = "Demo")]
    class demo_option
    {
        [Option("width", Default = 640, HelpText = "Width of the image to render")]
            public int Width { get; set; }
        [Option("heigh", Default = 480, HelpText = "Height of the image to render")]
            public int Height { get; set; }
        [Option("angle_deg", Default = 0.0, HelpText = "Angle of view")]
            public float Angle { get; set; }
        [Option("camera", Default = "perspective", HelpText = "Type of camera")]
            public string Camera { get; set; }
    }

    static void RunOptionDemo(demo_option opts)
    {
        if (opts.Camera != "perspective")
        {
            camera cam = new Orthogonal_Camera(aspect_ratio:opts.Width/opts.Height);
        }
        else
        {
            camera cam = new PerspectiveCamera();
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
        Console.WriteLine("culo");
        var sentenceBuilder = SentenceBuilder.Create();
        foreach (var error in errors)
            Console.WriteLine(sentenceBuilder.FormatError(error));
    }

    static void Main(string[] args)
    {
        var result = CommandLine.Parser.Default.ParseArguments<pfm2png_option>(args);
        result.WithParsed<pfm2png_option>(RunOptionPfm);
        result.WithNotParsed(errors => {
            var sentenceBuilder = SentenceBuilder.Create();
            foreach (var error in errors)
                Console.WriteLine(sentenceBuilder.FormatError(error));
        });
    }
}

