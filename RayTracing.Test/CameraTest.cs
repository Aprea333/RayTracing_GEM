namespace RayTracing.Test;

public class CameraTest
{
    private PerspectiveCamera cam = new PerspectiveCamera(1.0f, 2.0f);
    private Ray ray1 = cam.fire_ray(0.0f, 0.0f);
    private Ray ray2 = cam.fire_ray(1.0f, 0.0f);
    private Ray ray3 = cam.fire_ray(0.0f, 1.0f);
    private Ray ray4 = cam.fire_ray(1.0f, 1.0f);
    [Test]
    public void test_point()
    {
        Assert.True(Point.are_close(ray1.Origin, ray2.Origin));
        Assert.True(Point.are_close(ray1.Origin, ray3.Origin));
        Assert.True(Point.are_close(ray1.Origin, ray4.Origin));
    }
    
    [Test]
    public void test_point2()
    {
        Assert.True(Ray.are_close(ray1,ray2));
        Assert.True(Ray.are_close(ray1,ray3));
        Assert.True(Ray.are_close(ray1,ray4));
    }

    [Test]
    public void test_coord()
    {
        Assert.True(Point.are_close(ray1.At(1.0f), new Point(0.0f, 2.0f, -1.0f)));
        Assert.True(Point.are_close(ray2.At(1.0f), new Point(0.0f, -2.0f, -1.0f)));
        Assert.True(Point.are_close(ray3.At(1.0f), new Point(0.0f, 2.0f, 1.0f)));
        Assert.True(Point.are_close(ray4.At(1.0f), new Point(0.0f, -2.0f, 1.0f)));
    }

}
