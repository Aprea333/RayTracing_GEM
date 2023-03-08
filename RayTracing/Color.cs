
using System.Runtime.CompilerServices;

class Color
{
    public float r;
    public float g;
    public float b;

    public Color(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    public void stampa()
    {
        Console.WriteLine(r);
        Console.WriteLine(g);
        Console.WriteLine(b);
    }

    public static Color operator *(Color cc, float alpha)
    {
        cc.r *= alpha;
        cc.g *= alpha;
        cc.b *= alpha;

        return cc;
    }

    public static Color operator *(Color a, Color b)
    {
        Color prod = new Color(0f,0f, 0f);
        prod.r = a.r * b.r;
        prod.g = a.g * b.g;
        prod.b = a.b * b.b;
        
        return prod;
    }
    
    
    
}