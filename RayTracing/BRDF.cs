using System.Xml.Schema;

namespace RayTracing;

public abstract class Brdf
{
    public Pigment Pig;
    
    public Brdf(Pigment? pig=null)
    {
      Pig = pig ?? new UniformPigment(Colour.white);
    }

    public abstract Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv);
    public abstract Ray Scatter_Ray(PCG pcg, Vec incoming_dir, Point interaction_point, Normal normal, int depth);
}

public class DiffuseBrdf : Brdf
{
    public DiffuseBrdf(Pigment? pig=null):base(pig) {}

    public override Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv)
    {
        return Pig.get_colour(uv) * (float)(1.0f / Math.PI);
    }
    
}

public class SpecularBrdf : Brdf
{
    public SpecularBrdf(Pigment? pig=null) : base(pig){}

    public override Ray Scatter_ray(PCG pcg, Vec incoming_dir, Point interaction_point, Normal normal, int depth)
    {
        var Ray_dir = new Vec(incoming_dir.x, incoming_dir.y, incoming_dir.z).normalize();
        var norm = normal.To_vec().normalize();

        return new Ray(interaction_point,Ray_dir-2*norm*(norm*Ray_dir),(float)1e-5,float.PositiveInfinity,depth);
    }

}