namespace RayTracing;

public class Cylinder:Shape
{
    public Cylinder(Transformation? tran = null, Material? material= null) : base(tran, material)
    {
    }
    
    //CONSTRUCTOR ONLY FOR UNION OF THREE CYLINDER
    public Cylinder(Point center, float radius, float height, Vec direction, Material? material = null) : base(null,
        material)
    {
        Transformation scale = Transformation.scaling(radius, radius, height);
        Transformation translation = Transformation.translation(center.convert_to_vec());
        Transformation? rotation = null;
        //CREATE A CYLINDER ON THE THREE AXIS
        if (Vec.are_close(direction,new Vec(1,0,0))) rotation = Transformation.rotation_y(90);
        else if (Vec.are_close(direction, new Vec(0, 1, 0))) rotation = Transformation.rotation_x(90);
        else if (Vec.are_close(direction, new Vec(0, 0, 1))) rotation = new Transformation();
        else
        {
            throw new GrammarError("Direction must be normalized");
        }

        Transformation? tran = (translation * rotation);
        transformation = (Transformation)(tran * scale);
    }
    public override HitRecord? ray_intersection(Ray r)
    {
        List<HitRecord>? intersection = ray_intersection_list(r);
        if (intersection is { Count: 0 }) return null;
        return intersection?[0];
    }
    

    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        var inv_ray = Ray.transform(transformation.inverse(), r);
        float a = inv_ray.direction.x * inv_ray.direction.x + inv_ray.direction.y * inv_ray.direction.y;
        float b = 2f * (inv_ray.direction.x * inv_ray.origin.x + inv_ray.direction.y * inv_ray.origin.y);
        float c = inv_ray.origin.x * inv_ray.origin.x + inv_ray.origin.y * inv_ray.origin.y - 1f;
        float delta = b * b - 4f * a * c;
        //Solution
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/(2f*a);
        float tmax = (-b+delta_sqrt)/(2f*a);
        
        var intersection = new List<HitRecord>();

        if (tmin > 1e-5)
        {
            Point hit_point = inv_ray.at(tmin);
            if (hit_point.z > -0.5f && hit_point.z < 0.5f)
            {
                intersection.Add(new HitRecord(transformation*hit_point, transformation*cylider_normal(hit_point, r.direction), cyl_toUV(hit_point), tmin, r, material));
            }
        }
        if (tmax > 1e-5)
        {
            Point hit_point = inv_ray.at(tmax);
            if (hit_point.z > -0.5f && hit_point.z < 0.5f)
            {
                intersection.Add(new HitRecord(transformation*hit_point, transformation*cylider_normal(hit_point, r.direction), cyl_toUV(hit_point), tmax, r, material));
            }
        }
        
        //BOTTOM FACE
        Plane bottom = new Plane(Transformation.translation(new Vec(0, 0, -0.5f)));
        HitRecord? hit_bottom = bottom.ray_intersection(inv_ray);
        if (hit_bottom.HasValue)
        {
            Point intern = hit_bottom.Value.world_point;
            if (intern.x * intern.x + intern.y * intern.y < 1)
            {
                intersection.Add(new HitRecord(transformation*intern, transformation*cylider_normal(intern, r.direction), cyl_toUV(intern), hit_bottom.Value.t, r, material));
            }
        }
        
        //UP FACE
        Plane up = new Plane(Transformation.translation(new Vec(0, 0, 0.5f)));
        HitRecord? hit_up = up.ray_intersection(inv_ray);
        if (hit_up.HasValue)
        {
            Point intern = hit_up.Value.world_point;
            if (intern.x * intern.x + intern.y * intern.y < 1)
            {
                intersection.Add(new HitRecord(transformation*intern, transformation*cylider_normal(intern, r.direction), cyl_toUV(intern), hit_up.Value.t, r, material));

            }
        }

        return intersection.Count == 0 ? null : intersection.OrderBy(hit => hit.t).ToList();;
    }
    
    public override bool is_internal(Point point)
    {
        Point p = transformation.inverse() * point;
        var dist = p.x * p.x + p.y * p.y;
        return dist < 1f && p.z is >= -0.5f and <= 0.5f;
    }

    private Normal cylider_normal(Point point, Vec dir)
    {
        Point inv_point = transformation.inverse() * point;
        Vec inv_dir = transformation.inverse() * dir;
        Normal result = new Normal(inv_point.x, inv_point.y, 0);
        result = result.normalization();
        if (Functions.are_close(inv_point.z, 0.5f)) result = new Normal(0, 0, 1);
        if (Functions.are_close(inv_point.z, -0.5f)) result = new Normal(0, 0, -1);
        if (inv_point.x * inv_point.x + inv_point.y * inv_point.y > 0) result = result.opposite_normal();

        return result;
    }

    private Vec2D cyl_toUV(Point point)
    {
        float u, v;
        if (Functions.are_close(point.z, 0.5f))
        {
            u = 0.5f + (point.x + 1f) / 4f;
            v = (point.y + 1) / 4f;
        }else if (Functions.are_close(point.z, -0.5f))
        {
            u = (point.x + 1f) / 4f;
            v = (point.y + 1f) / 4f;
        }
        else
        {
            u = (float)((((float)Math.Atan2(point.y, point.x) + (2f * Math.PI)) % (2f * Math.PI)) / (2f * Math.PI));
            v = 1f - (point.z + 0.5f) / 2f;
        }

        return new Vec2D(u, v);
    }

}