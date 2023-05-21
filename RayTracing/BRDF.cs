namespace RayTracing;

public abstract class Brdf
{
    public Pigment Pig;
    
    public Brdf(Pigment? pig=null)
    {
      Pig = pig ?? new UniformPigment(Colour.white);
    }

    public abstract Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv);

}

public class DiffuseBrdf : Brdf
{
    public DiffuseBrdf(Pigment? pig=null):base(pig) {}

    public override Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv)
    {
        return Pig.get_colour(uv) * (float)(1.0f / Math.PI);
    }
}

