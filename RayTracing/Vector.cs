using System.Numerics;


namespace RayTracing;

/// <summary>
/// Vector in 3D space
/// </summary>

public struct Vec
{
    public float x;
    public float y;
    public float z;


    public Vec(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    /// <summary>
    /// Function that tests if two vectors are equal in a confidence interval
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">second vector</param>
    /// <returns></returns>
    public static bool are_close(Vec a, Vec b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z) < 0.00001;
    }
    
    /// <summary>
    /// Convert Vec to String
    /// </summary>
    /// <returns></returns>
    public string to_string()
    {
        return "Vec(x=" + x + ", y=" + y + ", z=" + z + ")";
    }
    
    /// <summary>
    /// Overloading operator + for sum of 2 Vecs 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vec operator +(Vec a, Vec b)
    {
        Vec c;
        c.x = a.x + b.x;
        c.y = a.y + b.y;
        c.z = a.z + b.z;
        return c;
    }
    
    /// <summary>
    /// Overloading operator - for subtraction of 2 Vecs
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vec operator -(Vec a, Vec b)
    {
        Vec c;
        c.x = a.x - b.x;
        c.y = a.y - b.y;
        c.z = a.z - b.z;
        return c;
    }
    
    /// <summary>
    /// Overloading operator * for product of Vec and scalar
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vec operator *(Vec a, float b)
    {
        Vec c;
        c.x = a.x * b;
        c.y = a.y * b;
        c.z = a.z * b;
        return c;
    }
    
    /// <summary>
    /// Overloading operator * for product of Vec and scalar
    /// </summary>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vec operator *(float b, Vec a)
    {
        Vec c;
        c.x = a.x * b;
        c.y = a.y * b;
        c.z = a.z * b;
        return c;
    }

    /// <summary>
    /// Return a vector with opposite coordinates of the initial Vec
    /// </summary>
    /// <returns></returns>
    public Vec neg()
    {
        Vec n;
        n.x = -x;
        n.y = -y;
        n.z = -z;
        return n;
    }

    /// <summary>
    /// Overloading operator * for scalar product 2 Vecs
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float operator *(Vec a, Vec b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }
    
    /// <summary>
    /// Overloading operator ^ for vector product of 2 Vecs
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vec operator ^(Vec a, Vec b)
    {
        Vec c;
        c.x = a.y * b.z - a.z * b.y;
        c.y = a.z * b.x - a.x * b.z;
        c.z = a.x * b.y - a.y * b.x;
        return c;
    }

    
    public float norm ()
    {
        return (float)Math.Sqrt(x * x + y * y + z * z);
    }
    
    public float squared_norm ()
    {
        return x * x + y * y + z * z;
    }

    public void normalize()
    {
        float l = norm();
        x /= l;
        y /= l;
        z /= l;
    }

    /// <summary>
    /// Convert Vec to Normal
    /// </summary>
    /// <returns></returns>
    public Normal ToNormal()
    {
        Normal n;
        n.x = x;
        n.y = y;
        n.z = z;
        return n;
    }
}

public struct Vec2D
{
    //First component
    public float u = 0.0f;
    
    //Second component
    public float v = 0.0f;

    public Vec2D(float u, float v)
    {
        this.u = u;
        this.v = v;
    }

    public static bool are_close(Vec2D a, Vec2D b)
    {
        float eps = 1e-5f;
        float diff_u = Math.Abs(a.u - b.u);
        float diff_v = Math.Abs(a.v - b.v);
        return diff_u < eps && diff_v < eps;
    }
}

