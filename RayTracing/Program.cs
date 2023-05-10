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
        [Option("--factor", Default = 0.2f, HelpText = "multiplicative factor")]
        public float Factor { get; set; }
        [Option("--gamma", Default = 1.0f, HelpText = "value to be used for gamma correction")]
        public float Gamma { get; set; }
        [Option("Input name", HelpText = "name of file to read")]
        public string Input { get; set; }
        [Option("Output name", HelpText = "name of file output")]
        public string Output { get; set; }
    }

    [Verb("add", HelpText = "Add file contents to the index.")]
    class AddOptions {
        [Option("stdin",
            Default = false,
            HelpText = "Read from stdin")]
        public bool stdin { get; set; }
    }
    [Verb("commit", HelpText = "Record changes to the repository.")]
    class CommitOptions {
        //commit options here
    }
    [Verb("clone", HelpText = "Clone a repository into a new directory.")]
    class CloneOptions {
        //clone options here
    }
    static void RunAddAndReturnExitCode(AddOptions opts)
    {
        if (opts.stdin)
        {
            Console.WriteLine("Culo");
        }
    }
    
    static void RunCommitAndReturnExitCode(CommitOptions opts)
    {
        //handle options
    }
    
    static void RunCloneAndReturnExitCode(CloneOptions opts)
    {
        //handle options
    }

    static void HandleError(IEnumerable<Error> errors)
    {
    }
    static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<AddOptions, CommitOptions, CloneOptions>(args)
            .WithParsed<AddOptions>(RunAddAndReturnExitCode)
            .WithParsed<CommitOptions>(RunCommitAndReturnExitCode)
            .WithParsed<CloneOptions>(RunCloneAndReturnExitCode)
            .WithNotParsed(HandleError);
    }
}

