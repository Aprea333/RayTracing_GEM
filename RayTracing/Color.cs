namespace RayTracing;

public class Colour
{
    public float r_c;
    public float g_c;
    public float b_c;

    public Colour()
    {
        r_c = 0;
        g_c = 0;
        b_c = 0;
    }

    public Colour(float r, float g, float b)
    {
        r_c = r;
        g_c = g;
        b_c = b;
    }
    
/// <summary>
/// Function that sums two colors
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
    public static Colour operator +(Colour a, Colour b)
    {
        Colour c = new Colour();
        c.r_c = a.r_c + b.r_c;
        c.g_c = a.g_c + b.g_c;
        c.b_c = a.b_c + b.b_c;
        return c;
    }
/// <summary>
/// Function that returns the product between a scalar and a color
/// </summary>
/// <param name="cc"></param>
/// <param name="alpha"></param>
/// <returns></returns>
    public static Colour operator *(Colour cc, float alpha)
    {
        Colour result = new Colour();
        result.r_c = cc.r_c * alpha;
        result.g_c = cc.g_c * alpha;
        result.b_c= cc.b_c * alpha;
        return result;
    }

    public static Colour operator *(float alpha, Colour cc)
    {
        Colour result = new Colour();
        result.r_c = cc.r_c * alpha;
        result.g_c = cc.g_c * alpha;
        result.b_c= cc.b_c * alpha;
        return result;
    }
/// <summary>
/// Function that print the value of the three value of color: Red, green and blue
/// </summary>
    public void print()
    {
        Console.WriteLine($"{r_c}, {g_c}, {b_c}");
    }
/// <summary>
/// function that returns the product between two colors
/// </summary>
/// <param name="a"></param>
/// <param name="b"></param>
/// <returns></returns>
    public static Colour operator *(Colour a, Colour b)
    {
        Colour prod = new Colour();
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
    public static bool are_close(Colour colore1, Colour colore2)
    {
             double epsilon = 1e-5;
             float diffRed = colore1.r_c - colore2.r_c;
             float diffGreen = colore1.g_c - colore2.g_c;
             float diffBlue = colore1.b_c - colore2.b_c;
             return Math.Sqrt(diffRed * diffRed + diffGreen * diffGreen + diffBlue * diffBlue) < epsilon;
       
    }

    /// <summary>
    /// Function that returns the luminosity of a pixel
    /// </summary>
    /// <returns></returns>
    public float luminosity()
    {
        return (Math.Max(Math.Max(r_c, g_c), b_c) +
                Math.Min(Math.Min(r_c, g_c), b_c)) / 2;
    }

}
    