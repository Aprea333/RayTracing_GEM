using System.ComponentModel.DataAnnotations;

namespace RayTracing.Test;

public class ImageTracerTest
{
    public delegate Colour F(Ray r);

    static HdrImage image = new HdrImage(4, 2);
    static PerspectiveCamera camera = new PerspectiveCamera(aspect_ratio: 2.0f);
    ImageTracer tracer = new ImageTracer(image, camera);

    [Test]
    public void TestOrientation()
    {
        //Fire a ray against top-left corner of the screen
        var topLeftRay = tracer.fire_ray(0, 0, 0.0f, 0.0f);
        Assert.True(Point.are_close(topLeftRay.at(1.0f),new Point(0.0f,2.0f,1.0f)), "Test ray top-left corner");
        //Fire a ray against bottom-right corner of the screen
        var bottomRightRay = tracer.fire_ray(3, 1, 1.0f, 1.0f);
        Assert.True(Point.are_close(bottomRightRay.at(1.0f),new Point(0.0f,-2.0f,-1.0f)), "Test ray bottom-right corner");
    }

    [Test]
    public void TestUvSubMapping()
    {
        var ray1 = tracer.fire_ray(0, 0, 2.5f, 1.5f);
        var ray2 = tracer.fire_ray(2, 1);
        Assert.True(Ray.are_close(ray1,ray2), "Test fire ray");
    }

    [Test]
    public void TestImageCoverage()
    {
        tracer.fire_all_rays(function.BaseColour);
        for (int i = 0; i < image.height; i++)
        {
            for (int j = 0; j < image.width; j++)
            {
                Assert.True(Colour.are_close(image.get_pixel(j,i), new Colour(0.1f, 0.2f,0.3f)), "Test fire_all_rays");
            }
        }
    }
    
        
        
        
    }
