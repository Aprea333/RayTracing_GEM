namespace RayTracing;

public class World
{
    public List<Shape> shapes;

    public World()
    {
        shapes = new List<Shape>();
    }

    public void add(Shape s)
    {
        shapes.Add(s);
    }

    public HitRecord? ray_intersection(Ray r)
    {
        HitRecord? closest = null;
        foreach (var shape in shapes)
        {
            HitRecord? intersection = shape.ray_intersection(r);
            
            if (intersection == null)
            {
                continue;
            }
            
            if ((closest == null) || (intersection?.t < closest?.t))
            {
                closest = intersection;
            }
        }

        return closest;
    }
}