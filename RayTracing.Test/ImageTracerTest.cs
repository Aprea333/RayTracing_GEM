using System.ComponentModel.DataAnnotations;

namespace RayTracing.Test;

public class ImageTracerTest
{
    public delegate Colore F(Ray r);
    
    [Test]
    public void image_test()
    {
        HDR image = new HDR(4, 2);
        camera cam = new PerspectiveCamera(Aspect_Ratio: 2);
        ImageTracer tracer = new ImageTracer(image, cam);
        Ray ray1 = tracer.fire_ray(0, 0, 2.5f, 1.5f);
        Ray ray2 = tracer.fire_ray(2, 1);
        Assert.True(Ray.are_close(ray1,ray2), "Test fire_ray");
        tracer.fire_all_rays(function.BaseColour);
        for (int i = 0; i < image.height; i++)
        {
            for (int j = 0; j < image.width; j++)
            {
                Assert.True(Colore.AreClose(image.get_pixel(j,i), new Colore(0.1f, 0.2f,0.3f)), "Test fire_all_rays");
            }
        }
        
        
        
    }
}