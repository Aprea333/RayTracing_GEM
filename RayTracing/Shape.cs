namespace RayTracing;

public abstract class Shape
{
    public Transformation transformation { get; set; }
    public Material material { get; }
    public Shape(Transformation? tran, Material? material)
    {
        transformation = tran ?? new Transformation();
        this.material = material ?? new Material();
    }

    public abstract HitRecord? ray_intersection(Ray r);
    public abstract List<HitRecord>? ray_intersection_list(Ray r);

    public abstract bool is_internal(Point p);

    public static CsgUnion operator +(Shape s1, Shape s2)
    {
        return new CsgUnion(s1, s2);
    }

    public static CsgDifference operator -(Shape s1, Shape s2)
    {
        return new CsgDifference(s1, s2);
    }

    public static CsgIntersection operator *(Shape s1, Shape s2)
    {
        return new CsgIntersection(s1, s2);
    }
}