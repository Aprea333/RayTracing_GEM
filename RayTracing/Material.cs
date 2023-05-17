namespace RayTracing;

public class Material
{
    public Brdf brdf = new DiiffuseBrdf();
    public Pigment emitted_radiance = new UniformPigment(BLACK);
}   