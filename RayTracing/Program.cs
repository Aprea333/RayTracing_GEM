using System.Runtime.InteropServices;
using RayTracing;
using SixLabors.ImageSharp.Formats.Png;

void main(string[] args)
{
    parameters = Parameters();
    HDR img = new HDR();
    try
    {
        parameters.parse_command_line(args);
    }
    catch (Exception e)
    {
        throw new Exception("Error: ", e);
    }

    using (FileStream in_pfm = File.Open(parameters.input_pfm_file_name, FileMode.Open))
    {
       
        img.read_pfm_image(in_pfm);
    }

    Console.WriteLine($"File {parameters.input_pfm_file_name} has been read from disk");
        
    img.NormalizeImage(parameters.factor);
    img.clamp_image();
    using (Stream out_pfm =
           File.Open(parameters.output_pfm_file_name, FileMode.Open, FileAccess.Write, FileShare.None))
    {
        img.write_ldr_image(out_pfm, parameters.factor, parameters.gamma);
    }

    Console.WriteLine($"File {parameters.output_pfm_file_name} has been written to disk");
}

main(args);