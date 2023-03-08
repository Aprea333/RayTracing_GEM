
using System.Runtime.CompilerServices;
public class Color
 {
     float r_c ;
     float g_c ;
     float b_c ;

     public Color()
     {
         r_c = 0;
         g_c = 0;
         b_c = 0;
     }
     public Color (float r, float g, float b)
     {
         r_c = r;
         g_c = g;
         b_c = b;
     }

     public void Stampa()
     {
         Console.WriteLine($"{r_c}, {g_c}, {b_c}");
     }

     public static Color operator +(Color a, Color b)
     {
         Color c = new Color();
         c.r_c = a.r_c + b.r_c;
         c.g_c = a.g_c + b.g_c;
         c.b_c = a.b_c + b.b_c;
         return c;
     }
     
     public static Color operator *(Color cc, float alpha)
     {
         cc.r_c *= alpha;
         cc.g_c *= alpha;
         cc.b_c *= alpha;

         return cc;
     }

     public static Color operator *(Color a, Color b)
     {
         Color prod = new Color();
         prod.r_c = a.r_c * b.r_c;
         prod.g_c = a.g_c * b.g_c;
         prod.b_c = a.b_c * b.b_c;
        
         return prod;
     }
 }

