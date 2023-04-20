using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.Formats.Png;

namespace RayTracing;

public struct Ray
{
    public Point Origin;
    public Vec Dir;
    public float Tmin;
    public float Tmax;
    public int Depth;

    public Ray (Point origin, Vec dir, float tm= 1e-5f,float tM=Single.PositiveInfinity, int depth=0 )
    {
        Origin = origin;
        Dir = dir;
        Tmin = tm;
        Tmax = tM;
        Depth = depth;
        
    }

    /// <summary>
    /// check that two rays have similare direction and origin
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <returns></returns>
      public static bool are_close(Ray r1, Ray r2)
      {
          return Vec.are_close(r1.Dir, r2.Dir) && Point.are_close(r1.Origin, r2.Origin);

      }

    public Point At (float t)
    {
        return Origin + (Dir * t);
    }

    public static Ray Transform(Tran T, Ray r1)
    {
        return new Ray(T * r1.Origin, T * r1.Dir, r1.Tmin, r1.Tmax, r1.Depth);
        
    }
}