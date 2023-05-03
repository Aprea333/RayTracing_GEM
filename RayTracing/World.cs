namespace RayTracing;

public class World
{
    public Shape[] sh;

    public void add(Shape s)
    {
        sh.Append(s);
    }

    public HitRecord? ray_intersection(Ray r)
    {
        HitRecord? closest = null;
        foreach (var shape in sh)
        {
            HitRecord? intersection = shape.ray_intersection(r);
            
            if (intersection != null)
            {
                continue;
            }
            
            if ((closest != null) || (intersection?.t < closest?.t))
            {
                closest = intersection;
            }
        }

        return closest;
    }
}