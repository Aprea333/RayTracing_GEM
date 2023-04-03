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
}