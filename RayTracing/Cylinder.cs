namespace RayTracing;

public class Cylinder:Shape
{
    public Cylinder(Transformation? tran = null, Material? material= null) : base(tran, material)
    {
    }

    public override HitRecord? ray_intersection(Ray r)
    {
        List<HitRecord>? intersection = ray_intersection_list(r);
        if (intersection is { Count: 0 }) return null;
        return intersection?[0];
    }
    

    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        var inv_ray = Ray.transform(transformation, r);
        float a = inv_ray.direction.x * inv_ray.direction.x + inv_ray.direction.y * inv_ray.direction.y;
        float b = 2f * (inv_ray.direction.x * inv_ray.origin.x + inv_ray.direction.y * inv_ray.origin.y);
        float c = inv_ray.origin.x * inv_ray.origin.x + inv_ray.origin.y * inv_ray.origin.y - 1f;
        float delta = b * b - 4f * a * c;
        if (delta <= 0) return null;
        //Solution
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/(2f*a);
        float tmax = (-b+delta_sqrt)/(2f*a);
        
        var intersection = new List<HitRecord>();
        
        //Minimum
        Point hit_point1 = inv_ray.at(tmin);
        //Angle phi, if negative add 2pi to make it positive
        float phi1 = (float)Math.Atan2(hit_point1.y, hit_point1.x);
        if (phi1 < 0) phi1 += 2f * (float)Math.PI;
        if (tmin < inv_ray.t_max && tmin > inv_ray.t_min && hit_point1.z is > 0f and < 1f)
        {
            float u = phi1 / (2f * (float)Math.PI);
            float v = hit_point1.z;
            Normal normal = new Normal(hit_point1.x, hit_point1.y, 0f);
            if (normal * inv_ray.direction > 0f) normal = normal.opposite_normal();
            intersection.Add(new HitRecord(transformation*hit_point1, transformation*normal, new Vec2D(u,v), tmin, r, material));
        }
        //Max
        Point hit_point2 = inv_ray.at(tmax);
        //Angle phi, if negative add 2pi to make it positive
        float phi2 = (float)Math.Atan2(hit_point2.y, hit_point2.x);

        if (phi2 < 0) phi2 += 2f * (float)Math.PI;
        if (tmax < inv_ray.t_max && tmax > inv_ray.t_min && hit_point2.z is > 0f and < 1f)
        {
            float u = phi2 / (2f * (float)Math.PI);
            float v = hit_point2.z;
            Normal normal = new Normal(hit_point2.x, hit_point2.y, 0f);
            if (normal * inv_ray.direction > 0f) normal = normal.opposite_normal();
            intersection.Add(new HitRecord(transformation*hit_point2, transformation*normal, new Vec2D(u,v), tmax, r, material));
        }
        
        //intersection with bottom and up
        Plane bottom = new Plane(Transformation.translation(new Vec(0, 0, 0)));
        HitRecord? hit_bottom = bottom.ray_intersection(r);
        if (hit_bottom.HasValue)
        {
            Point intern = hit_bottom.Value.world_point;
            if (intern.x * intern.x + intern.y * intern.y < 1)
            {
                intersection.Add(new HitRecord(transformation*intern, hit_bottom.Value.normal, hit_bottom.Value.surface_point, hit_bottom.Value.t, r, material));
            }
        }
        
        Plane up = new Plane(Transformation.translation(new Vec(0, 0, 1f)));
        HitRecord? hit_up = up.ray_intersection(r);
        if (hit_up.HasValue)
        {
            Point intern = hit_up.Value.world_point;
            if (intern.x * intern.x + intern.y * intern.y < 1)
            {
                intersection.Add(new HitRecord(transformation*intern, hit_up.Value.normal, hit_up.Value.surface_point, hit_up.Value.t, r, material));
            }
        }

        return intersection.Count == 0 ? null : intersection.OrderBy(hit => hit.t).ToList();;
    }
    
    public override bool is_internal(Point point)
    {
        Point p = transformation.inverse() * point;
        var dist = p.x * p.x + p.y * p.y;
        return dist < 1f && p.z is >= 0f and <= 1f;
    }

}