﻿namespace RayTracing;

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
    public string NorToString()
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
    public Normal neg_normal()
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
        return v.X * n.x + v.Y * n.y + v.Z * n.z;
    }
    
    /// <summary>
    /// Operator that computes the scalar product between a vector and a normal
    /// </summary>
    /// <param name="n">normal on the left side of the scalar product</param>
    /// <param name="v">vector on the right side of the scalar product</param>
    /// <returns>a float</returns>
    public static float operator *(Normal n, Vec v)
    {
        return v.X * n.x + v.Y * n.y + v.Z * n.z;
    }
    
    /// <summary>
    /// Operator that computes the cross product between a vector and a normal
    /// </summary>
    /// <param name="v"> vector on the left side of the cross product</param> 
    /// <param name="n"> normal on the right side of the cross product </param>
    /// <returns>a vector</returns>
    public static Vec operator ^(Vec v, Normal n)
    {
        float x = v.Y * n.z - v.Z * n.y;
        float y = v.Z * n.x - v.X * n.z;
        float z = v.X * n.y - v.Y * n.x;

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
        return scal * this;
    }
}