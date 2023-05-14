namespace RayTracing.Test;

public class CameraTest
{
    
    [Test]
    public void TestPerspectiveCamera()
    {
        camera cam = new PerspectiveCamera(1.0f, 2.0f);
        Ray ray1 = cam.fire_ray(0.0f, 0.0f);
        Ray ray2 = cam.fire_ray(1.0f, 0.0f);
        Ray ray3 = cam.fire_ray(0.0f, 1.0f);
        Ray ray4 = cam.fire_ray(1.0f, 1.0f);

        //Verify that all the rays depart from the same origin
        Assert.True(Point.are_close(ray1.Origin, ray2.Origin), "Test origin ray1-2");
        Assert.True(Point.are_close(ray1.Origin, ray3.Origin), "Test origin ray1-3");
        Assert.True(Point.are_close(ray1.Origin, ray4.Origin), "Test origin ray1-4");

        //Verify that the ray hitting the corners have the right coordinates
        Assert.True(Point.are_close(ray1.At(1.0f), new Point(0f, 2.0f, -1.0f)), "Test point ray1");
        Assert.True(Point.are_close(ray2.At(1.0f), new Point(0f, -2.0f, -1.0f)), "Test point ray2");
        Assert.True(Point.are_close(ray3.At(1.0f), new Point(0f, 2.0f, 1.0f)), "Test point ray3");
        Assert.True(Point.are_close(ray4.At(1.0f), new Point(0f, -2.0f, 1.0f)), "Test point ray4");

        //Verify the transformation
        Tran T = Tran.Translation_matr(new Vec(0.0f, -2.0f, 0.0f));
        camera cam2 = new PerspectiveCamera(tran:T);
        Ray ray = cam2.fire_ray(0.5f, 0.5f);
        var v = ray.At(1.0f);
        Assert.True(Point.are_close(ray.At(1.0f), new Point(0f, -2.0f, 0f)), "Test transformation");
        
    }
    
    [Test]
    public void test_orthogonal_camera()
    {
        camera cam = new Orthogonal_Camera(aspect_ratio:2.0f);
        Ray ray1 = cam.fire_ray(0.0f, 0.0f);
        Ray ray2 = cam.fire_ray(1.0f, 0.0f);
        Ray ray3 = cam.fire_ray(0.0f, 1.0f);
        Ray ray4 = cam.fire_ray(1.0f, 1.0f);
        
        //Verify that the rays are parallel by verifying that the cross products are equal to zero
        Assert.True(Math.Abs((ray1.Dir ^ ray2.Dir).squared_norm()) < 0.00001);
        Assert.True(Math.Abs((ray1.Dir ^ ray3.Dir).squared_norm()) < 0.00001);
        Assert.True(Math.Abs((ray1.Dir ^ ray4.Dir).squared_norm()) < 0.00001);
        
        //Verify that the ray hitting the corners have the right coordinates
        Assert.True(Point.are_close(ray1.At(1.0f), new Point(0.0f, 2.0f, -1.0f)));
        Assert.True(Point.are_close(ray2.At(1.0f), new Point(0.0f, -2.0f, -1.0f)));
        Assert.True(Point.are_close(ray3.At(1.0f), new Point(0.0f, 2.0f, 1.0f)));
        Assert.True(Point.are_close(ray4.At(1.0f), new Point(0.0f, -2.0f, 1.0f)));
    }

    [Test]
    public void test_orthogonal_camera_transform()
    {
        Tran tr = Tran.Translation_matr(new Vec(0.0f, -1.0f, 0.0f) * 2.0f) * Tran.Rotation_z(90);
        Orthogonal_Camera cam = new Orthogonal_Camera(tr);

        Ray r = cam.fire_ray(0.5f, 0.5f);
        Assert.True(Point.are_close(r.At(1.0f), new Point(0.0f, -2.0f, 0.0f)));
    }
}
