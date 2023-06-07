namespace RayTracing;

public class Cylinder:Shape
{
    
    
    public Cylinder(Transformation? tran = null, Material? material= null) : base(tran, material)
    {
    }

    public Cylinder(Point center, float radius, float height, Vec direction, Material? material = null) : base(null,
        material)
    {
        //Define the transformations
        Transformation scaling = Transformation.scaling(radius, radius, height);
        Transformation translation = Transformation.translation(center.convert_to_vec());
        
        //Angles
        direction = direction.normalize();
        float cos = direction * new Vec(0, 0, 1);
        float theta_x = MathF.Atan2(-direction.y, direction.z);
        float theta_y = MathF.Atan2(direction.x, MathF.Sqrt(direction.y * direction.y + direction.z * direction.z));
        float theta_z = MathF.Atan2(-(direction.x * direction.y) / (1 + direction.z),
            1 - (direction.x * direction.x) / (1 + direction.z));
        Transformation rotation = Transformation.rotation_z(theta_z) * Transformation.rotation_y(theta_y) *
                                  Transformation.rotation_x(theta_x);

        transformation = translation * rotation * scaling;
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
        float b = 2 * (inv_ray.direction.x * inv_ray.origin.x + inv_ray.direction.y * inv_ray.origin.y);
        float c = inv_ray.origin.x * inv_ray.origin.x + inv_ray.origin.y * inv_ray.origin.y - 1;
        float delta = b * b - 4 * a * c;
        if (delta <= 0) return new List<HitRecord>();
        //Solution
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/(2f*a);
        float tmax = (-b+delta_sqrt)/(2f*a);
        
        var intersection = new List<HitRecord>();

        if (tmin > 1e-5)
        {
            Point hit_point = inv_ray.at(tmin);
            if (hit_point.z > -0.5f && hit_point.z < 0.5f)
                intersection.Add(new HitRecord(transformation*hit_point, transformation*cylinder_normal(hit_point, r.direction), cylinder_to_uv(hit_point), tmin, r, material));
        }
       
        if (tmax > 1e-5)
        {
            Point hit_point = inv_ray.at(tmax);
            if (hit_point.z > -0.5f && hit_point.z < 0.5f)
                intersection.Add(new HitRecord(transformation*hit_point, transformation*cylinder_normal(hit_point, r.direction), cylinder_to_uv(hit_point), tmax, r, material));
        }
        
        //Intersection with bottom face
        Plane bottom = new Plane(Transformation.translation(new Vec(0, 0, -0.5f)));
        HitRecord? hit_bottom = bottom.ray_intersection(r);
        if (hit_bottom.HasValue)
        {
            Point intern = hit_bottom.Value.world_point;
            if (intern.x * intern.x + intern.y * intern.y < 1)
            {
                intersection.Add(new HitRecord(transformation*intern, transformation*cylinder_normal(intern, r.direction), cylinder_to_uv(intern), hit_bottom.Value.t, r, material));
            }
        }
        
        //Intersection with top face
        Plane top = new Plane(Transformation.translation(new Vec(0, 0, 0.5f)));
        //Console.WriteLine("test1");
        HitRecord? hit_top = top.ray_intersection(r);
        if (intersect(r, top))
        {
            Console.WriteLine("Check1");
        }else Console.WriteLine("Check2");
        //Console.WriteLine("test2");
        if (hit_top.HasValue)
        {
            //Console.WriteLine("test3");
            Point intern = hit_top.Value.world_point;
            if (intern.x * intern.x + intern.y * intern.y < 1)
            {
                intersection.Add(new HitRecord(transformation*intern, transformation*cylinder_normal(intern, r.direction), cylinder_to_uv(intern), hit_top.Value.t, r, material));
            }
        }

        return intersection.Count == 0 ? null: intersection.OrderBy(hit => hit.t).ToList();
    }

    private Normal cylinder_normal(Point p, Vec dir)
    {
        Point inv = transformation.inverse() * p;
        Vec inv_dir = transformation.inverse() * dir;
        Normal result = new Normal(inv.x, inv.y, 0);
        if (Functions.are_close(inv.z, 0.5f)) result = new Normal(0, 0, 1);
        if (Functions.are_close(inv.z, -0.5f)) result = new Normal(0, 0, -1);
        if (inv.x * inv_dir.x + inv.y * inv_dir.y > 0f) result = result.opposite_normal();
        
        return result;
    }

    private Vec2D cylinder_to_uv(Point p)
    {
        float u, v;
        if (Functions.are_close(p.z, 0.5f))
        {
            u = 0.5f + (p.x + 1f) / 4f;
            v = (p.y + 1f) / 4f;
        }else if (Functions.are_close(p.z, -0.5f))
        {
            u = (p.x + 1f) / 4f;
            v = (p.y + 1f) / 4f;
        }
        else
        {
            u =(float)((((float)Math.Atan2(p.y,p.x)+(2f*Math.PI))%(2f*Math.PI))/(2 * Math.PI));
            v = 1f - (p.z + 0.5f) / 2f;
        }

        return new Vec2D(u, v);
    }
    public override bool is_internal(Point point)
    {
        Point p = transformation.inverse() * point;
        var dist = p.x * p.x + p.y * p.y;
        return dist < 1f && p.z is >= -0.5f and <= 0.5f;
    }

    public bool intersect(Ray r, Plane p)
    {
        HitRecord? hit = p.ray_intersection(r);
        if (hit != null) return true;
        return false;
    }
}