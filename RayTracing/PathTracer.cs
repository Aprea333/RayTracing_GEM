using Color = System.Drawing.Color;

namespace RayTracing;

public class PathTracer
{
    public World Wld;
    public Colour BackgroundColor;
    public PCG RandGen;
    public int NRays;
    public int MaxDepth;
    public int MinDepth;

    public PathTracer(World Wld, Colour BackgroundColor, PCG RandGen, int NRays, int MaxDepth, int MinDepth)
    {
        this.Wld = Wld;
        this.BackgroundColor = BackgroundColor;
        this.RandGen = RandGen;
        this.NRays = NRays;
        this.MaxDepth = MaxDepth;
        this.MinDepth = MinDepth;
    }

    public Colour PathTracing(Ray ray)
    {
        if (ray.depth > MaxDepth)
            return new Colour();

        HitRecord? hit_record = Wld.ray_intersection(ray);
        if (hit_record == null)
            return BackgroundColor;
        
        Material hit_material = hit_record.Value.mt;
        Colour hit_color = hit_material.brdf.Pig.get_colour(hit_record.Value.surface_point);
        Colour emitted_radiance = hit_material.emitted_radiance.get_colour(hit_record.Value.surface_point);

        float hit_color_lum = Math.Max(Math.Max(hit_color.r_c, hit_color.b_c), hit_color.g_c);

        if (ray.depth >= MinDepth)
        {
            float q = Math.Max(0.05f, 1 - hit_color_lum);
            if (RandGen.random_float() > q)
                hit_color *= 1.0f / (1.0f - q);
            else
                return emitted_radiance;
        }

        Colour cum_radiance = new Colour();

        if (hit_color_lum > 0)
        {
            for (int ray_index = 0; ray_index < NRays; ray_index++)
            {
                Ray new_ray = hit_material.brdf.Scatter_Ray(RandGen, hit_record.Value.ray.direction,
                    hit_record.Value.world_point,
                    hit_record.Value.normal,
                    ray.depth + 1);
                Colour new_radiance = PathTracing(new_ray);
                cum_radiance += hit_color * new_radiance;
            }
        }

        return emitted_radiance + cum_radiance * (1.0f / NRays);

    }

}