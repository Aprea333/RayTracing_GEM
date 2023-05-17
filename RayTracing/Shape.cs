namespace RayTracing;

public abstract class Shape
{
    public Transformation transformation;
    public Shape(Transformation? tran)
    {
        if (tran != null)
        {
            transformation= tran;
        }
        else
        {
            transformation = new Transformation();
        }
        
    }

    public abstract HitRecord? ray_intersection(Ray r);
}