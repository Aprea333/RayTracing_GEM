namespace RayTracing;

public interface camera
{
    Ray fire_ray(float u, float v);
}

/// <summary>
/// Class for the othogonal projection
/// </summary>
public class Orthogonal_Camera : camera
{
    public float aspect_ratio;
    public Tran transformation;
    
    public Orthogonal_Camera( Tran tran = null, float aspect_ratio = 1.0f)
    {
        this.aspect_ratio = aspect_ratio;
        if (tran == null)
        {
            this.transformation = new Tran();
        }
        else
        {
            transformation = tran;
        }
    }
   
    /// <summary>
    /// Launch of a new ray in orthogonal projection
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public Ray fire_ray(float u, float v)
    {
        Point origin = new Point(-1, (1.0f - 2 * u) * aspect_ratio, 2 * v - 1);
        Vec direction = new Vec(1.0f, 0, 0);
        return Ray.Transform(this.transformation, new Ray (origin, direction, 1.0e-5f));
    }
}