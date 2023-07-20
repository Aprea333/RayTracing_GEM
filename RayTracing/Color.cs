namespace RayTracing;

public struct Colour
{
    public float r_c;
    public float g_c;
    public float b_c;
    public static Colour white = new Colour(255,255,255);
    public static Colour black = new Colour(0,0,0);

    /// <summary>
    /// Color constructor
    /// </summary>
    public Colour()
    {
        r_c = 0;
        g_c = 0;
        b_c = 0;
    }

    /// <summary>
    /// Color constructor
    /// </summary>
    /// <param name="r">Value for the first component</param>
    /// <param name="g">Value for the second component</param>
    /// <param name="b">Value for the third component</param>
    public Colour(float r, float g, float b)
    {
        r_c = r;
        g_c = g;
        b_c = b;
    }
    
    /// <summary>
    /// Function that sums two colors
    /// </summary>
    /// <param name="a">First color</param>
    /// <param name="b">Second color</param>
    /// <returns>Color</returns>
    public static Colour operator +(Colour a, Colour b)
    {
        return new Colour(a.r_c + b.r_c,a.g_c + b.g_c,a.b_c + b.b_c);
    }

    /// <summary>
    /// Function that returns the product between a scalar and a color
    /// </summary>
    /// <param name="cc">Colour</param>
    /// <param name="alpha">Scalar</param>
    /// <returns>Colour</returns>
    public static Colour operator *(Colour cc, float alpha)
    {
        return new Colour(cc.r_c * alpha, cc.g_c * alpha,cc.b_c * alpha);
    }

    /// <summary>
    /// Function that returns the product between a scalar and a color
    /// </summary>
    /// <param name="cc">Colour</param>
    /// <param name="alpha">Scalar</param>
    /// <returns>Colour</returns>
    public static Colour operator *(float alpha, Colour cc)
    {
        return new Colour(cc.r_c * alpha, cc.g_c * alpha, cc.b_c * alpha);
    }

    /// <summary>
    /// Function that print the value of the three value of color: red, green and blue
    /// </summary>
    public void print()
    {
        Console.WriteLine($"{r_c}, {g_c}, {b_c}");
    }

    /// <summary>
    /// function that returns the product between two colours
    /// </summary>
    /// <param name="a">First colour</param>
    /// <param name="b">Second colours</param>
    /// <returns>Colour</returns>
    public static Colour operator *(Colour a, Colour b)
    {
        return new Colour(a.r_c * b.r_c,a.g_c * b.g_c,a.b_c * b.b_c);
    }

    /// <summary>
    /// Function that assures me that two colors are equal within an arbitrary confidence interval
    /// </summary>
    /// <param name="colore1">First colour</param>
    /// <param name="colore2">Second colour</param>
    /// <returns>True if are equals</returns>
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
    