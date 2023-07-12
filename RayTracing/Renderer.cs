namespace RayTracing;

public abstract class Renderer
{
    public World wd;
    public Colour background;

    public Renderer(World? world = null, Colour? background_color = null)
    {
        wd = world ?? new World();
        background = background_color ?? Colour.black;
    }

    public abstract Colour tracing(Ray ray);
}

public class OnOffRenderer : Renderer
{
    public Colour objects;

    public OnOffRenderer(World? world = null, Colour? background_color = null, Colour? objects_color = null) : base(world,
        background_color)
    {
        wd = world ?? new World();
        background = background_color ?? Colour.black;
        objects = objects_color ?? Colour.white;
    }

    public override Colour tracing(Ray ray)
    {
        return wd.ray_intersection(ray) != null ? objects : background;
    }
}

public class FlatRenderer : Renderer
{
    public FlatRenderer(World? world = null, Colour? background_color = null):base(world,background_color){}

    public override Colour tracing(Ray ray)
    {
        var hit = wd.ray_intersection(ray);
        if (hit == null) return background;
        var material = hit.Value.mt;
        Colour color1 = material.brdf.Pig.get_colour(hit.Value.surface_point);
        Colour color2 = material.emitted_radiance.get_colour(hit.Value.surface_point);
        return (color1 + color2);
    }
}