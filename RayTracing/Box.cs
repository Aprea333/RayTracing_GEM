namespace RayTracing;

public class Box:Shape
{
    
    //Bound has two component: minimum and maximum of the box
    public Point[] Bound = new Point[2];
    
    public Box(Point? max = null, Point? min = null, Transformation? tran = null, Material? material = null) : base(tran, material)
    {
        Bound[0] = min ?? new Point(-1f, -1f, -1f);
        Bound[1] = max ?? new Point(1f, 1f, 1f);

    }

    public override HitRecord? ray_intersection(Ray r)
    {
        List<HitRecord> intersection = ray_intersection_list(r);
        return intersection.Count == 0 ? null : intersection[0];
    }

    /// <summary>
    ///
    ///Usin https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-box-intersection.html
    /// </summary>
    /// <param name="r"></param>
    /// <returns></returns>
    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        Ray inv_ray = Ray.transform(transformation.inverse(),r);
        float t0 = 0;
        float t1 = r.t_max;
        //Find the nearest intersection. Convert vectors and points in list for the next cycle 
        List<float> inv_ray_dir = new List<float> { inv_ray.direction.x, inv_ray.direction.y, inv_ray.direction.z };
        List<float> inv_ray_origin = new List<float> { inv_ray.origin.x, inv_ray.origin.y, inv_ray.origin.z };
        List<float> min =  new List<float> { Bound[0].x, Bound[0].y, Bound[0].z };
        List<float> max =  new List<float> { Bound[0].x, Bound[0].y, Bound[0].z };

        for (int i = 0; i < 3; i++)
        {
            float inv_dir = 1 / inv_ray_dir[i];
            float t_near = (min[i] - inv_ray_origin[i]) * inv_dir;
            float t_far = (max[i] - inv_ray_origin[i]) * inv_dir;
            //check if near is the first intersection, otherwise switch near and far
            if (t_near > t_far)
            {
                (t_near, t_far) = (t_far, t_near);
            }

            t0 = t_near > t0 ? t_near : t0;
            t1 = t_far < t1 ? t_far : t1;
            //If there are zero intersections, return a null list
            if (t0 > t1) return new List<HitRecord>();
        }

        List<HitRecord>? hits = new List<HitRecord>();
        
        //If t0 and t1 are > 0 add them to the hits list
        if (t0 > 0)
        {
            var hit_point0 = inv_ray.at(t0);
            hits.Add(new HitRecord(transformation*hit_point0, transformation*box_normal(hit_point0, r.direction), box_to_uv(hit_point0), t0, r, material ));
        }
        if (t1 > 0)
        {
            var hit_point1 = inv_ray.at(t1);
            hits.Add(new HitRecord(transformation*hit_point1, transformation*box_normal(hit_point1, r.direction), box_to_uv(hit_point1), t0, r, material ));
        }

        return hits;

    }

    public override bool is_internal(Point p)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Function that computes the normal to a box with the vector direction of the ray 
    /// </summary>
    /// <param name="p">The point on the surface</param>
    /// <param name="ray_dir">The direction of the ray</param>
    /// <returns></returns>
    public Normal box_normal(Point p, Vec ray_dir)
    {
        Normal result;
        if (Functions.are_close(p.x, Bound[0].x)) result = new Normal(-1f, 0f, 0f);
        else if (Functions.are_close(p.x, Bound[1].x)) result = new Normal(1f, 0f, 0f);
        else if (Functions.are_close(p.y, Bound[0].y)) result = new Normal(0f, -1f, 0f);
        else if (Functions.are_close(p.y, Bound[1].y)) result = new Normal(0f, 1f, 0f);
        else if (Functions.are_close(p.z, Bound[0].z)) result = new Normal(0f, -1f, 0f);
        else result = new Normal(0f, 1f, 0f);
        //normal is in the opposite direction to ray_dir
        if (result * ray_dir > 0) result.opposite_normal();
        return result;
    }

    /// <summary>
    /// Function that computes the uv coordinates of the 3d box
    ///Using info in http://ilkinulas.github.io/development/unity/2016/05/06/uv-mapping.html
    /// </summary>
    /// <param name="p">point</param>
    /// <returns></returns>
    public Vec2D box_to_uv(Point p)
    {
        float var1 = 0f;
        float var2 = 0f;
        Point min = Bound[0];
        Point max = Bound[0];
        int f = 0; //face of the box
        if (Functions.are_close(p.x, min.x))
        {
            f = 1;
            var1 = 1 - (p.z - min.z) / (max.z - min.z);
            var2 =(p.y - min.y) / (max.y - min.y);
        }else if (Functions.are_close(p.y, max.y))
        {
            f = 2;
            var1 = (p.x - min.x) / (max.x - min.x);
            var2 =(p.z - min.z) / (max.z - min.z);
        }else if (Functions.are_close(p.z, min.z))
        {
            f = 3;
            var1 = (p.x - min.x) / (max.x - min.x);
            var2 =(p.y - min.y) / (max.y - min.y);
        }else if (Functions.are_close(p.y, min.y))
        {
            f = 4;
            var1 = (p.x - min.x) / (max.x - min.x);
            var2 =1- (p.z - min.z) / (max.z - min.z);
        }else if (Functions.are_close(p.x, max.x))
        {
            f = 5;
            var1 = (p.z - min.z) / (max.z - min.z);
            var2 = (p.y - min.y) / (max.y - min.y);
        }
        else 
        {
            f = 6;
            var1 = 1-(p.x - min.x) / (max.x - min.x);
            var2 =(p.y - min.y) / (max.y - min.y);
        }

        float u = 0;
        float v = 0;
        //Using the site in the summery
        if (f == 1)
        {
            u = (0f + var1) / 4;
            v = (1f + var2) / 3;
        }else if (f == 2)
        {
            u = (1f + var1) / 4;
            v = (2f + var2) / 3;
        }else if (f == 3)
        {
            u = (1f + var1) / 4;
            v = (1f + var2) / 3;
        }else if (f == 4)
        {
            u = (1f + var1) / 4;
            v = (0f + var2) / 3;
        }else if (f == 5)
        {
            u = (2f + var1) / 4;
            v = (1f + var2) / 3;
        }else if (f == 6)
        {
            u = (3f + var1) / 4;
            v = (1f + var2) / 3;
        }

        return new Vec2D(u,v);
    }
}

public class Functions
{
    public static bool are_close(float a, float b)
    {
        if (Math.Abs(a - b) < 1e-5) return true;
        return false;
    }
}