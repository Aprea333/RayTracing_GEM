namespace RayTracing;

public interface Camera
{
    Ray fire_ray(float u, float v);
}

public class PerspectiveCamera : Camera
{
    public float distance { get; }

    public float aspect_ratio {get;}
    public Transformation T { get; }
    
    public PerspectiveCamera(float distance = 1.0f, float aspect_ratio =1.0f, Transformation? tran = null)
    {
        this.distance = distance;
        this.aspect_ratio = aspect_ratio;
        T = tran ?? new Transformation();
    }
    public Ray fire_ray(float u, float v)
    {
        Point origin = new Point(-distance, 0.0f, 0.0f);
        Vec direction = new Vec(distance, (float)((1.0 - 2 * u) * aspect_ratio), 2 * v - 1);
        return Ray.transform(T,new Ray(origin, direction, 1.0f) );
    }

    
}

   

/// <summary>
/// Class for the othogonal projection
/// </summary>
public class OrthogonalCamera : Camera
{
    public float aspect_ratio;
    public Transformation transformation;
    
    public OrthogonalCamera( Transformation? transformation = null, float aspect_ratio = 1.0f)
    {
        this.aspect_ratio = aspect_ratio;
        this.transformation = transformation ?? new Transformation();
    }
   
    /// <summary>
    /// Launch of a new ray in orthogonal projection
    /// </summary>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public Ray fire_ray(float u, float v)
    {
        Point origin = new Point(-1f, (1.0f - 2.0f * u) * aspect_ratio, 2 * v - 1);
        Vec direction = new Vec(1.0f, 0f, 0f);
        return Ray.transform(transformation, new Ray(origin, direction, 1.0f));
    }
}