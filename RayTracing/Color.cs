
 public class Color
 {
     float r_c ;
     float g_c ;
     float b_c ;
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
 }