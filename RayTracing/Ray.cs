using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.Formats.Png;

namespace RayTracing;

public struct Ray
{
    public Point origin;
    public Vec direction;
    public float t_min;
    public float t_max;
    public int depth;

    public Ray (Point origin, Vec dir, float tm= 1e-5f,float tM=Single.PositiveInfinity, int depth=0 )
    {
        this.origin = origin;
        direction = dir;
        t_min = tm;
        t_max = tM;
        this.depth = depth;
        
    }

    /// <summary>
    /// check that two rays have similare direction and origin
    /// </summary>
    /// <param name="r1"></param>
    /// <param name="r2"></param>
    /// <returns></returns>
      public static bool are_close(Ray r1, Ray r2)
      {
          return Vec.are_close(r1.direction, r2.direction) && Point.are_close(r1.origin, r2.origin);

      }

    public Point at (float t)
    {
        return origin + (direction * t);
    }

    public static Ray transform(Transformation T, Ray r1)
    {
        return new Ray(T * r1.origin, T * r1.direction, r1.t_min, r1.t_max, r1.depth);
        
    }
}