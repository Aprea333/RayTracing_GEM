namespace RayTracing;

public class Cylinder:Shape
{
    
    
    public Cylinder(Transformation? tran = null, Material? material= null) : base(tran, material)
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
        //Solutions for infinitive cylinder
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/(2*a);
        float tmax = (-b+delta_sqrt)/(2*a);
        float hit_t;
        //Find the hit_t
        if (tmin > inv_ray.t_min && tmin < inv_ray.t_max) hit_t = tmin;
        else if (tmax > inv_ray.t_min && tmax < inv_ray.t_max) hit_t= tmax;
        else return null;

        Point hit_point = inv_ray.at(hit_t);
        //Angle phi
        var phi = (float) Math.Atan2(hit_point.y, hit_point.x);
        if (phi < 0) phi += 2.0f * (float)Math.PI;
        var u = phi /2.0f * (float)Math.PI;
        var v = hit_point.z;
        Normal normal = new Normal(hit_point.x, hit_point.y, 0);
        if (normal*inv_ray.direction>0) normal = normal.opposite_normal();
        return new HitRecord(transformation * hit_point, transformation * normal, new Vec2D(u, v), hit_t, r, material);
    }

    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
       
        var inv_ray = Ray.transform(transformation, r);
        float a = inv_ray.direction.x * inv_ray.direction.x + inv_ray.direction.y * inv_ray.direction.y;
        float b = 2 * (inv_ray.direction.x * inv_ray.origin.x + inv_ray.direction.y * inv_ray.origin.y);
        float c = inv_ray.origin.x * inv_ray.origin.x + inv_ray.origin.y * inv_ray.origin.y - 1;
        float delta = b * b - 4 * a * c;
        if (delta <= 0) return null;
        //Solution
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/(2*a);
        float tmax = (-b+delta_sqrt)/(2*a);
        
        var intersection = new List<HitRecord>();
        Point hit_point1 = inv_ray.at(tmin);
        
        var phi1 = (float) Math.Atan2(hit_point1.y, hit_point1.x);
        if (phi1 < 0) phi1 += 2.0f * (float)Math.PI;
        if (tmin > inv_ray.t_min && tmin < inv_ray.t_max && hit_point1.z is >0f and <1f)
        {
            var u = phi1 /2.0f * (float)Math.PI;
            var v = hit_point1.z;
            Normal normal = new Normal(hit_point1.x, hit_point1.y, 0);
            if (normal * inv_ray.direction > 0) normal = normal.opposite_normal();
            intersection.Add(new HitRecord(transformation * hit_point1, transformation * normal,
                new Vec2D(u,v),tmin, r, material));
        }
        
        Point hit_point2 = inv_ray.at(tmax);
        //Angle phi
        var phi2 = (float) Math.Atan2(hit_point2.y, hit_point2.x);
        if (phi2 < 0) phi2 += 2.0f * (float)Math.PI;
        if (tmax > inv_ray.t_min && tmax < inv_ray.t_max && hit_point2.z is >0f and <1f)
        {
            var u = phi2 /2.0f * (float)Math.PI;
            var v = hit_point2.z;
            Normal normal = new Normal(hit_point2.x, hit_point2.y, 0);
            if (normal * inv_ray.direction > 0) normal = normal.opposite_normal();
            intersection.Add(new HitRecord(transformation * hit_point2, transformation * normal,
                new Vec2D(u,v),tmax, r, material));
            
        }

        return intersection.Count == 0 ? null : intersection;
    }

    public override bool is_internal(Point p)
    {
        p = transformation.inverse() * p;
        var dist = p.x * p.x + p.y * p.y;
        return dist < 1f && p.z > 0f && p.z < 1f;
    }
    
}