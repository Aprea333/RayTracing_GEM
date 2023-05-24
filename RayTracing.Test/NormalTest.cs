using NUnit.Framework;

namespace RayTracing.Test;


public class NormalTest
{
    public Normal a = new Normal(0.4f, 1.3f, 0.7f);
    public float scal = 3.0f;
    public Vec v = new Vec(0.5f, 2.1f, 1.7f);
    public Normal b = new Normal(1.0f, 0.6f, 1.8f);
    public double epsilon = 1e-5;
    public Normal c = new Normal(2.4f, 1.2f, 0.8f);

    [Test]
    public void Costruttore()
    {
        Assert.True(Normal.are_close(new Normal(0.4f, 1.3f, 0.7f), a));
        Assert.False(Normal.are_close(new Normal(0.3f, 1.2f,0.5f), a));
    }
    
    [Test]
    public void neg_test()
    {
        Assert.True(Normal.are_close(new Normal(-0.4f, -1.3f, -0.7f), a.opposite_normal()));
        Assert.False(Normal.are_close(new Normal(-0.3f, -1.2f, -0.5f), a.opposite_normal()));
    }

    [Test]
    public void prod_scal_nor()
    {
        Normal b = new Normal(1.2f, 3.9f, 2.1f);
        Assert.True(Normal.are_close(scal*a, b));
        Assert.False(Normal.are_close(scal*a, new Normal(1f, 3f, 2f) ));
    }

    [Test]
    public void prod_scalar()
    {
        float f = 4.12f;
        Assert.True(Math.Abs(v*a - f)<epsilon);
        Assert.False(Math.Abs(v*a - f)>epsilon);
        Assert.True(Math.Abs(a*v - f)<epsilon);
        Assert.False(Math.Abs(a*v - f)>epsilon);
    }

    [Test]
    public void cross_prod()
    {
        Vec w = new Vec(-0.74f, 0.33f, -0.19f);
        Assert.True(Vec.are_close(v^a, w));
        Assert.False(Vec.are_close(v^a, new Vec(-0.6f, 0.5f, 0.3f)));
        Assert.True(Vec.are_close(a^v, w.neg()));
        Assert.False(Vec.are_close(a^v, new Vec(0.6f,  -0.5f,  - 0.3f)));
    }

    [Test]
    public void cross_prod_norm()
    {
        Normal c = new Normal(1.92f, -0.02f, -1.06f);
        Assert.True(Normal.are_close(a^b, c));
        Assert.False(Normal.are_close(a^b, new Normal(1.5f, -0.02f, -1.06f)));
    }

    [Test]
    public void squared_norm()
    {
        float f = 2.34f;
        Assert.True(Math.Abs(a.squared_norm() - f)< epsilon);
        Assert.False(Math.Abs(a.squared_norm() - f)> epsilon);
    }

    [Test]
    public void norm()
    {
        float f = 2.8f;
        Assert.True(Math.Abs(c.norm() - f) < epsilon);
        Assert.False(Math.Abs(c.norm() - f) > epsilon);
    }

    [Test]
    public void onb_creation()
    {
        PCG pcg = new PCG();
        for (int i = 0; i < 100; i++)
        {
            Normal n = new Normal(pcg.random_float(), pcg.random_float(), pcg.random_float());
            n.normalization();
            (Vec e1, Vec e2, Vec e3) =  Normal.create_onb_from_z(n);
           
            
            //Verify the normalization
            Assert.True(Math.Abs(e1.squared_norm()-1f)< epsilon, "Test normalized e1");
            Assert.True(Math.Abs(e2.squared_norm()-1f)< epsilon, "Test normalized e2");
            //Assert.True(Math.Abs(e3.squared_norm()-1f)< epsilon, "Test normalized e3");
            
            //Test orthogonal
            Assert.True(Math.Abs(e1*e2)<epsilon, "Test e1 orthogonal to e2");
            Assert.True(Math.Abs(e1*e3)<epsilon, "Test e1 orthogonal to e3");
            Assert.True(Math.Abs(e3*e2)<epsilon, "Test e2 orthogonal to e3");

        }
    }
   
    
}