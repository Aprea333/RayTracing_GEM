namespace RayTracing;

public class Colore
{
    public float r_c;
    public float g_c;
    public float b_c;

    public Colore()
    {
        r_c = 0;
        g_c = 0;
        b_c = 0;
    }

    public Colore(float r, float g, float b)
    {
        r_c = r;
        g_c = g;
        b_c = b;
    }
/// <summary>
/// Function that makes the sum of two colors
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
    public static Colore operator +(Colore a, Colore b)
    {
        Colore c = new Colore();
        c.r_c = a.r_c + b.r_c;
        c.g_c = a.g_c + b.g_c;
        c.b_c = a.b_c + b.b_c;
        return c;
    }
/// <summary>
/// function that returns the product between a scalar and a color
/// </summary>
/// <param name="cc"></param>
/// <param name="alpha"></param>
/// <returns></returns>
    public static Colore operator *(Colore cc, float alpha)
    {
        Colore result = new Colore();
        result.r_c = cc.r_c * alpha;
        result.g_c = cc.g_c * alpha;
        result.b_c= cc.b_c * alpha;
        return result;
    }

    public static Colore operator *(float alpha, Colore cc)
    {
        Colore result = new Colore();
        result.r_c = cc.r_c * alpha;
        result.g_c = cc.g_c * alpha;
        result.b_c= cc.b_c * alpha;
        return result;
    }
/// <summary>
/// Function that print the value of the three value of color: Red, green and blue
/// </summary>
    public void Stampa()
    {
        Console.WriteLine($"{r_c}, {g_c}, {b_c}");
    }
/// <summary>
/// function that returns the product between two colors
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
    public static Colore operator *(Colore a, Colore b)
    {
        Colore prod = new Colore();
        prod.r_c = a.r_c * b.r_c;
        prod.g_c = a.g_c * b.g_c;
        prod.b_c = a.b_c * b.b_c;

        return prod;
    }
/// <summary>
/// Function that assures me that two colors are equal within an arbitrary confidence interval
/// </summary>
/// <param name="colore1"></param>
/// <param name="colore2"></param>
/// <returns></returns>
    public static bool AreClose(Colore colore1, Colore colore2)
    {
        double epsilon = 1e-5;
        float diffRed = colore1.r_c - colore2.r_c;
        float diffGreen = colore1.g_c - colore2.g_c;
        float diffBlue = colore1.b_c - colore2.b_c;
        return Math.Sqrt(diffRed * diffRed + diffGreen * diffGreen + diffBlue * diffBlue) < epsilon;
    }

    /// <summary>
    /// Luminosità
    /// </summary>
    /// <returns></returns>
    public float Luminosity()
    {
        return (Math.Max(Math.Max(r_c, g_c), b_c) +
                Math.Min(Math.Min(r_c, g_c), b_c)) / 2;
    }

}
    