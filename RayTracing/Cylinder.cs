namespace RayTracing;

public class Cylinder:Shape
{
    
    
    public Cylinder(Transformation? tran, Material? material) : base(tran, material)
    {
    }

    public override HitRecord? ray_intersection(Ray r)
    {
        //Cilinder with equation x^2 + y^2 = 1
        var inv_ray = Ray.transform(transformation, r);
        float a = inv_ray.direction.x * inv_ray.direction.x + inv_ray.direction.y * inv_ray.direction.y;
        float b = 2 * (inv_ray.direction.x * inv_ray.origin.x + inv_ray.direction.y * inv_ray.origin.y);
        float c = inv_ray.origin.x * inv_ray.origin.x + inv_ray.origin.y * inv_ray.origin.y - 1;
        float delta = b * b - 4 * a * c;
        if (delta <= 0) return null;
        //Solution for infinitive cylinder
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/(2*a);
        float tmax = (-b+delta_sqrt)/(2*a);
        float hit_t;
        if (tmin > inv_ray.t_min && tmin < inv_ray.t_max) hit_t = tmin;
        else if (tmax > inv_ray.t_min && tmax < inv_ray.t_max) hit_t= tmax;
        
        
        
        else return null;
        return null;
    }

    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
       
        
        throw new NotImplementedException();
    }

    public override bool is_internal(Point p)
    {
        throw new NotImplementedException();
    }
}