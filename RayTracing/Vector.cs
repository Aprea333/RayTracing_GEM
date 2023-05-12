using System.Numerics;


namespace RayTracing;

/// <summary>
/// Vector in 3D space
/// </summary>

public struct Vec
{
    public float X;
    public float Y;
    public float Z;


    public Vec(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    /// <summary>
    /// Function that tests if two vectors are equal in a confidence interval
    /// </summary>
    /// <param name="a">First vector</param>
    /// <param name="b">second vector</param>
    /// <returns></returns>
    public static bool are_close(Vec a, Vec b)
    {
        return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y) + (a.Z - b.Z) * (a.Z - b.Z) < 0.00001;
    }
    
    /// <summary>
    /// Convert Vec to String
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "Vec(x=" + X + ", y=" + Y + ", z=" + Z + ")";
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
        c.X = a.X + b.X;
        c.Y = a.Y + b.Y;
        c.Z = a.Z + b.Z;
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
        c.X = a.X - b.X;
        c.Y = a.Y - b.Y;
        c.Z = a.Z - b.Z;
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
        c.X = a.X * b;
        c.Y = a.Y * b;
        c.Z = a.Z * b;
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
        c.X = a.X * b;
        c.Y = a.Y * b;
        c.Z = a.Z * b;
        return c;
    }

    /// <summary>
    /// Return a vector with opposite coordinates of the initial Vec
    /// </summary>
    /// <returns></returns>
    public Vec neg()
    {
        Vec n;
        n.X = -X;
        n.Y = -Y;
        n.Z = -Z;
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
        return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
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
        c.X = a.Y * b.Z - a.Z * b.Y;
        c.Y = a.Z * b.X - a.X * b.Z;
        c.Z = a.X * b.Y - a.Y * b.X;
        return c;
    }

    
    public float norm ()
    {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    }
    
    public float squared_norm ()
    {
        return X * X + Y * Y + Z * Z;
    }

    public void normalize()
    {
        float l = norm();
        X /= l;
        Y /= l;
        Z /= l;
    }

    /// <summary>
    /// Convert Vec to Normal
    /// </summary>
    /// <returns></returns>
    public Normal ToNormal()
    {
        Normal n;
        n.x = X;
        n.y = Y;
        n.z = Z;
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

