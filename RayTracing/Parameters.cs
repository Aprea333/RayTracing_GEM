namespace RayTracing;

public class Parameters
{
    public static string input_pfm_file_name = "";
    private float factor;
    private float gamma;
    public static string output_png_file_name = "";

    public void parse_command(string[] argv)
    {
        if (argv.Length != 5)
        {
            throw new ("Usage: main.cs INPUT_PFM_FILE FACTOR GAMMA OUTPUT_PNG_FILE ");
        }

        input_pfm_file_name = argv[1];
        try
        {
            factor = Convert.ToSingle(argv[2]);
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Invalid value, factor ({argv[2]}) must be a floating-point");
            throw;
        }

        try
        {
            gamma = Convert.ToSingle(argv[3]);
        }
        catch (ArgumentException)
        {
            Console.WriteLine($"Invalid value, gamma ({argv[3]}) must be a floating-point");
            throw;
        }

        output_png_file_name = argv[4];
    }
}