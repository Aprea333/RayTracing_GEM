namespace RayTracing.Test;

public class SphereTest
{
    [Test]
    public void sphere_test()
    {
        
        Ray ray1 = new Ray(new Point(0f, 0f, 2f), new Vec(0f, 0f, -1f));
        Sphere sph = new Sphere();
        var intersection1 = sph.ray_intersection(ray1);
        Assert.True(HitRecord.are_close((HitRecord)intersection1,
            new HitRecord(new Point(0f, 0f, 1f), new Normal(0f, 0f, 1f), new Vec2D(0.0f,0.0f), 1.0f, ray1)), "Test1");
        
        Ray ray2 = new Ray(new Point(3f, 0f, 0f), new Vec(-1f, 0f, 0f));
        var intersection2 = sph.ray_intersection(ray2);
        Assert.True(HitRecord.are_close((HitRecord)intersection2, new HitRecord(new Point(1f,0f,0f), new Normal(1f,0f,0f), new Vec2D(0f,0.5f),2.0f, ray2)), "Test2");
        
        Ray ray3 = new Ray(new Point(0f, 0f, 0f), new Vec(1, 0, 0));
        var intersection3 = sph.ray_intersection(ray3);
        Assert.True(HitRecord.are_close((HitRecord)intersection3, new HitRecord(new Point(1f,0f,0f), new Normal(-1f,0f,0f), new Vec2D(0f,0.5f),1.0f, ray3)), "Test3");
    }

    [Test]
    public void sphere_transformation()
    {
        Transformation t = Transformation.translation(new Vec(10f,0f,0f));
        Sphere sph = new Sphere(t);
        Ray r = new Ray(new Point(10f, 0f, 2f), new Vec(0f, 0f, -1f));
        var inter1 = sph.ray_intersection(r);
        Assert.True(HitRecord.are_close((HitRecord)inter1,
            new HitRecord(new Point(10f, 0f, 1f), new Normal(0f, 0f, 1f), new Vec2D(0.0f,0.0f), 1.0f, r)), "Test1");
        
        Ray r2 = new Ray(new Point(13f, 0f, 0f), new Vec(-1f, 0f, 0f));
        var inter2 = sph.ray_intersection(r2);
        Assert.True(HitRecord.are_close((HitRecord)inter2,
            new HitRecord(new Point(11f, 0f, 0f), new Normal(1f, 0f, 0f), new Vec2D(0.0f,0.5f), 2.0f, r2)), "Test2");

        Assert.Null(sph.ray_intersection(new Ray(new Point(0f,0f,2f),new Vec(0f,0f,-1f))), "Test no intersection");
        Assert.Null(sph.ray_intersection(new Ray(new Point(-10f,0f,0f),new Vec(0f,0f,-1f))), "Test no intersection");

    }
}