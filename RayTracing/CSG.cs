namespace RayTracing;


//==========================================================================================================
// UNION === UNION === UNION === UNION === UNION === UNION === UNION === UNION === UNION === UNION === UNION
// =========================================================================================================

/// <summary>
/// Constructive Solid Geometry (CGS)
/// Union of two shapes
/// </summary>
public class CsgUnion:Shape
{
    public Shape s1;
    public Shape s2;

    public CsgUnion(Shape s1, Shape s2, Transformation? tran= null, Material? material=null) : base(tran, material)
    {
        this.s1 = s1;
        this.s2 = s2;
    }

    /// <summary>
    /// Check if a ray intersect the union shape
    /// </summary>
    /// <param name="r">The ray</param>
    /// <returns>The first element on the list of intersections. If no intersection is found "null" is returned.</returns>
    public override HitRecord? ray_intersection(Ray r)
    {
        
        return ray_intersection_list(r)?[0];
    }

    /// <summary>
    /// List of intersections sorted in ascending order by distance from the origin
    /// </summary>
    /// <param name="r"> The ray</param>
    /// <returns>The list of intersections. If no intersection is found, <"null"< is returned.</returns>
    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        var inv_ray = Ray.transform(transformation, r);
        var inter1 = s1.ray_intersection_list(inv_ray);
        var inter2 = s2.ray_intersection_list(inv_ray);

        var intersection = new List<HitRecord>(); 
        if(inter1 !=null) intersection.AddRange(inter1);
        if(inter2 !=null) intersection.AddRange(inter2);
        return intersection.Count != 0 ? intersection.OrderBy(o => o.t).ToList() : null;
    }

    public override bool is_internal(Point p)
    {
        p = transformation.inverse() * p;
        return s1.is_internal(p) || s2.is_internal(p);
    }
}


//=====================================================================================================
// DIFFERENCE === DIFFERENCE === DIFFERENCE === DIFFERENCE === DIFFERENCE === DIFFERENCE === DIFFERENCE
// ====================================================================================================
/// <summary>
/// Constructive Solid Geometry (CGS)
/// Difference between two shapes
/// </summary>

public class CsgDifference : Shape
{
    public Shape s1;
    public Shape s2;
    
    /// <summary>
    /// CgsDifference constructor
    /// </summary>
    /// <param name="s1">The first shape</param>
    /// <param name="s2">The second shape</param>
    /// <param name="tran">The transformation associated to the difference shape. If not specified it is initialized to the identity</param>
    /// <param name="material">The material of the difference shape </param>
    public CsgDifference(Shape s1, Shape s2, Transformation? tran= null, Material? material=null) : base(tran, material)
    {
        this.s1 = s1;
        this.s2 = s2;
    }
    
    /// <summary>
    /// Check if a ray intersect the difference shape
    /// </summary>
    /// <param name="r">The ray</param>
    /// <returns>The first element on the list of intersections. If no intersection is found "null" is returned.</returns>
    public override HitRecord? ray_intersection(Ray r)
    {
        return ray_intersection_list(r)?[0];
    }

    /// <summary>
    /// List of intersections sorted in ascending order by distance from the origin
    /// </summary>
    /// <param name="r"> The ray</param>
    /// <returns>The list of intersections. If no intersection is found, <"null"< is returned.</returns>
    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        var inv_ray = Ray.transform(transformation, r);
        var inter1 = s1.ray_intersection_list(inv_ray);
        if (inter1 == null) return null;
        //Remove all the intersections inside s2 
        for (int i = inter1.Count - 1; i >= 0; i--)
        {
            if (s2.is_internal(inter1[i].world_point)) inter1.RemoveAt(i);
        }
        //Remove all the intersections on the surface of s2 that are not inside s1
        var inter2 = s2.ray_intersection_list(inv_ray);
        if (inter2 != null)
        {
            for (int i = inter2.Count - 1; i >= 0; i--)
            {
                if(!s1.is_internal(inter2[i].world_point)) inter2.RemoveAt(i);
            }
        }
        //Create the new list
        var intersection = new List<HitRecord>();
        intersection.AddRange(inter1);
        if(inter2!=null) intersection.AddRange(inter2);
        return intersection.Count != 0 ? intersection.OrderBy(o => o.t).ToList() : null;

    }

    public override bool is_internal(Point p)
    {
        p = transformation.inverse() * p;
        return s1.is_internal(p) && !s2.is_internal(p);
    }
}

//==================================================================================================
// INTERSECTION === INTERSECTION === INTERSECTION === INTERSECTION === INTERSECTION === INTERSECTION
// =================================================================================================
/// <summary>
/// Constructive Solid Geometry (CGS)
/// Intersection between two shapes
/// </summary>

public class CsgIntersection : Shape
{
    
    public Shape s1;
    public Shape s2;
    
    /// <summary>
    /// CgsIntersection constructor
    /// </summary>
    /// <param name="s1">The first shape</param>
    /// <param name="s2">The second shape</param>
    /// <param name="tran">The transformation associated to the intersection shape. If not specified it is initialized to the identity</param>
    /// <param name="material">The material of the intersection shape </param>
    public CsgIntersection(Shape s1, Shape s2, Transformation? tran= null, Material? material=null) : base(tran, material)
    {
        this.s1 = s1;
        this.s2 = s2;
    }

    /// <summary>
    /// Check if a ray intersect the intersection shape
    /// </summary>
    /// <param name="r">The ray</param>
    /// <returns>The first element on the list of intersections. If no intersection is found "null" is returned.</returns>
    public override HitRecord? ray_intersection(Ray r)
    {
        return ray_intersection_list(r)?[0];
    }

    /// <summary>
    /// List of intersections sorted in ascending order by distance from the origin
    /// </summary>
    /// <param name="r"> The ray</param>
    /// <returns>The list of intersections. If no intersection is found, <"null"< is returned.</returns>
    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        var inv_ray = Ray.transform(transformation, r);
        var inter1 = s1.ray_intersection_list(inv_ray);
        if (inter1 == null) return null;
        //Remove all the intersections in s1 that are not inside s2 
        for (int i = inter1.Count - 1; i >= 0; i--)
        {
            if (!s2.is_internal(inter1[i].world_point)) inter1.RemoveAt(i);
        }
        //Remove all the intersections in s2 that are not inside s1
        var inter2 = s2.ray_intersection_list(inv_ray);
        if (inter2 == null) return null;
        for (int i = inter2.Count - 1; i >= 0; i--)
        {
            if(!s1.is_internal(inter2[i].world_point)) inter2.RemoveAt(i);
        }
        
        //Create the new list
        var intersection = new List<HitRecord>();
        intersection.AddRange(inter1);
        intersection.AddRange(inter2);
        return intersection.Count != 0 ? intersection.OrderBy(o => o.t).ToList() : null;

    }

    public override bool is_internal(Point p)
    {
        p = transformation.inverse() * p;
        return s1.is_internal(p) && s2.is_internal(p);
    }
}