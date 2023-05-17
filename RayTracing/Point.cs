namespace RayTracing;

public struct Point
{
    public  float x;
    public float y;
    public float z;

    public Point()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    
    public  Point(float x,float y, float z )
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    /// <summary>
    /// Converting to string
    /// </summary>
    /// <returns></returns>

    public string to_string()
    {
        return $"Point(X= {x}, Y = {y}, Zn= {z} ";
    }
    
    /// <summary>
    /// comparison between two point
    /// </summary>
    public static bool are_close(Point a, Point b)
    {
        double epsilon = 1e-5;
        float diffX = a.x - b.x;
        float diffY = a.y - b.y;
        float diffz = a.z - b.z;
        return Math.Sqrt(Math.Pow(diffX, 2) +Math.Pow(diffY,2) + Math.Pow(diffz,2))<epsilon;
    }

    
    public static Point operator +(Point a, Vec v)
    {
        float sumX = a.x + v.x;
        float sumY = a.y + v.y;
        float sumZ = a.z + v.z;
        return new Point(sumX, sumY, sumZ);
    }

    public static Vec operator -(Point a, Point b)
    {
        float diffX = a.x - b.x;
        float diffY = a.y - b.y;
        float diffZ = a.z - b.z;
        return new Vec(diffX, diffY, diffZ);
    }
    
    public static Point operator -(Point a, Vec v)
    {
        float diffX = a.x - v.x;
        float diffY = a.y - v.y;
        float diffZ = a.z - v.z;
        return new Point(diffX, diffY, diffZ);
    }

    public Vec convert_to_vec()
    {
        return new Vec(x,y,z);
    }
    
}