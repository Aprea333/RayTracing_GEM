namespace RayTracing.Test;

public class TransformationTest
{
    public static float[] a = { 1, 2, 3, 1, 1, 1, 4, 1, 2, 4, 7, 4, 0, -3, 3, -1 };
    public static float[] b = { -18, 29, -5, -9, 5, -7, 1, 2, 4, -6, 1, 2, -3, 3, 0, -1 };
    public Tran tr = new Tran(a,b);

    [Test]
    public void constructor_test()
    {
        Tran prova;
        Assert.Throws<Exception>(() => prova = new Tran(new float[] { 0, 1 }, new float[] { 1, 0 }));
    }

    [Test]
    public void are_close_test()
    {
        float [] c = new float[] { 1, 2, 3, 1, 1, 1, 4, 1, 2, 4, 7, 4, 0, -3, 3, -1 };
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
}