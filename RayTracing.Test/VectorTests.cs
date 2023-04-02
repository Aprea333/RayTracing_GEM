namespace RayTracing.Test;

public class VectorTests
{
    public Vec v = new Vec(5, 6, 7);
    public Vec w = new Vec(2, 9, 5);

    [Test]
    public void are_close_test()
    {
        Assert.True(Vec.are_close(v, new Vec((float)5.000001, (float)6.00000001, (float)7.000001)));
        Assert.False(Vec.are_close(v, new Vec((float)5.01, (float)6.0, (float)7.0)));
        Assert.False(Vec.are_close(v, new Vec((float)5.0, (float)6.01, (float)7.0)));
        Assert.False(Vec.are_close(v, new Vec((float)5.0, (float)6.0, (float)7.01)));
    }

    [Test]
    public void VecSumTest()
    {
        Assert.True(Vec.are_close(v + w, new Vec(7, 15, 12)));
    }
    
    [Test]
    public void VecSubTest()
    {
        Assert.True(Vec.are_close(v - w, new Vec(3, -3, 2)));
    }

    [Test]
    public void ConstantProdTest()
    {
        Assert.True(Vec.are_close(17 * v, new Vec(85, 102, 119)));
        Assert.True(Vec.are_close(v * 17, new Vec(85, 102, 119)));
    }
    
    [Test]
    public void negTest()
    {
        Assert.True(Vec.are_close(v.neg(), new Vec(-v.X, -v.Y, -v.Z)));
    }
    
    [Test]
    public void ScalarProdTest()
    {
        Assert.True(Math.Abs(v*w - 99) < 0.0001);
    }
    
    [Test]
    public void CrossProductTest()
    {
        Assert.True(Vec.are_close(v ^ w, new Vec(-33, -11, 33)));
    }

    [Test]
    public void NormalizeTest()
    {
        Vec n = v;
        n.normalize();
        Assert.True(Math.Abs(n.squared_norm() - 1) < 0.0001);
    }

    
}