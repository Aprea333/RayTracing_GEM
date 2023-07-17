namespace RayTracing;

public abstract class Shape
{
    public Transformation transformation { get; set; }
    protected Material material { get; }

    protected Shape(Transformation? tran, Material? material)
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

public static class Useful_Shape
{
       
        
    public static Shape union_shapes(Transformation? transf = null)
    {
        var yellow = new Material(new DiffuseBrdf(new UniformPigment(new Colour(1, 1, 0))));
        var red = new Material(new DiffuseBrdf(new UniformPigment(new Colour(1, 0, 0))));
        var blue =new Material(new DiffuseBrdf(new UniformPigment(new Colour(0, 0, 1))));
        Shape C1 = new Cylinder(new Point(0, 0, 0), 0.7f, 3f, new Vec(1, 0, 0), yellow);
        Shape C2 = new Cylinder(new Point(0, 0, 0), 0.7f, 3f, new Vec(0, 1, 0), yellow);
        Shape C3 = new Cylinder(new Point(0, 0, 0), 0.7f, 3f, new Vec(0, 0, 1), yellow);
        Shape S = new Sphere(Transformation.scaling(1.35f, 1.35f, 1.35f), blue);
        Shape B = new Box(material:red);
        Shape cyl_tot = (C1 + C2) + C3;
        Shape ext = S * B;
        Shape tot = ext-cyl_tot;
        if (transf != null) tot.transformation = (Transformation)transf;
        return tot;
    }
}