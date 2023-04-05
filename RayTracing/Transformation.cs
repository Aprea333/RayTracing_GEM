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

    public Tran Translation_matr(Vec v)
    {
        float[] m = new float[] { 1, 0, 0, v.X, 0, 1, 0, v.Y, 0, 0, 1, v.Z, 0, 0, 0, 1 };
        float[] invm = new float[] { 1, 0, 0, -v.X, 0, 1, 0, -v.Y, 0, 0, 1, -v.Z, 0, 0, 0, 1 };
        return new Tran(m, minv);
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
/// function that return the translation af a vector by matrix of translation 4x4;
/// </summary>
/// <param name="a"></param>
/// <param name="p"></param>
/// <returns></returns>
    public Point Translation_Point(Point p)
    {
        float x = p.X * m[0] + p.Y * m[1] + p.Z * m[2] + m[3];
        float y = p.X * m[4] + p.Y * m[5] + p.Z * m[6] + m[7];
        float z = p.X * m[8] + p.Y * m[9] + p.Z * m[10] + m[11];
        float w = p.X * m[12] + p.Y * m[13] + p.Z * m[14] + m[15];
        
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

public Vec Translation_Vec(Vec v)
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
    /// Check if m and minv are one the inverse of the other: the matrix product has to be
    /// the identity matrix 4x4
    /// </summary>
    /// <returns></returns>
    public bool is_consistent()
    {
        float[] prod = matr_prod(m, minv);
        return are_matr_close(prod, new float [] {1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1} );
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