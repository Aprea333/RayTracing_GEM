namespace RayTracing;

public abstract class Brdf
{
    public Pigment Pig;
    
    public Brdf(Pigment? pig=null)
    {
      Pig = pig;
    }

    public abstract Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv);

}

public class DiiffuseBrdf : Brdf
{
    public DiiffuseBrdf(Pigment? pig=null):base(pig) {}

    public override Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv)
    {
        Colour C = new Colour();
        return  C = Pigment.get_colour(uv) * 1f / Math.PI;
    }
}

