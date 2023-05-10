using System.IO.Compression;
using System.Runtime.InteropServices;
using RayTracing;
using SixLabors.ImageSharp.Formats.Png;
using CommandLine;







/*static void main(string[] args)
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
}*/

public class Program
{
    
    [Verb("pfm2png", HelpText = "Add file contents to the index.")]
    class Options
    {
        [Option("factor", Default = 0.2f, HelpText = "multiplicative factor")]
        public float Factor { get; set; }
        [Option("gamma", Default = 1.0f, HelpText = "value to be used for gamma correction")]
        public float Gamma { get; set; }
        [Option("Input name", HelpText = "name of file to read")]
        public string Input { get; set; }
        [Option("Output name", HelpText = "name of file output")]
        public string Output { get; set; }
    }

    static void RunoptionPfm(Options opts)
    {
        Console.WriteLine(opts.Factor);
        HDR img = new HDR();
        
        using (FileStream in_pfm = File.Open(opts.Input, FileMode.Open))
        {
            img.read_pfm_image(in_pfm);
        }

        Console.WriteLine($"File {opts.Input} has been read from disk");

        img.NormalizeImage(opts.Factor);
        img.clamp_image();

        File.CreateText(opts.Output).Close();
        Stream out_png = File.Open(opts.Output, FileMode.Open, FileAccess.Write, FileShare.None);
        img.write_ldr_image(out_png, ".png", opts.Gamma);
        Console.WriteLine($"File {opts.Output} has been written to disk");
        out_png.Close();
        
    }
    
    
    static void HandleError(IEnumerable<Error> errors)
    {
    }
    static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>( RunoptionPfm)
            .WithNotParsed(HandleError);
    }
}

