namespace RayTracing;

public class Plane:Shape
{
    public Plane(Transformation? tran=null, Material? material=null) : base(tran, material)
    {
    }

    public override HitRecord? ray_intersection(Ray r)
    {
        var invRay = Ray.transform(transformation.inverse(), r);
        if (Math.Abs(invRay.direction.z) < 1e-5f) return null;
        var t = -invRay.origin.z / invRay.direction.z;
        //Check if t is smaller than the minimum or larger than the maximum
        if (t < invRay.t_min || t >= invRay.t_max) return null;

        var hitPoint = invRay.at(t);

        float dZ;
        if (invRay.direction.z < 0.0f) dZ = 1.0f;
        else dZ = -1.0f;
        float u = hitPoint.x - (float)Math.Floor(hitPoint.x);
        float v = hitPoint.y - (float)Math.Floor(hitPoint.y);
        Vec2D vec = new Vec2D(u,v);

        return new HitRecord(transformation * hitPoint, transformation * new Normal(0f, 0f, dZ), vec, t, r, material);
    }

    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        var hit = ray_intersection(r);
        if (hit == null) return null;
        var intersection = new List<HitRecord> { (HitRecord)hit };
        return intersection;
    }

    public override bool is_internal(Point p)
    {
        p = transformation.inverse() * p;
        return Math.Abs(p.z) < 1e-05;
    }
}