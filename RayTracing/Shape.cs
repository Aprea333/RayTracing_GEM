namespace RayTracing;

public abstract class Shape
{
    public Tran tr;
    public Shape(Tran? Tr)
    {
        if (Tr != null)
        {
            tr= Tr;
        }
        else
        {
            tr = new Tran();
        }
        
    }

    public abstract HitRecord? ray_intersection(Ray r);
}