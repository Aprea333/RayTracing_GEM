namespace RayTracing;

public class Material
{
    public Brdf brdf;
    public Pigment emitted_radiance;
    
    public Material(Brdf? brdf = null, Pigment? emitted_radiance = null)
    {
        this.brdf = brdf ?? new DiffuseBrdf();
        this.emitted_radiance = emitted_radiance ?? new UniformPigment(Colour.black);
    }
}   