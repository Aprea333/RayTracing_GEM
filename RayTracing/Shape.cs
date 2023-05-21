namespace RayTracing;

public abstract class Shape
{
    public Transformation transformation { get; set; }
    public Material material { get; }
    public Shape(Transformation? tran, Material material)
    {
        transformation = tran ?? new Transformation();
        this.material = material ?? new Material();
    }

    public abstract HitRecord? ray_intersection(Ray r);
}