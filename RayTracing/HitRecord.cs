using System.Numerics;

namespace RayTracing;

public struct HitRecord
{
    public Point world_point;
    public Normal normal;
    public Vector2 surface_point;
    public float t;
    public Ray ray;
}