namespace RayTracing.Test;

public class CsgTest
{
    [Test]
    public void UnionTest()
    {
        var sphere1 = new Sphere();
        var sphere2 = new Sphere(Transformation.translation(new Vec(0f, 0f, 0.5f)));
        //Union
        var csg = sphere1 + sphere2;
        var ray1 = new Ray(new Point(0f, 0f, -2f), new Vec(0f, 0f, 1f));
        var intersection1 = csg.ray_intersection(ray1);
        Assert.True(intersection1 != null, "Test intersection non null");
        Assert.True(
            HitRecord.are_close(
                new HitRecord(new Point(0f, 0f, -1f), new Normal(0f, 0f, -1f), new Vec2D(0f, 1f), 1f, ray1,
                    new Material()), (HitRecord)intersection1), "Test intersection close to the new hit record");

        var ray2 = new Ray(new Point(-2, 0, 1.6f), new Vec(-1, 0, 0));
        var intersection2 = csg.ray_intersection(ray2);
        Assert.True(intersection2 == null, "Test null intersection");
        
        //Test to verify the is_internal function
        Assert.True(csg.is_internal(new Point(0,0,0.7f)), "Test internal in sphere1");
        Assert.True(csg.is_internal(new Point(0.5f,0.2f,1.2f)), "Test internal in sphere2");
        Assert.True(csg.is_internal(new Point(0,0,0.8f)), "Test internal in union");
        Assert.False(csg.is_internal(new Point(0,0,1.6f)), "Test non internal in sphere2");
        Assert.False(csg.is_internal(new Point(0,0,-1.2f)), "Test non internal in sphere1");
    }
    
    [Test]
    public void DifferenceTest()
    {
        var sphere1 = new Sphere();
        var sphere2 = new Sphere(Transformation.translation(new Vec(0f, 0f, 0.5f)));
        //Difference
        var csg = sphere1 - sphere2;
        var ray1 = new Ray(new Point(0f, 0f, -2f), new Vec(0f, 0f, 1f));
        var intersection1 = csg.ray_intersection(ray1);
        Assert.True(intersection1 != null, "Test intersection non null");
        Assert.True(
            HitRecord.are_close(
                new HitRecord(new Point(0f, 0f, -1f), new Normal(0f, 0f, -1f), new Vec2D(0f, 1f), 1f, ray1,
                    new Material()), (HitRecord)intersection1), "Test intersection close to the new hit record");

        var ray2 = new Ray(new Point(-2, 0, 0.5f), new Vec(-1, 0, 0));
        var intersection2 = csg.ray_intersection(ray2);
        Assert.True(intersection2 == null, "Test null intersection");
        
        //Test to verify the is_internal function
        Assert.True(csg.is_internal(new Point(0,0,-0.7f)), "Test internal in sphere1");
        Assert.False(csg.is_internal(new Point(0f,0f,-0.4f)), "Test non internal in difference");
        Assert.False(csg.is_internal(new Point(0,0,0.8f)), "Test non internal in difference");
        Assert.True(csg.is_internal(new Point(0,0,-0.5f)), "Test internal in sphere1");
        
    }
    
    [Test]
    public void IntersectionTest()
    {
        var sphere1 = new Sphere();
        var sphere2 = new Sphere(Transformation.translation(new Vec(0f, 0f, 0.5f)));
        //Intersection
        var csg = sphere1 * sphere2;
        var ray1 = new Ray(new Point(0f, 0f, -2f), new Vec(0f, 0f, 1f));
        var intersection1 = csg.ray_intersection(ray1);
        Assert.True(intersection1 != null, "Test intersection non null");
        Assert.True(
            HitRecord.are_close(
                new HitRecord(new Point(0f, 0f, -0.5f), new Normal(0f, 0f, -1f), new Vec2D(0f, 1f), 1.5f, ray1,
                    new Material()), (HitRecord)intersection1), "Test intersection close to the new hit record");

        var ray2 = new Ray(new Point(-2, 0, 1.1f), new Vec(-1, 0, 0));
        var intersection2 = csg.ray_intersection(ray2);
        Assert.True(intersection2 == null, "Test null intersection");
        
        //Test to verify the is_internal function
        Assert.True(csg.is_internal(new Point(0,0,-0.3f)), "Test internal in sphere1");
        Assert.False(csg.is_internal(new Point(0f,0f,-0.6f)), "Test non internal in difference");
        Assert.False(csg.is_internal(new Point(0,0,1.01f)), "Test non internal in difference2");
        Assert.False(csg.is_internal(new Point(0,0,1.3f)), "Test non internal in sphere2");
        
    }
}