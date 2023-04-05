namespace RayTracing;


public class Tran
{
    public float [] m;
    public float[] minv;

    
    /// <summary>
    /// Constructor for m and minv
    /// </summary>
    /// <param name="matr"></param>
    /// <param name="matr_inv"></param>
    public Tran(float[] matr, float[] matr_inv) // in questo modo si passa l'indirizzo, 
    {                                           // si puo fare anche l'assegnazione membro a membro
        if (matr.Length != 16 || matr_inv.Length != 16)
        {
            throw new Exception("Need 4x4 matrix, length must be 16!");
        }
        m = matr;
        minv = matr_inv;
    }

    public static Tran Translation_matr(Vec v)
    {
        float[] mat = new float[]{ 1, 0, 0, v.X, 0, 1, 0, v.Y, 0, 0, 1, v.Z, 0, 0, 0, 1 };
        float[] mat_1 = new float[] { 1, 0, 0, -v.X, 0, 1, 0, -v.Y, 0, 0, 1, -v.Z, 0, 0, 0, 1 };
        return new  Tran(mat, mat_1);
    }
    
    /// <summary>
    /// Matrix product
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public float [] matr_prod (float [] a, float [] b)
    {
        float[] prod = new float[16];
        for (int i = 0; i < 4; i++)
        {
            for (int l = 0; l < 4; l++)
            {
                for (int k = 0; k < 4; k++)
                {
                    prod[i * 4 + l] += a[i * 4 + k] * b[k * 4 + l];
                }
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
    

    public static Point Translation_Point(Tran T, Point p)
    {
        float x = p.X * T.m[0] + p.Y * T.m[1] + p.Z * T.m[2] + T.m[3];
        float y = p.X * T.m[4] + p.Y * T.m[5] + p.Z * T.m[6] + T.m[7];
        float z = p.X * T.m[8] + p.Y * T.m[9] + p.Z * T.m[10] + T.m[11];
        float w = p.X * T.m[12] + p.Y * T.m[13] + p.Z * T.m[14] + T.m[15];
        
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
    
public static Vec Translation_Vec(Tran T, Vec v)
{
    return v;
}
      
    
    /// <summary>
    /// Control element per element if 2 matrices are close up to 10^-5
    /// </summary>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public bool are_matr_close(float[] m1, float [] m2)
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
    public static bool Are_Tran_close(Tran T1, Tran T2)
    {
        
        for (int i = 0; i < 16; i++)
        {
            if (Math.Abs(T1.m[i] - T2.m[i]) > 1e-5 && Math.Abs(T1.minv[i]-T2.minv[i])>1e-5)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Check if m and minv are one the inverse of the other: the matrix product has to be
    /// the identity matrix 4x4
    /// </summary>
    /// <returns></returns>
    public bool is_consistent()
    {
        float[] prod = matr_prod(m, minv);
        return are_matr_close(prod, new float [] {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1} );
    }
    
    public Tran scale_matrix(float a, float b, float c)
    {
        return new Tran(new float[] { a, 0, 0, 0, 0, b, 0, 0, 0, 0, c, 0, 0, 0, 0, 0 },
            new float[] { 1 / a, 0, 0, 0, 0, 1 / b, 0, 0, 0, 0, 1 / c, 0, 0, 0, 0, 0 });
    }

    public Vec scale_transformation(Vec a)
    {
        Vec res = new Vec(a.X * m[0], a.Y * m[5], a.Z * m[10]);
        return res;
    }
    
    public Normal scale_transformation(Normal a)
    {
        Normal res = new Normal(a.x / m[0], a.y / m[5], a.z / m[10]);
        return res;
    }
    
    public Point scale_transformation(Point a)
    {
        Point res = new Point(a.X * m[0], a.Y * m[5], a.Z * m[10]);
        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Tran Rotation_x(float angle) //angle in deg
    {
        float rad = (float)(angle * Math.PI / 180.0);
        float[] mat = {1,0,0,0,(float)Math.Cos(rad),-(float)Math.Sin(rad),0,(float)Math.Sin(rad),(float)Math.Cos(rad)};
        float[] inv = {1,0,0,0,(float)Math.Cos(rad),(float)Math.Sin(rad),0,-(float)Math.Sin(rad),(float)Math.Cos(rad)};
        return new Tran(mat, inv);
    }
    
    public Tran Rotation_y(float angle) //angle in deg
    {
        float rad = (float)(angle * Math.PI / 180.0);
        float[] mat = {(float)Math.Cos(rad), 0, (float)Math.Sin(rad),0,1,0, -(float)Math.Sin(rad),0,(float)Math.Cos(rad)};
        float[] inv = {(float)Math.Cos(rad), 0, -(float)Math.Sin(rad),0,1,0, (float)Math.Sin(rad),0,(float)Math.Cos(rad)};
        return new Tran(mat, inv);
    }
    
    public Tran Rotation_z(float angle) //angle in deg
    {
        float rad = (float)(angle * Math.PI / 180.0);
        float[] mat = {(float)Math.Cos(rad), -(float)Math.Sin(rad), 0, (float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0, 0, 1};
        float[] inv = {(float)Math.Cos(rad), (float)Math.Sin(rad), 0, -(float)Math.Sin(rad), (float)Math.Cos(rad), 0, 0, 0, 1};
        return new Tran(mat, inv);
    }

}