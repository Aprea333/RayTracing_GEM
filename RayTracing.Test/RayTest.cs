using NUnit.Framework;
using SixLabors.ImageSharp.PixelFormats;

namespace RayTracing.Test;

public class RayTest
{
    
    private Ray R1 = new Ray(new Point(1.0f, 2.0f, 3.0f), new Vec(5.0f, 4.0f, -1.0f));
    private Ray R2= new Ray(new Point(1.0f, 2.0f, 3.0f), new Vec(5.0f, 4.0f, -1.0f));
    private Ray R3 = new Ray(new Point(5.0f, 1.0f, 4.0f), new Vec(3.0f, 9.0f, 4.0f));

    [Test]
    public void RayAreClose()
    {
       Assert.True(Ray.are_close(R1,R2));
       Assert.False(Ray.are_close(R2,R3));
    }

    [Test]

    public void At_test()
    {
        Assert.True(Point.are_close(R1.at(0.0f),R1.origin));
        Assert.False(Point.are_close(R1.at(1.0f),R1.origin));
        Assert.True(Point.are_close(R1.at(1.0f),new Point(6.0f,6.0f,2.0f)));
        Assert.False(Point.are_close(R1.at(1.0f),new Point(5.0f,4.0f,5.0f)));
        Assert.True(Point.are_close(R1.at(2.0f),new Point(11.0f,10.0f,1.0f)));
        Assert.False(Point.are_close(R1.at(2.0f),new Point(14.0f,10.0f,1.0f)));
        
    }

    [Test]

    public void Transform_test()
    {
        Ray r1 = new Ray(new Point(1.0f, 2.0f, 3.0f), new Vec(6.0f, 5.0f, 4.0f));
        Transformation T = Transformation.translation(new Vec(10.0f, 11.0f, 12.0f)) * Transformation.rotation_x(90.0f);
        
        Assert.True(Point.are_close(Ray.transform(T, r1).origin, new Point(11.0f, 8.0f, 14.0f)));
        Assert.True(Vec.are_close(Ray.transform(T, r1).direction,new Vec(6.0f,-4.0f,5.0f)));
    }
}

