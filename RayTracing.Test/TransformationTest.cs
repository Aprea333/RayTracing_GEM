namespace RayTracing.Test;

public class TransformationTest
{
    public static float[] a = { 1, 2, 3, 1, 1, 1, 4, 1, 2, 4, 7, 4, 0, -3, 3, -1 };
    public static float[] b = { -18, 29, -5, -9, 5, -7, 1, 2, 4, -6, 1, 2, -3, 3, 0, -1 };
    public Tran tr = new(a,b);

    [Test]
    public void constructor_test()
    {
        Tran prova;
        Assert.Throws<Exception>(() => prova = new Tran(new float[] { 0, 1 }, new float[] { 1, 0 }));
    }

    [Test]
    public void are_close_test()
    {
        float [] c = { 1, 2, 3, 1, 1, 1, 4, 1, 2, 4, 7, 4, 0, -3, 3, -1 };
        float err1 = (float)0.001;

        for (int i = 0; i < 16; i++)
        {
            tr.m[i] += err1;
            Assert.False(tr.are_matr_close(tr.m, c));
            tr.m[i] -= err1;
        }
        
        Assert.True(tr.are_matr_close(tr.m, c));
    }

    [Test]
    public void matr_prod_test()
    {
        Assert.True(tr.are_matr_close(tr.matr_prod(tr.m,tr.minv), new float []{1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1}));
    }

    [Test]
    public void is_consistent_test()
    {
        Assert.True(tr.is_consistent());
    }

    [Test]
    public void scale_test()
    {
        Tran tr1 = tr.scale_matrix(3, 4, 5);
        
        float[] m1 = { 3, 0, 0, 0, 0, 4, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0 };
        float[] m2 = { 1f/3f , 0.0f, 0f, 0.0f, 0.0f, 1.0f / 4.0f, 0f, 0f, 0f, 0f, 1f / 5f, 0f, 0f, 0f, 0f, 0f };
        
        Normal n = new Normal(3, 4, 7);
        Vec v = new Vec(3, 4, 7);
        Point p = new Point(3, 4, 7);
        
        Assert.True(tr1.are_matr_close(tr1.m, m1));
        Assert.True(tr1.are_matr_close(tr1.minv, m2));
        
       
        Assert.True(Math.Abs(tr1.scale_transformation(n).x - n.x * m2[0]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(n).y - n.y * m2[5]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(n).z - n.z * m2[10]) < 0.00001);
        
        Assert.True(Math.Abs(tr1.scale_transformation(v).X - v.X * m1[0]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(v).Y - v.Y * m1[5]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(v).Z - v.Z * m1[10]) < 0.00001);
        
        Assert.True(Math.Abs(tr1.scale_transformation(p).X - p.X * m1[0]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(p).Y - p.Y * m1[5]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(p).Z - p.Z * m1[10]) < 0.00001);


    }
}