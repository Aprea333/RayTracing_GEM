namespace RayTracing.Test;

public class PathTracerTest
{
    [Test]
    public void FurnaceTest()
    {
        PCG pcg = new PCG();

        for (int i = 0; i <10; i++)
        {
            World wrl = new World();

            float emitted_radiance = pcg.random_float();
            float reflectance = pcg.random_float();
            Material enclosure_material = new Material(
                new DiffuseBrdf(new UniformPigment(new Colour(1.0f, 1.0f, 1.0f) * reflectance)),
                new UniformPigment(new Colour(1.0f, 1.0f, 1.0f) * emitted_radiance));
            
            wrl.add(new Sphere(material: enclosure_material));

            PathTracer path_tracer = new PathTracer(RandGen: pcg, NRays: 1, Wld: wrl, MaxDepth: 100,russianRoulette: 101);
            Ray ray = new Ray(new Point(0, 0, 0), new Vec(1, 0, 0));
            Colour color = path_tracer.tracing(ray);

            float expected = emitted_radiance / (1.0f - reflectance);
            Console.WriteLine($"stampami la differenza {Math.Abs(expected - color.r_c)}");
            Assert.True(Math.Abs(expected - color.r_c) < 0.01);
            Assert.True(Math.Abs(expected - color.b_c) < 0.01);
            Assert.True(Math.Abs(expected - color.g_c) < 0.01);
        }
    }
}