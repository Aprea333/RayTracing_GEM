namespace RayTracing;

interface camera
{
    void fire_ray(float u, float v);
    

}

public class PerspectiveCamera : camera
{
    private float distance;
    private float aspect_ratio;
    private Tran T;
    
    public void fire_ray(float u, float v)
    {
        Point origin = new Point(-this.distance, 0.0f, 0.0f);
        Vec direction = new Vec(-distance, (float)((1.0 - 2 * u) * aspect_ratio), 2 * v - 1);
        Ray r = new Ray(origin, direction, 1.0f);
        return r.transform(T);
    }

    public PerspectiveCamera(float d = 1.0f, float a =1.0f, Tran T = null)
    {
        this.distance = d;
        this.aspect_ratio = a;
        this.T = T;
    }

}