using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace RayTracing;

public class Sphere:Shape
{
    public Sphere(Transformation? tr = null, Material? material = null) : base(tr, material)
    {
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: RayTracing.Transformation; size: 2846MB")]
    public override HitRecord? ray_intersection(Ray r)
    {
        Ray inv_ray = Ray.transform(transformation.inverse(), r);
        Vec O = inv_ray.origin.convert_to_vec();
        float b = O * inv_ray.direction;
        float a = inv_ray.direction.squared_norm();
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
        if (tmin > inv_ray.t_min && tmin < inv_ray.t_max)
        {
            hit_t = tmin;
        }else if (tmax > inv_ray.t_min && tmax < inv_ray.t_max)
        {
            hit_t= tmax;
        }
        else return null;

        Point hit_point = inv_ray.at(hit_t);
        return new HitRecord(transformation * hit_point, transformation * sphere_normal(hit_point, r.direction),
            sphere_point_to_uv(hit_point),hit_t, r, material);
    }

    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        Ray inv_ray = Ray.transform(transformation.inverse(), r);
        Vec O = inv_ray.origin.convert_to_vec();
        float b = O * inv_ray.direction;
        float a = inv_ray.direction.squared_norm();
        float c = O.squared_norm()-1;
        float delta = b * b - a * c;
        if (delta <= 0)
        {
            return null;
        }
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/a;
        float tmax = (-b+delta_sqrt)/a;
        var intersection = new List<HitRecord>();
        var hit_point1 = inv_ray.at(tmin);
        var hit_point2 = inv_ray.at(tmax);
        
        if (tmin > inv_ray.t_min && tmin < inv_ray.t_max)
        {
            intersection.Add(new HitRecord(transformation * hit_point1, transformation * sphere_normal(hit_point1, r.direction),
                sphere_point_to_uv(hit_point1),tmin, r, material));
        }
        if (tmax > inv_ray.t_min && tmax < inv_ray.t_max)
        {
            intersection.Add(new HitRecord(transformation * hit_point2, transformation * sphere_normal(hit_point2, r.direction),
                sphere_point_to_uv(hit_point2),tmax, r, material));
        }

        return intersection.Count == 0 ? null : intersection;
    }

    public override bool is_internal(Point p)
    {
        p = transformation.inverse() * p;
        return p.convert_to_vec().squared_norm() < 1f;
    }

    public Normal sphere_normal(Point p, Vec dir)
    {
        Normal result = new Normal(p.x, p.y, p.z);
        if (p.convert_to_vec() * dir > 0) result = result.opposite_normal();
        return result;
    }

    public Vec2D sphere_point_to_uv(Point p)
    {
        float u = (float)(Single.Atan2(p.y, p.x)/(2*Math.PI));
        float v = (float)(Single.Acos(p.z)/Math.PI);
        return new Vec2D(u >= 0f ? u : u + 1f, v);
    }
}