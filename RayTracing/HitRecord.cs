using System.Numerics;

namespace RayTracing;

public struct HitRecord
{
    public Point world_point { get; set; }
    public Normal normal { get; set; }
    public Vector2 surface_point { get; set; }
    public float t { get; set; }
    public Ray ray { get; set; }
}