namespace RayTracing.Test;

public class SphereTest
{
    [Test]
    public void sphere_test()
    {
        Material mt = new Material();
        Ray ray1 = new Ray(new Point(0f, 0f, 2f), new Vec(0f, 0f, -1f));
        Sphere sph = new Sphere();
        var intersection1 = sph.ray_intersection(ray1);
        Assert.True(HitRecord.are_close((HitRecord)intersection1,
            new HitRecord(new Point(0f, 0f, 1f), new Normal(0f, 0f, 1f), new Vec2D(0.0f,0.0f), 1.0f, ray1, mt)), "Test1");
        
        Ray ray2 = new Ray(new Point(3f, 0f, 0f), new Vec(-1f, 0f, 0f));
        var intersection2 = sph.ray_intersection(ray2);
        Assert.True(HitRecord.are_close((HitRecord)intersection2, new HitRecord(new Point(1f,0f,0f), new Normal(1f,0f,0f), new Vec2D(0f,0.5f),2.0f, ray2, mt)), "Test2");
        
        Ray ray3 = new Ray(new Point(0f, 0f, 0f), new Vec(1, 0, 0));
        var intersection3 = sph.ray_intersection(ray3);
        Assert.True(HitRecord.are_close((HitRecord)intersection3, new HitRecord(new Point(1f,0f,0f), new Normal(-1f,0f,0f), new Vec2D(0f,0.5f),1.0f, ray3, mt)), "Test3");
    }

    [Test]
    public void sphere_transformation()
    {
        Material mt = new Material();
        Transformation t = Transformation.translation(new Vec(10f,0f,0f));
        Sphere sph = new Sphere(t);
        Ray r = new Ray(new Point(10f, 0f, 2f), new Vec(0f, 0f, -1f));
        var inter1 = sph.ray_intersection(r);
        Assert.True(HitRecord.are_close((HitRecord)inter1,
            new HitRecord(new Point(10f, 0f, 1f), new Normal(0f, 0f, 1f), new Vec2D(0.0f,0.0f), 1.0f, r, mt)), "Test1");
        
        Ray r2 = new Ray(new Point(13f, 0f, 0f), new Vec(-1f, 0f, 0f));
        var inter2 = sph.ray_intersection(r2);
        Assert.True(HitRecord.are_close((HitRecord)inter2,
            new HitRecord(new Point(11f, 0f, 0f), new Normal(1f, 0f, 0f), new Vec2D(0.0f,0.5f), 2.0f, r2, mt)), "Test2");

        Assert.Null(sph.ray_intersection(new Ray(new Point(0f,0f,2f),new Vec(0f,0f,-1f))), "Test no intersection");
        Assert.Null(sph.ray_intersection(new Ray(new Point(-10f,0f,0f),new Vec(0f,0f,-1f))), "Test no intersection");

    }
}

public class PlaneTest
{
    [Test]
    public void plane_test()
    {
        Plane plane = new Plane();

        Ray r = new Ray(new Point(0f, 0f, 1f), new Vec(0, 0, -1));
        var intersection1 = plane.ray_intersection(r);
        Assert.True(intersection1!=null, "Check intersection != 0");
        Assert.True(HitRecord.are_close(new HitRecord(new Point(0f,0f,0f), new Normal(0,0,1),new Vec2D(0,0),1f,r, new Material()), (HitRecord)intersection1));
        Assert.True(Vec2D.are_close(new Vec2D(0f,0f), intersection1.Value.surface_point), "Test UV coordinates");
    }

    [Test]
    public void plane_transformation()
    {
        Plane p = new Plane(Transformation.rotation_y(90f));
        Ray r = new Ray(new Point(1f, 0f, 0f), new Vec(-1f, 0f, 0f));
        var intersection = p.ray_intersection(r);
        
        Assert.True(HitRecord.are_close(new HitRecord(new Point(0f,0f,0f), new Normal(1,0,0),new Vec2D(0,0),1f,r, new Material()), (HitRecord)intersection), "Test transformation 1");

        Ray r2 = new Ray(new Point(0f, 0f, 1f), new Vec(0f, 0f, 1f));
        var intersection2 = p.ray_intersection(r2);
        Assert.False(intersection2!= null, "Test transformation 2");
    }

    
}