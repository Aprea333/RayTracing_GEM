namespace RayTracing;

public abstract class Shape
{
    public Tran transformation;
    public Shape(Tran tr)
    {
        transformation = tr;
    }

    public abstract HitRecord? ray_intersection(Ray r);
}