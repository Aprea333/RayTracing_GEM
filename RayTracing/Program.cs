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
        public DemoOption()
        {
            
        }

        [Option("width", Default = 600, HelpText = "Width of the image to render")]
        public int Width { get; set; }

        [Option("height", Default = 600, HelpText = "Height of the image to render")]
        public int Height { get; set; }

        [Option("angle_deg", Default = 0.0f, HelpText = "Angle of view")]
        public float Angle { get; set; }

        [Option("camera", Default = "perspective", HelpText = "Type of camera")]
        public string Camera { get; set; }
        
        [Option("output_file", Default = "image.png", HelpText = "path + output file name + .png")]
        public string output { get; set; } = null!;


    }

    //====================================================================================================================
    // DEMO 10 SPHERES === DEMO 10 SPHERES === DEMO 10 SPHERES === DEMO 10 SPHERES === DEMO 10 SPHERES === DEMO 10 SPHERES
    // //=================================================================================================================
    static void RunDemo(DemoOption opts)
    {
        int w = opts.Width;
        int h = opts.Height;
        HdrImage image = new HdrImage(w, h);
        World world = new World();

        Colour c = new Colour(255f, 128f, 0f);//giallo
        Colour c5 = new Colour(0f, 255f, 1f);

        Colour c1 = new Colour(255f, 0f, 0f); //rosso
        // Colour c2 = new Colour(184f, 31f, 88f); //magenta
        Colour c3 = new Colour(0f, 0f, 255f);
        Colour lil1 = new Colour(0, 2, 206);
        Colour lil2 = new Colour(172, 2, 206);

        Material m1 = new Material(new DiffuseBrdf(new UniformPigment(c5)));

        Material m2 = new Material(new DiffuseBrdf(new CheckeredPigment(c1, c5)));

        Material m3 = new Material(new DiffuseBrdf(new CheckeredPigment(c, c5)));
        
        Material m4 = new Material(new DiffuseBrdf(new CheckeredPigment(lil1, lil2, 8)));
        
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
        }

        //Add four spheres
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, 0.5f, 0.0f)) * sphereScale, m2));
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, 0.0f, -0.5f)) * sphereScale, m5));
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, -0.5f, 0.0f)) * sphereScale, m2));
           world.add(new Sphere(Transformation.translation(new Vec(0.0f, 0.0f, +0.5f)) * sphereScale, m5));
           world.add(new Sphere(Transformation.translation(new Vec(0.5f, 0.0f, 0f)) * sphereScale, m5));

           world.add(new Plane(Transformation.translation(new Vec(0f,0f,-0.7f)),m3));
            Transformation cam_tr = Transformation.rotation_z(opts.Angle) *
                                    Transformation.translation(new Vec(0.55f, 0f, 0f));
            Camera cam = new PerspectiveCamera(aspect_ratio: (float)opts.Width/opts.Height, tran: cam_tr);
            if (opts.Camera != "perspective")
            {
                cam = new OrthogonalCamera(aspect_ratio: (float)opts.Width / opts.Height, transformation: cam_tr);
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

    //=======================================================================
    // DEMO COLORED SPHERES === DEMO COLORED SPHERES === DEMO COLORED SPHERES
    //=======================================================================
    static void RunDemo2(DemoOption opts)
    {
        int w = opts.Width;
        int h = opts.Height;
        HdrImage image = new HdrImage(w, h);
        World world = new World();

        Colour yellow = new Colour(255f, 128f, 0f);
        Colour green = new Colour(0f, 255f, 1f);
        Colour red = new Colour(255f, 0f, 0f);
        Colour blue = new Colour(0, 0, 255);

        Material uni_green = new Material(new DiffuseBrdf(new UniformPigment(green)));
        Material yel_red = new Material(new DiffuseBrdf(new CheckeredPigment(yellow, red, 8)));
        Material blu_black = new Material(new DiffuseBrdf(new CheckeredPigment(blue, Colour.black)));


        Transformation sphereScale = Transformation.scaling(0.1f, 0.1f, 0.1f);
        for (float x = -0.5f; x <= 0.5f; x += 1f)
        {
            for (float y = -0.5f; y <= 0.5f; y += 1f)
            {
                for (float z = -0.5f; z <= 0.5f; z += 1f)
                {
                    world.add(new Sphere(Transformation.translation(new Vec(x, y, z)) * sphereScale, blu_black));
                }
            }


            //Add three spheres
            world.add(new Sphere(Transformation.translation(new Vec(0f, 0.5f, 0f)) * sphereScale, yel_red));
            world.add(new Sphere(Transformation.translation(new Vec(0f, 0f, -0.5f)) * sphereScale, uni_green));
            Transformation cam_tr = Transformation.rotation_z(opts.Angle) *
                                    Transformation.translation(new Vec(-1f, 0f, 0f));
            Camera cam = new PerspectiveCamera(aspect_ratio: (float)opts.Width / opts.Height, tran: cam_tr);
            if (opts.Camera != "perspective")
            {
                cam = new OrthogonalCamera(aspect_ratio: (float)opts.Width / opts.Height, transformation: cam_tr);
            }

            ImageTracer imageTracer = new ImageTracer(image, cam);

            Renderer rend = new FlatRenderer(world);
            imageTracer.fire_all_rays(rend);

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

    //=======================================================================
    // DEMO EARTH === DEMO EARTH === DEMO EARTH === DEMO EARTH === DEMO EARTH
    //=======================================================================
        static void RunDemo3(DemoOption opts)
        {
            int w = opts.Width;
            int h = opts.Height;
            HdrImage image = new HdrImage();
            World world = new World();
            string root_directory = Environment.CurrentDirectory;
            //string root_directory = @"C:\Users\miche\RayTracing_GEM\RayTracing\ImagePfm";
            //string input_pfm = Path.Combine(@"C:\Users\miche\RayTracing_GEM\","world.pfm");
            string input_pfm = "world.pfm";
            //Console.WriteLine("\ntest1");

            using (FileStream inputStream = File.OpenRead(input_pfm))
            {
                //Console.WriteLine("\ntest2");
                image.read_pfm_image(inputStream);
                //Console.WriteLine("\ntest3");
                Console.WriteLine($"Texture image has been correctly read from disk");
            }

            
            Colour c1 = new Colour(255f, 128f, 0f);
            Colour c2 = new Colour(0f, 255f, 1f);

            Material m = new Material(new DiffuseBrdf(new CheckeredPigment(c1, c2, 6)));
            Material earth = new Material(new DiffuseBrdf(new ImagePigment(image)));

            Transformation sphereScale = Transformation.scaling(0.7f, 0.7f, 0.7f);
            world.add(new Sphere(Transformation.translation(new Vec(0f, 0f, 0f)) * sphereScale, earth));
            //world.add(new Plane(Transformation.translation(new Vec(0f, -0.7f, -07f)), milky));

            Transformation cam_tr = Transformation.rotation_z(opts.Angle) *
                                    Transformation.translation(new Vec(-1f, 0f, 0f));
            Camera cam = new PerspectiveCamera(aspect_ratio: (float)opts.Width / opts.Height, tran: cam_tr);

            HdrImage img = new HdrImage(w, h);
            ImageTracer imageTracer = new ImageTracer(img, cam);


            Renderer rend = new FlatRenderer(world);
            imageTracer.fire_all_rays(rend);


            //Console.WriteLine("\ntest4");

            string path = "./image.pfm";
            File.CreateText(path).Close();
            Stream file_out = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None);
            imageTracer.Image.write_pfm(file_out, true);
            file_out.Close();

            //Console.WriteLine("\ntest5");

            using (FileStream in_pfm = File.Open(path, FileMode.Open))
            {
                img.read_pfm_image(in_pfm);
            }
            
            //Console.WriteLine("\ntest8");

            img.clamp_image();
            File.CreateText(opts.output).Close();
            Stream out_png = File.Open(opts.output, FileMode.Open, FileAccess.Write, FileShare.None);
            img.write_ldr_image(out_png, ".png", 2f);
            out_png.Close();
            

        }

        //========================================================================
        // RANDOM SPHERES === RANDOM SPHERES === RANDOM SPHERES === RANDOM SPHERES
        //========================================================================
        static void RunDemo_cas(DemoOption opts)
        {
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
        
            ImageTracer imageTracer = new ImageTracer(image, cam, sample_per_side: 4);
            Renderer rend = new PathTracer(world, new Colour(1, 1, 1), new PCG(), 5, 4, 3);
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
    //===============================================================================
    // DEMO 2 PLANES 2 SPHERES === DEMO 2 PLANES 2 SPHERES === DEMO 2 PLANES 2 SPHERES
    //===============================================================================
    
     [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Single[]; size: 26580MB")]
     [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Single[]; size: 461969MB")]
     static void RunDemo4(DemoOption opts)
    {
        //=============================================================================
        // CASUAL === CASUAL === CASUAL === CASUAL === CASUAL === CASUAL === CASUAL ===
        //=============================================================================
        /*

        HdrImage img = new HdrImage();

        */


        
        //=============================================================================
        // DA TXT === DA TXT === DA TXT === DA TXT === DA TXT === DA TXT === DA TXT ===
        //=============================================================================
        
         
        //string file = "texture/FirstScene.txt";
        //string file = "texture/SecondScene.txt";
        
         string file = "texture/Cornell.txt";
        
        int w = opts.Width;
        int h = opts.Height;
        World world = new World();
        HdrImage image = new HdrImage(w,h);


        

        string output_pfm = "output.pfm";
       
        Stream output_stream = File.OpenWrite(opts.output);
        
        IDictionary<string,float> myDic = ((EnumKeyword[])Enum.GetValues(typeof(EnumKeyword))).ToDictionary(k => k.ToString(), v => (float)v);

        

        RenderScene.ExecuteRender(file, w,h, output_pfm, output_stream, 1, 'p', myDic,1f , 2f,4,4, 3 );


        /*
        Colour beige = new Colour(1, 0.9f, 0.5f);

        Colour green = new Colour(0.3f, 0.5f, 0.1f);
        Colour blue = new Colour(0.1f, 0.2f, 0.5f);
        Colour navy = new Colour(0.3f, 0.4f, 0.8f);
        Colour red = new Colour(0.6f, 0.2f, 0.3f);

        //Materials
        Material sky = new Material(new DiffuseBrdf(new UniformPigment(Colour.black)),
            new UniformPigment(beige));

        Material ground = new Material(new DiffuseBrdf(new CheckeredPigment(green, blue)));
        Material sphere_material = new Material(new DiffuseBrdf(new UniformPigment(navy)));
        Material mirror_material = new Material(new SpecularBrdf(new UniformPigment(red)));
        
        //Add shapes
        world.add(new Sphere(Transformation.scaling(210f,210f,210f)*Transformation.translation(new Vec(0,0,0.4f)), sky));
        //world.add(new Plane(material:ground));
        //world.add(new Sphere(Transformation.translation(new Vec(0,0,1)), sphere_material));
        //world.add(new Box(tran:Transformation.scaling(0.5f,0.5f,0.5f), material:mirror_material));
        //world.add(new Sphere(Transformation.translation(new Vec(1f,2.5f,0)), mirror_material));
        //world.add(new Cylinder(new Transformation(), sphere_material));
 
        Transformation cam_tr = Transformation.rotation_z(opts.Angle) * Transformation.translation(new Vec(-2f, 0f, 0f));
        Camera cam;
        if (opts.Camera != "perspective")
        {
            cam = new OrthogonalCamera(aspect_ratio: (float)opts.Width / opts.Height, transformation: cam_tr);
        }
        else
        {
            cam = new PerspectiveCamera(aspect_ratio: (float)opts.Width / opts.Height, tran: cam_tr);
        }
           
        
        ImageTracer imageTracer = new ImageTracer(image, cam, sample_per_side: 1);
        Renderer rend = new FlatRenderer(world);
        //Renderer rend = new PathTracer(world, Colour.black, NRays:3 , MaxDepth: 2);
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
        img.write_ldr_image(out_png, ".png", 0.5f);
        out_png.Close();
*/
    }
     
     //==============================================================================
    // DEMO BOX === DEMO BOX === DEMO BOX === DEMO BOX === DEMO BOX === DEMO BOX === 
    //===============================================================================
    
     static void RunDemo5(DemoOption opts)
    {
        int w = opts.Width;
        int h = opts.Height;
        HdrImage image = new HdrImage(w, h);
        World world = new World();

        Colour beige = new Colour(1, 0.9f, 0.5f);
        Colour green = new Colour(0.3f, 0.5f, 0.1f);
        

        //Materials
        
        Material box_material = new Material(new DiffuseBrdf(new UniformPigment(beige)));
        Material plane_material = new Material(new DiffuseBrdf(new UniformPigment(green)));
        
        //Add shapes
        Point min = new Point(-1, -1, -1);
        Point max = new Point(1, 1, 1);
        world.add(new Cylinder(Transformation.translation(new Vec(1.5f,0,1.3f))*Transformation.scaling(0.5f,0.5f,2f), box_material));
        //world.add(new Box(min: min, max: max, tran: Transformation.scaling(0.3f,0.3f,0.3f), material: box_material));


        Transformation cam_tr = Transformation.rotation_z(opts.Angle) * Transformation.translation(new Vec(-2f, 0f, 1f));
        Camera cam;
        if (opts.Camera != "perspective")
        {
            cam = new OrthogonalCamera(aspect_ratio: (float)opts.Width / opts.Height, transformation: cam_tr);
        }
        else
        {
            cam = new PerspectiveCamera(aspect_ratio: (float)opts.Width / opts.Height, tran: cam_tr);
        }

        ImageTracer imageTracer = new ImageTracer(image, cam);

        Renderer rend = new FlatRenderer(world);
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

    static void HandleError(IEnumerable<Error> errors)
    {
        var sentenceBuilder = SentenceBuilder.Create();
        foreach (var error in errors)
            Console.WriteLine(sentenceBuilder.FormatError(error));
    }
    
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments< DemoOption>(args)
            //.WithParsed<pfm2png_option>(RunOptionPfm)
            .WithParsed<DemoOption>(RunDemo_cas)

            .WithNotParsed(HandleError);
    }
}

//pfm2png  --input_file image.pfm
