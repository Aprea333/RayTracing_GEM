namespace RayTracing;

public struct Point
{
    public  float X;
    public float Y;
    public float Z;

    public Point()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }
    
    public  Point(float X,float Y, float Z )
    {
        this.X = X;
        this.Y = Y;
        this.Z = Z;
    }
    /// <summary>
    /// Converting to string
    /// </summary>
    /// <returns></returns>

    public string ToString()
    {
        return $"Point(X= {X}, Y = {Y}, Zn= {Z} ";
    }
    
    /// <summary>
    /// comparison between two point
    /// </summary>
    public static bool AreClose(Point a, Point b)
    {
        double epsilon = 1e-5;
        float diffX = a.X - b.X;
        float diffY = a.Y - b.Y;
        float diffz = a.Z - b.Z;
        return Math.Sqrt(Math.Pow(diffX, 2) +Math.Pow(diffY,2) + Math.Pow(diffz,2))<epsilon;
    }

    
    public static Point operator + (Point a, Vec v)
    {
        float sumX = a.X + v.X;
        float sumY = a.Y + v.Y;
        float sumZ = a.Z + v.Z;
        return new Point(sumX, sumY, sumZ);
    }

    public static Vec operator -(Point a, Point b)
    {
        float diffX = a.X - b.X;
        float diffY = a.Y - b.Y;
        float diffZ = a.Z - b.Z;
        return new Vec(diffX, diffY, diffZ);
    }
    
    public static Point operator -(Point a, Vec v)
    {
        float diffX = a.X - v.X;
        float diffY = a.Y - v.Y;
        float diffZ = a.Z - v.Z;
        return new Point(diffX, diffY, diffZ);
    }

    public Vec Convert()
    {
        return new Vec(this.X, this.Y, this.Z);
    }
    
}