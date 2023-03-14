
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
    
    public static Colore operator +(Colore a, Colore b)
    {
        Colore c = new Colore();
        c.r_c = a.r_c + b.r_c;
        c.g_c = a.g_c + b.g_c;
        c.b_c = a.b_c + b.b_c;
        return c;
    }

    public static Colore operator *(Colore cc, float alpha)
    {
        cc.r_c *= alpha;
        cc.g_c *= alpha;
        cc.b_c *= alpha;
        return cc;
    }
    
    public static Colore operator *(float alpha, Colore cc)
    {
        cc.r_c *= alpha;
        cc.g_c *= alpha;
        cc.b_c *= alpha;
        return cc;
    }

    public void Stampa()
    { 
        Console.WriteLine($"{r_c}, {g_c}, {b_c}");
    }

    public static Colore operator *(Colore a, Colore b)
    {
        Colore prod = new Colore();
        prod.r_c = a.r_c * b.r_c;
        prod.g_c = a.g_c * b.g_c;
        prod.b_c = a.b_c * b.b_c;

        return prod;
    }

    public static bool AreClose(Colore colore1, Colore colore2)
    {
        double epsilon = 1e-5;
        float diffRed = colore1.r_c - colore2.r_c;
        float diffGreen = colore1.g_c - colore2.g_c;
        float diffBlue = colore1.b_c - colore2.b_c;
        return Math.Sqrt(diffRed * diffRed + diffGreen * diffGreen + diffBlue * diffBlue) < epsilon;
    }
}