namespace RayTracing;

public interface camera
{
    Ray fire_ray(float u, float v);
}

public class PerspectiveCamera : camera
{
    private float distance;

    public float aspect_ratio {get;}
    private Tran T;
    
    public PerspectiveCamera(float Distance = 1.0f, float Aspect_Ratio =1.0f, Tran tran = null)
    {
        distance = Distance;
        aspect_ratio = Aspect_Ratio;
        if (tran == null)
        {
            T = new Tran();
        }else
        {
            T = tran;
        }
    }
    public Ray fire_ray(float u, float v)
    {
        Point origin = new Point(-distance, 0.0f, 0.0f);
        Vec direction = new Vec(-distance, (float)((1.0 - 2 * u) * aspect_ratio), 2 * v - 1);
        return Ray.Transform(T,new Ray(origin, direction, 1.0f) );
    }
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
        Point origin = new Point(-1, (1.0f - 2.0f * u) * aspect_ratio, 2 * v - 1);
        Vec direction = new Vec(1.0f, 0, 0);
        return Ray.Transform(transformation, new Ray (origin, direction, 1.0f));
    }
}