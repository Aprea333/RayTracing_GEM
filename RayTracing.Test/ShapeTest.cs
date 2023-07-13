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
        Plane plane = new Plane(Transformation.translation(new Vec(0,0,0.5f)));

        Ray r = new Ray(new Point(0f, 0f, 10f), new Vec(0, 0, -1));
        var intersection1 = plane.ray_intersection(r);
        Assert.True(intersection1!=null, "Check intersection != 0");
        Assert.True(HitRecord.are_close(new HitRecord(new Point(0f,0f,0.5f), new Normal(0,0,1),new Vec2D(0,0),9.5f,r, new Material()), (HitRecord)intersection1));
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

public class BoxTest
{
    [Test]
    public void TestBox()
    {
        Box box = new Box();
        Ray r = new Ray(new Point(-5, 0, 0), new Vec(1, 0, 0));
        HitRecord? intersection = box.ray_intersection(r);
        Assert.True(intersection != null, "Check non null intersection with ray directed along the x-axis" );
        HitRecord hit = new HitRecord(new Point(-1, 0, 0), new Normal(-1, 0, 0), new Vec2D(0.125f, 0.5f), 4, r,
            new Material());
        Assert.True(HitRecord.are_close(hit, (HitRecord)intersection), "Test intersections are close");

        Ray r2 = new Ray(new Point(0, 0, 10), new Vec(0, 0, -1));
        HitRecord? intersection2 = box.ray_intersection(r2);
        Assert.True(intersection2 != null,"Check non null intersection with ray directed along the z-axis" );
        HitRecord hit2 = new HitRecord(new Point(0, 0, 1), new Normal(0, 0, 1), new Vec2D(0.875f, 0.5f), 9, r2,
            new Material());
        Assert.True(HitRecord.are_close((HitRecord)intersection2, hit2), "Test intersection along z-axis are close");

        Ray r3 = new Ray(new Point(0, 3, 0), new Vec(0, -1, 0));
        HitRecord? intersection3 = box.ray_intersection(r3);
        Assert.True(intersection3 != null, "Check non null intersection with ray directed along the z-axis" );
        HitRecord hit3 = new HitRecord(new Point(0, 1, 0), new Normal(0, 1, 0), new Vec2D(0.375f, (float)(2.5f/3)), 2, r3,
            new Material());
        Assert.True(HitRecord.are_close((HitRecord)intersection3, hit3), "Test intersection along y-axis are close");

        Ray r4 = new Ray(new Point(0, 3, 0), new Vec(0, 1, 0));
        HitRecord? intersection4 = box.ray_intersection(r4);
        Assert.True(intersection4 == null, "Check null intersection");
    }
}

public class CylinderTest
{
    [Test]
    public void TestCylinder()
    {
        Cylinder cylinder = new Cylinder();
        Ray r = new Ray(new Point(-5, 0, 0f), new Vec(1, 0, 0));
        HitRecord? intersection = cylinder.ray_intersection(r);
        Assert.True(intersection != null, "Intersection non null");
        HitRecord hit = new HitRecord(new Point(-1, 0, 0f), new Normal(1, 0, 0), new Vec2D(0.5f, 0.75f), 4, r, new Material());
        Normal p = intersection.Value.normal;
        Console.WriteLine($"x: {p.x}, y:{p.y}, z:{p.z}");
        Assert.True(Normal.are_close(intersection.Value.normal, hit.normal));
        
        Assert.True(HitRecord.are_close((HitRecord)intersection, hit), "Test1");

        Ray ray2 = new Ray(new Point(0, 0, 10), new Vec(0, 0, -1));
        HitRecord? inter2 = cylinder.ray_intersection(ray2);
        /*Assert.True(inter2 != null, "Intersection 2 non null");
        HitRecord hit2 = new HitRecord(new Point(0, 0, 0.5f), new Normal(0, 0, 1), new Vec2D(0.75f, 0.25f), 9.5f, ray2, new Material());
        Assert.True(HitRecord.are_close((HitRecord)inter2, hit2));

        Ray ray3 = new Ray(new Point(-5, 0, 0.6f), new Vec(1, 0, 0));
        HitRecord? inter3 = cylinder.ray_intersection(ray3);
        Assert.True(inter3 == null, "Test null");*/
    }

    /*[Test]
    public void TestTransformation()
    {
        var cyl = new Cylinder(Transformation.rotation_y(-90));
        Ray ray = new Ray(new Point(0.5f, 0, 2), new Vec(0, 0, -1));
        Ray r = Ray.transform(Transformation.rotation_y(-90), ray);
        
        HitRecord? intersection = cyl.ray_intersection(ray);
        Assert.True(intersection != null, "Intersection non null");
        HitRecord hit = new HitRecord(new Point(-0.5f, 0, -1f), new Normal(0, 0, -1), new Vec2D(0.5f, 0.5f), 1, ray,
            new Material());
        Assert.True(HitRecord.are_close(hit, (HitRecord)intersection));
        
        //Test second intersection
        var inter_list = cyl.ray_intersection_list(ray);
        Assert.True(inter_list.Count == 2, "Test list");
        HitRecord hit1 = new HitRecord(new Point(-0.5f, 0, 1f), new Normal(0, 0, -1), new Vec2D(0f, 0.5f), 3, ray,
            new Material());
        Assert.True(HitRecord.are_close(inter_list[0], hit), "Test first hit");
        Assert.True(HitRecord.are_close(hit1, inter_list[1]), "Test second hit");

    }*/
}