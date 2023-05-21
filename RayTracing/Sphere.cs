﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace RayTracing;

public class Sphere:Shape
{
    public Sphere(Transformation? tr = null, Material? material = null) : base(tr, material)
    {
    }

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
        return new Vec2D(u, v);
    }
}