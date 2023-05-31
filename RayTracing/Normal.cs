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
    
    /// <summary>
    /// Function that returns the string containing the coordinates of the normal
    /// </summary>
    /// <returns></returns>
    public string normal_to_string()
    {
        return $"Normal (x = {x}, y = {y}, z = {z})";
    }
    
    /// <summary>
    /// Operator that tests if two normals are equals in a confidence interval
    /// </summary>
    /// <param name="a">first normal</param>
    /// <param name="b">second normal</param>
    /// <returns></returns>
    public static bool are_close(Normal a, Normal b)
    {
        double epsilon = 1e-5;
        float x = a.x - b.x;
        float y = a.y - b.y;
        float z = a.z - b.z;

        return Math.Sqrt(x * x + y * y + z * z) < epsilon;
    }
    
    /// <summary>
    /// Operator that computes the opposite of a normal
    /// </summary>
    /// <returns></returns>
    public Normal opposite_normal()
    {
        return new Normal(-x, -y, -z);
    }
    
    /// <summary>
    /// Operator that computes the product between a scalar and a normal
    /// </summary>
    /// <param name="scal">scalar on the left side of the product</param>
    /// <param name="n">normal on the right side of the product</param>
    /// <returns></returns>
    public static Normal operator *(float scal, Normal n)
    {
        return new Normal(n.x * scal, n.y * scal, n.z * scal);
    }
    
    /// <summary>
    /// Operator that computes the product between a scalar and a normal
    /// </summary>
    /// <param name="n"> normal on the left side of the product</param>
    /// <param name="scal">scalar on the left side of the product</param>
    /// <returns>a normal</returns>
    public static Normal operator *(Normal n, float scal)
    {
        return new Normal(n.x * scal, n.y * scal, n.z * scal);
    }
    
    /// <summary>
    /// Operator that computes the scalar product between a vector and a normal
    /// </summary>
    /// <param name="v">vector on the left side of the scalar product</param>
    /// <param name="n">normal on the right side of the scalar product</param>
    /// <returns></returns>
    public static float operator *(Vec v, Normal n)
    {
        return v.x * n.x + v.y * n.y + v.z * n.z;
    }
    
    /// <summary>
    /// Operator that computes the scalar product between a vector and a normal
    /// </summary>
    /// <param name="n">normal on the left side of the scalar product</param>
    /// <param name="v">vector on the right side of the scalar product</param>
    /// <returns>a float</returns>
    public static float operator *(Normal n, Vec v)
    {
        return v.x * n.x + v.y * n.y + v.z * n.z;
    }
    
    /// <summary>
    /// Operator that computes the cross product between a vector and a normal
    /// </summary>
    /// <param name="v"> vector on the left side of the cross product</param> 
    /// <param name="n"> normal on the right side of the cross product </param>
    /// <returns>a vector</returns>
    public static Vec operator ^(Vec v, Normal n)
    {
        float x = v.y * n.z - v.z * n.y;
        float y = v.z * n.x - v.x * n.z;
        float z = v.x * n.y - v.y * n.x;

        return new Vec(x,y,z);
    }
    
    /// <summary>
    /// Operator that computes the cross product between a vector and a normal
    /// </summary>
    /// <param name="n">normal on the left side of the cross product</param>
    /// <param name="v">vector on the left side of the cross product </param>
    /// <returns>a vector</returns>
    public static Vec operator ^(Normal n, Vec v)
    {
        Vec w = v ^ n;
        return w.neg();
    }

    /// <summary>
    /// Operator that computes the cross product between two normals
    /// </summary>
    /// <param name="v">normal on the left side of the cross product</param>
    /// <param name="n">vector on the right side of the cross product</param>
    /// <returns>a normal</returns>
    public static Normal operator ^(Normal v, Normal n)
    {
        float x = v.y * n.z - v.z * n.y;
        float y = v.z * n.x - v.x * n.z;
        float z = v.x * n.y - v.y * n.x;
        return new Normal(x, y, z);
    }
    
    /// <summary>
    /// Function that returns the squared norm of a normal
    /// </summary>
    /// <returns></returns>
    public float squared_norm()
    {
        return x * x + y * y + z * z;
    }
    
    /// <summary>
    /// Function that computes the norm of a normal
    /// </summary>
    /// <returns></returns>
    public float norm()
    {
        return (float)Math.Sqrt(squared_norm());
    }
    
    /// <summary>
    /// Function that normalizes a normal
    /// </summary>
    /// <returns></returns>
    public Normal normalization()
    {
        float scal = 1/norm();
        return new Normal(this.x*scal, this.y*scal, this.z*scal);
    }
    

    /// <summary>
    /// Creates a orthonormal basis (onb) from a normal representing the z axis.
    /// ATTENTION: The normal must be normalized when this method is invoked
    /// </summary>
    /// <param name="normal">The normal</param>
    /// <returns>tuple containing the three vectors (e1,e2,e3) of the basis</returns>
    public static (Vec, Vec, Vec) create_onb_from_z(Normal n)
    {
        Normal normal = n.normalization();
        float sign;
        if (normal.z > 0) sign = 1f;
        else sign = -1f;
        float a = (float)(-1.0f / (sign + normal.z));
        float b = normal.x * normal.y * a;

        Vec e1 = new Vec((float)(1.0 + sign * normal.x * normal.x * a), sign * b, -sign * normal.x);
        Vec e2 = new Vec(b, sign + normal.y * normal.y * a, -normal.y);
        Vec e3 = new Vec(normal.x, normal.y, normal.z);
        var onb = (e1, e2, e3);
        return onb;
    }
    
    public Vec To_vec()
    {
        return new(x, y, z);
    }
    
}




