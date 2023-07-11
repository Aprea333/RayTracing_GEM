using System.Diagnostics;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using RayTracing;

namespace RayTracing;

public class RenderScene
{

    public class Converter
    {

        public static void ExecuteConvert(string inputpfm, Stream outputldr, float factor, float gamma,
            float? luminosity)
        {
            //string fmt = outputldr.Substring(outputldr.Length - 3, 3);

            Console.WriteLine("\n\nStarting file conversion using these parameters:\n");

            Console.WriteLine("pfmFile: " + inputpfm);
            Console.WriteLine("ldrFile: " + outputldr);
            Console.WriteLine("Factor: " + factor);
            Console.WriteLine("Gamma: " + gamma);
            Console.WriteLine(luminosity.HasValue ? ("Manual luminosity: " + luminosity) : "Average luminosity");

            Console.WriteLine("\n");

            HdrImage myImg = new HdrImage();

            try
            {
                using (FileStream inputStream = File.OpenRead(inputpfm))
                {
                    myImg.read_pfm_image(inputStream);
                    Console.WriteLine($"File {inputpfm} has been correctly read from disk.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Starting Tone Mapping...");
            try
            {
                Console.WriteLine(">>>> Normalizing image...");

                if (luminosity.HasValue) myImg.normalize_image(factor, luminosity.Value);
                else myImg.normalize_image(factor);

                Console.WriteLine(">>>> Clamping image...");
                myImg.clamp_image();

                Console.WriteLine(">>>> Saving LDR image...");

                myImg.write_ldr_image(outputldr, ".png", gamma);

                Console.WriteLine($"File {outputldr} has been correctly written to disk.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        } //Convert


    }





    public static void ExecuteRender(string file, int width, int height, string pfmFile,
        Stream ldrFile, int spp, char rend, IDictionary<string, float> variables,
        float factor, float gamma, int maxDepth, int nRays, int rrLimit)
    {
        Console.WriteLine("CHECK1");

        if (variables.Count == 0) Console.WriteLine("    - No Variables");
        foreach (var item in variables)
        {
            Console.WriteLine($"    - {item.Key} = {item.Value}");
        }

        //Scene scene = new Scene();
        //var root_directory = Environment.CurrentDirectory;
        //Console.WriteLine($"Root Dir: {root_directory}");
        //string path = Path.Combine(root_directory, file);
        
        Stream inputStream = File.OpenRead(file);
        InputStream input =  new InputStream(inputStream, file);
        Scene scene = Scene.parse_scene(input, variables);
          
        
        
        
       // InputStream input =  new InputStream(file_out, file);
        //scene = Scene.parse_scene(input, variables);
        
       /* using (InputStream inputSceneStream = new InputStream(file))
        {
            try
            {
                InputStream input = new InputStream(stream: inputSceneStream, file_name: file);
                scene = Scene.parse_scene(inputFile: input, variables: variables);
            }
            catch (GrammarError e)
            {
                SourceLocation loc = e.location;
                Console.WriteLine($"{loc.file_name}:{loc.line_num}:{loc.col_num}: {e.Message}");
                return;
            }
        }*/

       HdrImage image = new HdrImage(width, height);

        // Run the ray-tracer
        if (scene.Camera != null)
        {
            ImageTracer tracer = new ImageTracer(image, scene.Camera, (int)MathF.Sqrt(spp));

            Renderer renderer;
            if (rend == 'o')
            {
                Console.WriteLine("\nUsing on/off renderer:");
                renderer = new OnOffRenderer(world: scene.Wd);
            }
            else if (rend == 'f')
            {
                Console.WriteLine("\nUsing flat renderer:");
                renderer = new FlatRenderer(world: scene.Wd);
            }
            else if (rend == 'p')
            {
                Console.WriteLine("\nUsing PathTracer renderer:");
                renderer = new PathTracer(scene.Wd, NRays:nRays, russianRoulette: rrLimit, MaxDepth: maxDepth);
            }

            else
            {
                Console.WriteLine($"Unknown renderer: {rend}");
                return;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            tracer.fire_all_rays(renderer);



            Console.WriteLine("Saving in pfm format...");
            using (FileStream outpfmstream = File.OpenWrite(pfmFile))
            {
                image.write_pfm(outpfmstream, true);
                Console.WriteLine($"Image saved in {pfmFile}");
            }

            Converter.ExecuteConvert(pfmFile, ldrFile, factor, gamma, null);

            sw.Stop();
            TimeSpan ts = sw.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("\nRun Time: " + elapsedTime);
        }

        Console.WriteLine("See you next time!\n");
    }

    public static IDictionary<string, float> build_variable_table(List<string> declare)
    {
        var variables = new Dictionary<string, float>();
        foreach (var declaration in declare)
        {
            var parts = declaration.Split(":");
            if (parts.Length != 2)
            {
                Console.WriteLine($"Lunghezza ={parts.Length}");
                Console.WriteLine($"Stampami la lista che vedi {parts}");
                throw new GrammarError("Error, the definition does not follow the pattern NAME:VALUE");
            }

            var name = parts[0];
            var string_value = parts[1];
            float value;
            try
            {
                value = Convert.ToSingle(string_value);
            }
            catch
            {
                throw new GrammarError("Invalid floating-point");
            }

            variables[name] = value;
        }

        return variables;
    }
}


/*{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file">File .txt that describe the scene</param>
    /// <param name="width">Width of the image</param>
    /// <param name="height">Height of the image</param>
    /// <param name="pfm_file">Output file in pfm format</param>
    /// <param name="spp">Samples per pixel for antialiasing</param>
    /// <param name="render">Render type</param>
    /// <param name="variables">Dictionary</param>
    /// <param name="gamma"></param>
    /// <param name="max_depth"></param>
    /// <param name="n_rays"></param>
    /// <param name="rrlimit"></param>
    public static void ExecuteRenderer(string file, int width, int height, string pfm_file, int spp, char rend, Dictionary<string, float> variables, float gamma, int max_depth, int n_rays, int rrlimit)
    {
        if (variables.Count == 0) Console.WriteLine("No variables");
        //Type of variables
        foreach (var item in variables)
        {
            Console.WriteLine($"    -{item.Key} = {item.Value}");
        }

        Scene scene = new Scene();

        using (FileStream input_stream = File.OpenRead(file))
        {
            try
            {
                scene = Scene.parse_scene(new InputStream(input_stream, file), variables);
            }
            catch (GrammarError error)
            {
                SourceLocation location = error.location;
                Console.WriteLine($"{location.file_name}:{location.line_num}:{location.col_num}: {error.Message}");
                return;
            }
        }
        
        //Create image
        HdrImage image = new HdrImage(width, height);
        ImageTracer tracer = new ImageTracer(image, scene.Camera, (int)MathF.Sqrt(spp));
        Renderer renderer;
        if (rend == 'o')
        {
            Console.WriteLine("\nUsing on/off renderer:");
            renderer = new OnOffRenderer( scene.Wd);
        }
        else if (rend == 'f')
        {
            Console.WriteLine("\nUsing flat renderer:");
            renderer = new FlatRenderer(world: scene.Wd);
        }
        else if (rend == 'r')
        {
            Console.WriteLine("\nUsing a path tracer:");
            renderer = new PathTracer(scene.Wd, NRays: n_rays, MaxDepth: max_depth, russianRoulette: rrlimit);
            Console.WriteLine($">>>> Max depth: {((PathTracer)renderer).MaxDepth}");
            Console.WriteLine($">>>> Russian Roulette Limit: {((PathTracer)renderer).RussianRoulette}");
            Console.WriteLine($">>>> Number of rays: {((PathTracer)renderer).NRays}");
        }else
        {
            Console.WriteLine($"Unknown renderer: {rend}");
            return;
        }

        Stopwatch sw = new Stopwatch();
        tracer.fire_all_rays(renderer);
    }
}
*/
 //Main Funcs


       
