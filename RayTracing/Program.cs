using System.IO.Compression;
using System.Runtime.InteropServices;
using RayTracing;
using SixLabors.ImageSharp.Formats.Png;
using CommandLine;

class Program
{
    [Verb("pfm2png", HelpText = "Add file contents to the index.")]
    class Options
    {
        [Option("--factor", Default = 0.2f, HelpText = "multiplicative factor")]
        public float Factor { get; set; }
        [Option("--gamma", Default = 1.0f, HelpText = "value to be used for gamma correction")]
        public float Gamma { get; set; }
        [Option("Input name", HelpText = "name of file to read")]
        public string Input { get; set; }
        [Option("Output name", HelpText = "name of file output")]
        public string Output { get; set; }
    }

    static void RunOptions(Options opts)
    {
        
    }
    
    int Main(string[] args)
    {
        return CommandLine.Parser.Default.ParseArguments<Options, CommitOptions, CloneOptions>(args)
            .MapResult<>(
                (Options opts) => RunOptions(opts),
                (CommitOptions opts) => RunCommitAndReturnExitCode(opts),
                (CloneOptions opts) => RunCloneAndReturnExitCode(opts),
                errs => 1);
    }

   
}


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
    }

    //main(args);*/
