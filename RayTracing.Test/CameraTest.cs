namespace RayTracing.Test;

public class CameraTest
{
    public static camera cam = new Orthogonal_Camera(aspect_ratio : 2.0f);
    
    public Ray ray1 = cam.fire_ray(0.0f, 0.0f); 
    public Ray ray2 = cam.fire_ray(1.0f, 0.0f);
    public Ray ray3 = cam.fire_ray(0.0f, 1.0f);
    public Ray ray4 = cam.fire_ray(1.0f, 1.0f);

    [Test]
    public void test_orthogonal_camera()
    {
        Assert.True(Math.Abs((ray1.Dir ^ ray2.Dir).squared_norm()) < 0.00001);
        Assert.True(Math.Abs((ray1.Dir ^ ray3.Dir).squared_norm()) < 0.00001);
        Assert.True(Math.Abs((ray1.Dir ^ ray4.Dir).squared_norm()) < 0.00001);
       
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
