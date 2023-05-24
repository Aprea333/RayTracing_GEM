namespace RayTracing;

public abstract class Brdf
{
    public Pigment Pig;
    
    public Brdf(Pigment? pig=null)
    {
      Pig = pig ?? new UniformPigment(Colour.white);
    }

    public abstract Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv);

    public abstract Ray ScatterRay(PCG pcg, Vec incoming_dir, Point interaction_point, Normal normal, int depth);

}

public class DiffuseBrdf : Brdf
{
    public DiffuseBrdf(Pigment? pig=null):base(pig) {}

    public override Colour Eval(Normal normal, Vec inDir, Vec outDir, Vec2D uv)
    {
        return Pig.get_colour(uv) * (float)(1.0f / Math.PI);
    }

    public override Ray ScatterRay(PCG pcg, Vec incoming_dir, Point interaction_point, Normal normal, int depth)
    {
        (Vec e1, Vec e2, Vec e3) = create_onb_from_z(normal); // L'ho fatto immaginando che "create_onb_from_z" ritorni una tupla
        float cos_theta_sq = pcg.random_float();
        float cos_theta = (float)Math.Sqrt(cos_theta_sq);
        float sin_theta = (float)Math.Sqrt(1.0 - cos_theta_sq);
        float phi = (float)(2.0 * Math.PI * pcg.random_float());
        return new Ray(interaction_point,
            e1 * (float)Math.Cos(phi) * cos_theta + e2 *(float) Math.Sin(phi) * cos_theta + e3 * sin_theta,
            1.0e-3f,
            Single.PositiveInfinity,
            depth);
    }
}

