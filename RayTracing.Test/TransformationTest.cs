﻿using System.Numerics;

namespace RayTracing.Test;

public class TransformationTest
{
    public static float[] a = { 1, 2, 3, 1, 1, 1, 4, 1, 2, 4, 7, 4, 0, -3, 3, -1 };
    public static float[] b = { -18, 29, -5, -9, 5, -7, 1, 2, 4, -6, 1, 2, -3, 3, 0, -1 };
    public Transformation tr = new(a,b);

    [Test]
    public void constructor_test()
    {
        Transformation prova;
        Assert.Throws<Exception>(() => prova = new Transformation(new float[] { 0, 1 }, new float[] { 1, 0 }));
    }

    [Test]
    public void are_close_test()
    {
        float [] c = { 1, 2, 3, 1, 1, 1, 4, 1, 2, 4, 7, 4, 0, -3, 3, -1 };
        float err1 = (float)0.001;

        for (int i = 0; i < 16; i++)
        {
            tr.m[i] += err1;
            Assert.False(Transformation.are_close(tr.m, c));
            tr.m[i] -= err1;
        }
        
        Assert.True(Transformation.are_close(tr.m, c));
    }

    [Test]
    public void matr_prod_test()
    {
        
        Assert.True(Transformation.are_close(Transformation.matrix_product(tr.m,tr.minv),new float []{1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1}));
    }

    [Test]
    public void is_consistent_test()
    {
        Assert.True(tr.is_consistent());
    }

    [Test]

    public void Translation_matr_test()
    {
        
        float[] matr = new float[] { 1.0f, 0, 0, 2.0f, 0f, 1f, 0, 3.0f, 0, 0, 1, 4.0f, 0, 0, 0, 1 };
        float[] matr1 = new float[] { 1, 0, 0, -2.0f, 0, 1, 0, -3.0f, 0, 0, 1, -4.0f, 0, 0, 0, 1 };
        
        Assert.True(Transformation.are_close(Transformation.translation(new Vec(2.0f, 3.0f, 4.0f)), new Transformation(matr,matr1)));
        Assert.False(Transformation.are_close(Transformation.translation(new Vec(4.0f, 3.0f, 4.0f)), new Transformation(matr,matr1)));
    }
    
    [Test]
    public void Translation_Point_Test()
    {
        float[] matr = { 1.0f, 0, 0, 2.0f, 0, 1f, 0, 3.0f, 0, 0, 1, 4.0f, 0, 0, 0, 1 };
        float[] matr1 = { 1, 0, 0, -2.0f, 0, 1f, 0, -3.0f, 0, 0, 1, -4.0f, 0, 0, 0, 1 };
        
        Assert.True(Point.are_close(Transformation.translation_point(new Transformation(matr,matr1),new Point(1.0f, 2.0f,3.0f)),new Point(3.0f,5.0f,7.0f)));
        Assert.False(Point.are_close(Transformation.translation_point(new Transformation(matr,matr1),new Point(1.0f, 2.0f,3.0f)),new Point(2.0f,5.0f,7.0f)));
    }
    [Test]
    public void Translation_Vec_Test()
    {
        Assert.True(Vec.are_close(Transformation.translation_vec(tr,new Vec(1.0f, 2.0f, 3.0f)),new Vec(1.0f,2.0f,3.0f)));
        Assert.False(Vec.are_close(Transformation.translation_vec(tr,new Vec(1.0f, 2.0f, 3.0f)),new Vec(4.0f,2.0f,3.0f)));
    }


    [Test]

    public void scale_test()
{
   
    Transformation tr1 = Transformation.scaling(3, 4, 5);
        
        float[] m1 = { 3, 0, 0, 0, 0, 4, 0, 0, 0, 0, 5, 0, 0, 0, 0, 1 };
        float[] m2 = { 1f/3f , 0.0f, 0f, 0.0f, 0.0f, 1.0f / 4.0f, 0f, 0f, 0f, 0f, 1f / 5f, 0f, 0f, 0f, 0f, 1f };
        
        Normal n = new Normal(3, 4, 7);
        Vec v = new Vec(3, 4, 7);
        Point p = new Point(3, 4, 7);
        
        Assert.True(Transformation.are_close(tr1.m, m1));
        Assert.True(Transformation.are_close(tr1.minv, m2));
        
       
        Assert.True(Math.Abs(tr1.scale_transformation(n).x - n.x * m2[0]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(n).y - n.y * m2[5]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(n).z - n.z * m2[10]) < 0.00001);
        
        Assert.True(Math.Abs(tr1.scale_transformation(v).x - v.x * m1[0]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(v).y - v.y * m1[5]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(v).z - v.z * m1[10]) < 0.00001);
        
        Assert.True(Math.Abs(tr1.scale_transformation(p).x - p.x * m1[0]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(p).y - p.y * m1[5]) < 0.00001);
        Assert.True(Math.Abs(tr1.scale_transformation(p).z - p.z * m1[10]) < 0.00001);


    }

    [Test]
    public void Rotation_test()
    {
        float alfa = 60.0f;
        float alfarad = alfa * (float)Math.PI / 180f;
        //Test to verify if the rotation matrix is equal to its inverse 
        Transformation test_x = Transformation.rotation_x(alfa);
        Transformation test_y = Transformation.rotation_y(alfa);
        Transformation test_z = Transformation.rotation_z(alfa);
        Assert.True(test_x.is_consistent());
        Assert.True(test_y.is_consistent());
        Assert.True(test_z.is_consistent());
        //Test to verify the rotation
        float cos = (float)Math.Cos(alfarad);
        float sin = (float)Math.Sin(alfarad);
        Point p = new Point(1.0f, 2.0f, 3.0f);
        Point px = test_x * p;
        Assert.True(Point.are_close(px, new Point(1,2*cos-3*sin,2*sin+3*cos)));
        Vec v = new Vec(1.0f, 2.0f, 3.0f);
        Vec vy = test_y * v;
        Assert.True(Vec.are_close(vy, new Vec(cos+3*sin,2,-sin+3*cos)));
        Normal n = new Normal(1.0f, 2.0f, 3.0f);
        Normal nz = test_z * n;
        Assert.True(Normal.are_close(nz, new Normal(cos-2*sin, sin+2*cos, 3.0f)));
    }

    /// <summary>
    /// Test to verify the operator multiplication between two transformations
    /// </summary>
    [Test]
    public void Mult_tran_tran()
    {
        float[] ma = { 1, 2, 3, 1, 1, 1, 4, 1, 2, 4, 7, 4, 0, -3, 3, -1 };
        float[] inv_a = { -18, 29, -5, -9, 5, -7, 1, 2, 4, -6, 1, 2, -3, 3, 0, -1 };
        float[] mb = { 1.0f, 0, 0, 2.0f, 0, 1f, 0, 3.0f, 0, 0, 1, 4.0f, 1, 0, 0, 1 };
        float[] inv_b = { -1, 0, 0, 2, -3, 1, 0, 3, -4, 0, 1, 4, 1, 0, 0, -1 };
        float[] mc = { 2,2,3,21,2,1,4,22,6,4,7,48,-1,-3,3,2};
        float[] inv_c = { 12,-23,5,7,50,-85,16,26,64,-110,21,34,-15,26,-5,-8};
        Transformation A = new Transformation(ma, inv_a);
        Transformation B = new Transformation(mb, inv_b);
        Transformation C = A*B;
        Assert.True(Transformation.are_close(mc, C.m));
        Assert.True(Transformation.are_close(inv_c, C.minv));

    }

    [Test]
    public void Mult_point_tran()
    {
        float[] matr = { 1.0f, 0, 0, 2.0f, 0, 1f, 0, 3.0f, 0, 0, 1, 4.0f, 0, 0, 0, 1 };
        float[] matr1 = { 1, 0, 0, -2.0f, 0, 1f, 0, -3.0f, 0, 0, 1, -4.0f, 0, 0, 0, 1 };
        Transformation T = new Transformation(matr, matr1);
        Point p = new Point(1.0f, 2.0f, 3.0f);
        Point r = T * p;
        Assert.True(Point.are_close(r, new Point(3.0f, 5.0f,7.0f)));
    }

    [Test]
    public void Mult_vec_tran()
    {
        Vec v = new Vec(1.0f, 2.0f, 3.0f);
        float[] matr = { 1.0f, 0, 0, 2.0f, 0, 5f, 0, 3.0f, 0, 0, 2, 4.0f, 0, 0, 0, 1 };
        float[] matr1 = { 1, 0, 0, -2.0f, 0, 1/5f, 0, -3/5f, 0, 0, 0.5f, -2.0f, 0, 0, 0, 1 };
        Transformation T = new Transformation(matr, matr1);
        Vec w = T * v;
        Assert.True(Vec.are_close(w,new Vec(1.0f,10.0f,6.0f)));
    }

    [Test]
    public void Mult_nor_tran()
    {
        float[] matr = { 1.0f, 0, 0, 2.0f, 0, 5f, 0, 3.0f, 0, 0, 2, 4.0f, 0, 0, 0, 1 };
        float[] matr1 = { 1, 0, 0, -2.0f, 0, 1/5f, 0, -3/5f, 0, 0, 0.5f, -2.0f, 0, 0, 0, 1 };
        Transformation T = new Transformation(matr, matr1);
        Normal n = new Normal(0.4f, 1.3f, 0.7f);
        Normal r = T * n;
        Assert.True(Normal.are_close(r, new Normal(0.4f, (float)1.3/5,0.35f)));
    }
}

