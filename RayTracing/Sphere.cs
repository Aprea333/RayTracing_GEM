using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace RayTracing;

public class Sphere:Shape
{
    public Sphere(Transformation? tr = null, Material? material = null) : base(tr, material)
    {
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: RayTracing.Transformation; size: 2846MB")]
    public override HitRecord? ray_intersection(Ray r)
    {
        Ray inv_ray = Ray.transform(transformation.inverse(), r);
        Vec O = inv_ray.origin.convert_to_vec();
        float b = O * inv_ray.direction;
        float a = inv_ray.direction.squared_norm();
        float c = O.squared_norm()-1;
        float delta = b * b - a * c;
        if (delta <= 0)
        {
            return null;
        }
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/a;
        float tmax = (-b+delta_sqrt)/a;
        float hit_t;
        if (tmin > inv_ray.t_min && tmin < inv_ray.t_max)
        {
            hit_t = tmin;
        }else if (tmax > inv_ray.t_min && tmax < inv_ray.t_max)
        {
            hit_t= tmax;
        }
        else return null;

        Point hit_point = inv_ray.at(hit_t);
        return new HitRecord(transformation * hit_point, transformation * sphere_normal(hit_point, r.direction),
            sphere_point_to_uv(hit_point),hit_t, r, material);
    }

    public override List<HitRecord>? ray_intersection_list(Ray r)
    {
        Ray inv_ray = Ray.transform(transformation.inverse(), r);
        Vec O = inv_ray.origin.convert_to_vec();
        float b = O * inv_ray.direction;
        float a = inv_ray.direction.squared_norm();
        float c = O.squared_norm()-1;
        float delta = b * b - a * c;
        if (delta <= 0)
        {
            return null;
        }
        float delta_sqrt = (float)Math.Sqrt(delta);
        float tmin = (-b-delta_sqrt)/a;
        float tmax = (-b+delta_sqrt)/a;
        var intersection = new List<HitRecord>();
        var hit_point1 = inv_ray.at(tmin);
        var hit_point2 = inv_ray.at(tmax);
        
        if (tmin > inv_ray.t_min && tmin < inv_ray.t_max)
        {
            intersection.Add(new HitRecord(transformation * hit_point1, transformation * sphere_normal(hit_point1, r.direction),
                sphere_point_to_uv(hit_point1),tmin, r, material));
        }
        if (tmax > inv_ray.t_min && tmax < inv_ray.t_max)
        {
            intersection.Add(new HitRecord(transformation * hit_point2, transformation * sphere_normal(hit_point2, r.direction),
                sphere_point_to_uv(hit_point2),tmax, r, material));
        }

        return intersection.Count == 0 ? null : intersection;
    }

    public override bool is_internal(Point p)
    {
        p = transformation.inverse() * p;
        return p.convert_to_vec().squared_norm() < 1f;
    }

    public Normal sphere_normal(Point p, Vec dir)
    {
        Normal result = new Normal(p.x, p.y, p.z);
        if (p.convert_to_vec() * dir > 0) result = result.opposite_normal();
        return result;
    }

    public Vec2D sphere_point_to_uv(Point p)
    {
        float u = (float)(Single.Atan2(p.y, p.x)/(2*Math.PI));
        float v = (float)(Single.Acos(p.z)/Math.PI);
        return new Vec2D(u >= 0f ? u : u + 1f, v);
    }

    public static float sphere_casual_generation(int n_sphere, World world)
    {
        var sky_material = new Material(new DiffuseBrdf(new UniformPigment(new Colour(0, 0, 0))),
            new UniformPigment(new Colour(1, 0.9f, 0.5f)));
        var ground_material =
            new Material(new DiffuseBrdf(new CheckeredPigment(new Colour(0.3f, 0.5f, 0.1f),
                new Colour(0.1f, 0.2f, 0.5f))));
        var ground_material2 =
            new Material(new DiffuseBrdf(new UniformPigment(new Colour(0.53f, 0.81f, 0.98f))));
        var sphere2_material = new Material(new DiffuseBrdf(new UniformPigment(new Colour(0.7f, 0.85f, 0.9f))),
            new UniformPigment(new Colour()));
        var sphere1_material = new Material(new SpecularBrdf(new UniformPigment(new Colour(1f, 0f, 0f))),
            new UniformPigment(new Colour()));
        var sphere3_material = new Material(new SpecularBrdf(new UniformPigment(new Colour(0f, 1f, 0f))),
            new UniformPigment(new Colour()));


        
        world.add(new Sphere(Transformation.scaling(200,200,200)*Transformation.translation(new Vec(0,0,0.4f)), sky_material));
        world.add(new Plane(material:ground_material2));
        Console.WriteLine("\nInitializing shapes...");
        PCG pcg = new PCG();
        //limits
        float xlim = 20f;
        float ylim = 30f;
        float small = 0.5f;
        float large = 3f;
        int max_tries = 20; //maximum numbers of tries
        
        //Generating larges spheres
        List<Sphere> all_spheres = new List<Sphere>();
        List<Vec> center_large = new List<Vec>();
        Vec v1 = new Vec(0, 0, 1);
        Vec v2 = new Vec(2.5f, 2.5f, 1);
        Vec v3 = new Vec(6, 6, 1);
        center_large.Add(v1);
        center_large.Add(v2);
        center_large.Add(v3);
        List<Vec> center = new List<Vec>();
        
        Sphere large1 = new Sphere(Transformation.scaling(large, large, large) *
                                   Transformation.translation(v1), sphere1_material);
        Sphere large2 = new Sphere(Transformation.scaling(large, large, large) *
                                   Transformation.translation(v2), sphere2_material);
        Sphere large3 = new Sphere(Transformation.scaling(large, large, large) *
                                   Transformation.translation(v3), sphere3_material);
        all_spheres.Add(large1);
        all_spheres.Add(large2);
        all_spheres.Add(large3);
        foreach (Sphere s in all_spheres)
        {
            world.add(s);
        }

        
        //TEXTURE MOON
        HdrImage moon = new HdrImage();
        string input_pfm = "image/moon.pfm";
        using (FileStream inputStream = File.OpenRead(input_pfm))
        {
            moon.read_pfm_image(inputStream);
            Console.WriteLine($"Texture image has been correctly read from disk");
        }
        float min_x = xlim;
        float min_y = ylim;
        for (int i = 0; i < n_sphere; i++)
        {
            //Create material, percentage of diffuse to specular brdf is 50%
            Material mat = new Material();
            mat.emitted_radiance = new UniformPigment(Colour.black);

            if (i % 5 == 0)
            {
                mat.brdf = new DiffuseBrdf(new ImagePigment(moon));
                Console.WriteLine("\nAdd moon");
            }
            
            else if (i % 2 == 0)
                mat.brdf = new DiffuseBrdf(new UniformPigment(new Colour(pcg.random_float(), pcg.random_float(),
                    pcg.random_float())));
            else mat.brdf = new SpecularBrdf(new UniformPigment(new Colour(pcg.random_float(), pcg.random_float(),
                pcg.random_float())));
            
            bool is_internal = false;
            Sphere newSphere;
            int tries = 0;
            float x, y;

            while (!is_internal)
            {
                //generation floating number in (-lim, lim)
                x = pcg.random_float() * 2 * xlim - xlim;
                y = pcg.random_float() * 2 * ylim - ylim;
                is_internal = true;
                foreach (var s in center)
                {
                    float dif_x = (s.x - x) * (s.x - x);
                    float dif_y = (s.y - y) * (s.y - y);
                    if (Math.Sqrt(dif_x + dif_y) < 5f)
                    {
                        is_internal = false;
                        break;
                    }
                }

                if (is_internal)
                {
                    foreach (var s in center_large)
                    {
                        float dif_x = (s.x - x) * (s.x - x);
                        float dif_y = (s.y - y) * (s.y - y);
                        if (Math.Sqrt(dif_x + dif_y) < 5f)
                        {
                            is_internal = false;
                            break;
                        }
                    }
                }

                if (is_internal)
                {
                    newSphere = new Sphere(Transformation.scaling(small,small,small)*Transformation.translation(new Vec(x,y, 1)), mat);
                    world.add(newSphere);
                    all_spheres.Add(newSphere);
                    if (x < min_x) min_x = x;
                    if (y < min_y) min_y = y;
                    center.Add(new Vec(x,y,1));
                }
                else
                {
                    if (tries > max_tries) break;
                }
            }

        }

        int n = all_spheres.Count;
        Console.WriteLine($"\nNumber: {n}. \n{min_x}");
        for (int i = 0; i < center.Count; i++)
        {
            Console.WriteLine($"\nX: {center[i].x}, Y: {center[i].y} ");
        }
        return min_x;
    }
}