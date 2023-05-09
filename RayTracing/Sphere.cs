using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace RayTracing;

public class Sphere:Shape
{
    public Sphere(Tran? tr = null) : base(tr)
    {
    }

    public override HitRecord? ray_intersection(Ray r)
    {
        Ray inv_ray = Ray.Transform(tr.inverse(), r);
        Vec O = inv_ray.Origin.Convert();
        float b = O * inv_ray.Dir;
        float a = inv_ray.Dir.squared_norm();
        float c = O.squared_norm()-1;
        float delta = b * b - a * c;
        if (delta <= 0)
        {
            return null;
        }
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/a;
        float tmax = (-b+delta_sqrt)/a;
        float hit_t;
        if (tmin > inv_ray.Tmin && tmin < inv_ray.Tmax)
        {
            hit_t = tmin;
        }else if (tmax > inv_ray.Tmin && tmax < inv_ray.Tmax)
        {
            hit_t= tmax;
        }
        else return null;

        Point hit_point = inv_ray.At(hit_t);
        return new HitRecord(tr * hit_point, tr * sphere_normal(hit_point, r.Dir),
            sphere_point_to_uv(hit_point),hit_t, r);
    }

    public Normal sphere_normal(Point p, Vec dir)
    {
        Normal result = new Normal(p.X, p.Y, p.Z);
        if (p.Convert() * dir > 0) result = result.neg_normal();
        return result;
    }

    public Vec2D sphere_point_to_uv(Point p)
    {
        float u = (float)(Single.Atan2(p.Y, p.X)/(2*Math.PI));
        float v = (float)(Single.Acos(p.Z)/Math.PI);
        return new Vec2D(u, v);
    }
}