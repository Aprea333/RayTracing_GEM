using System.Runtime.InteropServices;
using RayTracing;

Colore c = new Colore(1.0f , 2.0f, 3.0f);
Colore d = new Colore(5.0f, 6.0f, 7.0f);
Console.WriteLine("Hello, World!");
Console.WriteLine("Hello, GEM!");
Colore s = new Colore();
Colore p = new Colore();
s = c + d;

Console.WriteLine(Colore.AreClose(d, d));

int a = 30;
int b = 40;


HDR culo = new HDR(a ,b);
Console.WriteLine(culo.parse_endianness_isLittle("-222"));
culo.set_pixel(s,3,4);
