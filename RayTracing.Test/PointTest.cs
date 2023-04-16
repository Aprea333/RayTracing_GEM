namespace  RayTracing.Test;

public class PointTest
{
    public Point _p1 = new Point(1.0f,2.0f,3.0f);
    public Point _p2 = new Point(3.0f,4.0f,1.0f);
    public Vec _v = new Vec(2.0f, 3.0f, 4.0f);

    [Test]

    public void Constructor()
    {
        Assert.True(Point.are_close(_p1,new Point(1.0f,2.0f,3.0f)));
        Assert.False(Point.are_close(_p1,new Point(1.0f,7.0f,3.0f)));
        
    }
/// <summary>
/// check the sum betweeen a vector and a point, the result must be a point
/// </summary>
[Test]

public void Sum_PointVec()
{
    Assert.True(Point.are_close(_p1+_v,new Point(3.0f,5.0f,7.0f) ));
    Assert.False(Point.are_close(_p1+_v,new Point(7.0f,5.0f,7.0f) ));
    
}

/// <summary>
/// check the difference between two points, the result must be a vector
/// </summary>
[Test]

public void Diff_Points()
{
 
    Assert.True(Vec.are_close(_p1-_p2,new Vec(-2.0f,-2.0f,2.0f )));
    Assert.False(Vec.are_close(_p1-_p2,new Vec(2.0f,-2.0f,-2.0f )));
}

/// <summary>
/// Check the difference between point and a vectore, the result must be a point
/// </summary>
/// <returns></returns>
[Test]

public void Diff_VecPoint()
{
    Assert.True(Point.are_close(_p1-_v, new Point(-1.0f, -1.0f, -1.0f)));
    Assert.False(Point.are_close(_p1-_v, new Point(1.0f, 1.0f, 1.0f)));
}
/// <summary>
/// check that the function Convert(), convert a point into a vector
/// </summary>
[Test]

public void Convert()
{
    Assert.True(Vec.are_close(_p1.Convert(), new Vec(1.0f,2.0f,3.0f)));
    Assert.True(Vec.are_close(_p1.Convert(), new Vec(1.0f,2.0f,3.0f)));
}

}

