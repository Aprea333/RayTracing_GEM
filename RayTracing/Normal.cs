namespace RayTracing;

public struct Normal
{
    public float x;
    public float y;
    public float z;

    public Normal()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    
    public Normal(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public string NorToString()
    {
        return $"Normal (x = {this.x}, y = {this.y}, z = {this.z})";
    }

    public static bool are_close(Normal a, Normal b)
    {
        double epsilon = 1e-5;
        float x = a.x - b.x;
        float y = a.y - b.y;
        float z = a.z - b.z;

        return Math.Sqrt(x * x + y * y + z * z) < epsilon;
    }

    public Normal neg_normal()
    {
        return new Normal(-x, -y, -z);
    }

    public static Normal operator *(float scal, Normal n)
    {
        return new Normal(n.x * scal, n.y * scal, n.z * scal);
    }

    public static float operator *(Vec v, Normal n)
    {
        return v.X * n.x + v.Y * n.y + v.Z * n.z;
    }
    
    public static Vec operator ^(Vec v, Normal n)
    {
        float x = v.Y * n.z - v.Z * n.y;
        float y = v.Z * n.x - v.X * n.z;
        float z = v.X * n.y - v.Y * n.x;

        return new Vec(x,y,z);
    }

    public static Normal operator ^(Normal v, Normal n)
    {
        float x = v.y * n.z - v.z * n.y;
        float y = v.z * n.x - v.x * n.z;
        float z = v.x * n.y - v.y * n.x;

        return new Normal(x, y, z);
    }

    public float squared_norm()
    {
        return x * x + y * y + z * z;
    }

    public float norm()
    {
        return (float)Math.Sqrt(squared_norm());
    }

    public Normal normalization()
    {
        float scal = 1/squared_norm();
        return scal * this;
    }
}