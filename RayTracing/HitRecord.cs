using System.Numerics;

namespace RayTracing;

public struct HitRecord
{
    public Point world_point { get; set; }
    public Normal normal { get; set; }
    public Vec2D surface_point { get; set; }
    public float t { get; set; } 
    public Ray ray { get; set; }
    public  Material mt { get; set; }

    public HitRecord(Point wp, Normal n, Vec2D sp, float T, Ray r, Material material)
    {
        world_point = wp;
        normal = n;
        surface_point = sp;
        t = T;
        ray = r;
        mt = material;
    }

    public static bool are_close(HitRecord a, HitRecord b)
    {
        float eps = 1e-5f;
        float diff = Math.Abs(a.t - b.t);
        return Point.are_close(a.world_point, b.world_point) && Normal.are_close(a.normal, b.normal) &&
               Vec2D.are_close(a.surface_point, b.surface_point) && Ray.are_close(a.ray, b.ray) && diff < eps;

    }
}