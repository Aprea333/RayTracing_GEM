using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace RayTracing;


public class Transformation
{
    public float [] m;
    public float[] minv;

    public Transformation()
    {
        m = new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
        minv = new float[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 };
    }
    
    
    
    /// <summary>
    /// Constructor for m and minv
    /// </summary>
    /// <param name="matr"></param>
    /// <param name="matr_inv"></param>
    public Transformation(float[] matr, float[] matr_inv) // in questo modo si passa l'indirizzo, 
    {                                           // si puo fare anche l'assegnazione membro a membro
        if (matr.Length != 16 || matr_inv.Length != 16)
        {
            throw new Exception("Need 4x4 matrix, length must be 16!");
        }
        m = matr;
        minv = matr_inv;
    }

    public static Transformation translation(Vec v)
    {
        float[] mat = new float[]{ 1, 0, 0, v.x, 0, 1, 0, v.y, 0, 0, 1, v.z, 0, 0, 0, 1 };
        float[] mat_1 = new float[] { 1, 0, 0, -v.x, 0, 1, 0, -v.y, 0, 0, 1, -v.z, 0, 0, 0, 1 };
        return new  Transformation(mat, mat_1);
    }

    /// <summary>
    /// Matrix product
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float [] matrix_product (float [] a, float [] b)
    {
        float[] prod = new float[16];
        for (int i = 0; i < 16; i++)
        {
            int j = i / 4;
            int c = j*4;
            for (int k = i%4; k < 16; k += 4)
            {
                prod[i] += a[c] * b[k];
                c++;
            }
        }
        return prod;
    }


    /// <summary>
    /// function that return the translation af a Point by matrix of translation 4x4;
    /// </summary>
    /// <param name="a"></param>
    /// <param name="T"></param>
    /// <param name="p"></param>
    /// <returns></returns>
    

    public static Point translation_point(Transformation T, Point p)
    {
        float x = p.x * T.m[0] + p.y * T.m[1] + p.z * T.m[2] + T.m[3];
        float y = p.x * T.m[4] + p.y * T.m[5] + p.z * T.m[6] + T.m[7];
        float z = p.x * T.m[8] + p.y * T.m[9] + p.z * T.m[10] + T.m[11];
        float w = p.x * T.m[12] + p.y * T.m[13] + p.z * T.m[14] + T.m[15];
        
        Point newp = new Point(x,y,z);
        
        if (Math.Abs(w - 1) < 1e-5)
        {
            return newp;
        }

        else
        {
            return new Point(x/w, y/w, z/w);
        }
        
    }
    
public static Vec translation_vec(Transformation T, Vec v)
{
    return v;
}
      
    
    /// <summary>
    /// Control element per element if 2 matrices are close up to 10^-5
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static bool are_close(float[] m1, float [] m2)
    {
        for (int i = 0; i < 16; i++)
        {
            if (Math.Abs(m1[i] - m2[i]) > 0.00001)
            {
                return false;
            }
        }
        return true;
    }
/// <summary>
/// Check that two transformation are similar into a confident number epsilon
/// </summary>
/// <param name="T1"></param>
/// <param name="T2"></param>
/// <returns></returns>
    public static bool are_close(Transformation T1, Transformation T2)
    {
        if (are_close(T1.m, T2.m) && are_close(T1.minv, T2.minv))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if m and minv are one the inverse of the other: the matrix product has to be
    /// the identity matrix 4x4
    /// </summary>
    /// <returns></returns>
    public bool is_consistent()
    {
        float[] prod = matrix_product(m, minv);
        return are_close(prod, new float [] {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1} );
    }
    
    public static Transformation scaling(float a, float b, float c)
    {
        return new Transformation(new float[] { a, 0, 0, 0, 0, b, 0, 0, 0, 0, c, 0, 0, 0, 0, 1 },
            new float[] { 1 / a, 0, 0, 0, 0, 1 / b, 0, 0, 0, 0, 1 / c, 0, 0, 0, 0, 1 });
    }

    public Vec scale_transformation(Vec a)
    {
        Vec res = new Vec(a.x * m[0], a.y * m[5], a.z * m[10]);
        return res;
    }
    
    public Normal scale_transformation(Normal a)
    {
        Normal res = new Normal(a.x / m[0], a.y / m[5], a.z / m[10]);
        return res;
    }
    
    public Point scale_transformation(Point a)
    {
        Point res = new Point(a.x * m[0], a.y * m[5], a.z * m[10]);
        return res;
    }

    /// <summary>
    /// Function that computes the rotation of an angle along x-axis 
    /// </summary>
    /// <param name="angle">angle in deg</param>
    /// <returns>the transformation due to rotation</returns>
    public static Transformation rotation_x(float angle) //angle in deg
    {
        float rad = (float)(angle * Math.PI / 180.0);
        float[] mat = {1,0,0,0,0,(float)Math.Cos(rad),-(float)Math.Sin(rad),0,0,(float)Math.Sin(rad),(float)Math.Cos(rad),0,0,0,0,1};
        float[] inv = {1,0,0,0,0,(float)Math.Cos(rad),(float)Math.Sin(rad),0,0,-(float)Math.Sin(rad),(float)Math.Cos(rad),0,0,0,0,1};
        return new Transformation(mat, inv);
    }
    
    /// <summary>
    /// Function that computes the rotation of an angle along y-axis 
    /// </summary>
    /// <param name="angle">angle in deg</param>
    /// <returns>the transformation due to rotation</returns>
    public static Transformation rotation_y(float angle) //angle in deg
    {
        float rad = (float)(angle * Math.PI / 180.0);
        float[] mat = {(float)Math.Cos(rad), 0, (float)Math.Sin(rad),0,0,1,0,0, -(float)Math.Sin(rad),0,(float)Math.Cos(rad),0,0,0,0,1};
        float[] inv = {(float)Math.Cos(rad), 0, -(float)Math.Sin(rad),0,0,1,0,0, (float)Math.Sin(rad),0,(float)Math.Cos(rad),0,0,0,0,1};
        return new Transformation(mat, inv);
    }
    
    /// <summary>
    /// Function that computes the rotation of an angle along z-axis 
    /// </summary>
    /// <param name="angle">angle in deg</param>
    /// <returns>the transformation due to rotation</returns>
    public static Transformation rotation_z(float angle) //angle in deg
    {
        float rad = (float)(angle * Math.PI / 180.0);
        float[] mat = {(float)Math.Cos(rad), -(float)Math.Sin(rad), 0,0, (float)Math.Sin(rad), (float)Math.Cos(rad), 0,0, 0, 0, 1,0,0,0,0,1};
        float[] inv = {(float)Math.Cos(rad), (float)Math.Sin(rad), 0,0, -(float)Math.Sin(rad), (float)Math.Cos(rad), 0,0,0,0, 1,0,0,0,0,1};
        return new Transformation(mat, inv);
    }
    
    /// <summary>
    /// Operator multiplication between two transformations
    /// </summary>
    /// <param name="A">first transformation</param>
    /// <param name="B">second transformation</param>
    /// <returns>transformation</returns>
    public static Transformation operator *(Transformation A, Transformation B)
    {
        float[] c1 = new float[16];
        float[] c2 = new float[16];
        
        c1 = matrix_product(A.m, B.m);
        c2 = matrix_product(B.minv, A.minv);
        Transformation C = new Transformation(c1, c2);
        return C;
    }

    /// <summary>
    /// Operator multiplication between a transformation and a point
    /// </summary>
    /// <param name="t">transformation on the left</param>
    /// <param name="p">point on the right</param>
    /// <returns>point</returns>
    public static Point operator *(Transformation t, Point p)
    {
        Point r = new Point
        {
            x = t.m[0] * p.x + t.m[1] * p.y + t.m[2] * p.z + t.m[3],
            y = t.m[4] * p.x + t.m[5] * p.y + t.m[6] * p.z + t.m[7],
            z = t.m[8] * p.x + t.m[9] * p.y + t.m[10] * p.z + t.m[11]
        };
        float w = t.m[12] * p.x + t.m[13] * p.y + t.m[14] * p.z + t.m[15];
        if (w != 1)
        {
            r.x = r.x / w;
            r.y = r.y / w;
            r.z = r.z / w;
        }

        return r;

    }

    /// <summary>
    /// Operator multiplication between a transformation and a vector
    /// </summary>
    /// <param name="t">transformation</param>
    /// <param name="v">vector</param>
    /// <returns>vector</returns>
    public static Vec operator *(Transformation t, Vec v)
    {
        Vec r = new Vec
        {
            x = t.m[0] * v.x + t.m[1] * v.y + t.m[2] * v.z,
            y = t.m[4] * v.x + t.m[5] * v.y + t.m[6] * v.z,
            z = t.m[8] * v.x + t.m[9] * v.y + t.m[10] * v.z
        };
        float w = t.m[12] * v.x + t.m[13] * v.y + t.m[14] * v.z;
        if (w != 0)
        {
            throw new Exception("Error: the fourth element must be equal to zero");
        }
        return r;
    }

    /// <summary>
    /// Operator multiplication between a transformation and a vector
    /// </summary>
    /// <param name="t"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static Normal operator *(Transformation t, Normal n)
    {
        Normal r = new Normal()
        {
            x = t.minv[0] * n.x + t.minv[4] * n.y + t.minv[8] * n.z,
            y = t.minv[1] * n.x + t.minv[5] * n.y + t.minv[9] * n.z,
            z = t.minv[2] * n.x + t.minv[6] * n.y + t.minv[10] * n.z
        };
        return r;
    }

    public Transformation inverse()
    {
        return new Transformation(this.minv, this.m);
    }
    

}