namespace RayTracing;

public abstract class Pigment
{

    public abstract Colour get_colour(Vec2D uv);
}

public class UniformPigment : Pigment
{
    public Colour color { get; }

    public UniformPigment(Colour? color=null)
    {
        this.color = color ?? Colour.white;
    }
    public override Colour get_colour(Vec2D uv)
    {
        return color;
    }
    
}

public class CheckeredPigment : Pigment
{
    public Colour color1 { get; }
    public Colour color2 { get; }
    public int num_of_steps { get; }

    public CheckeredPigment(Colour color1, Colour color2, int numOfSteps = 10)
    {
        this.color1 = color1;
        this.color2 = color2;
        this.num_of_steps = numOfSteps;
    }

    public override Colour get_colour(Vec2D uv)
    {
        int u = (int)Math.Floor(uv.u * num_of_steps);
        int v = (int)Math.Floor(uv.v * num_of_steps);
        //If u and v are both even or both odd, u+v is even
        return (v + u) % 2 == 0 ? color1 : color2;
    }
}

public class ImagePigment : Pigment
{
    public HdrImage image;

    public ImagePigment(HdrImage img)
    {
        image = img;
    }

    public override Colour get_colour(Vec2D uv)
    {
        int col = (int)(uv.u * image.width);
        int row = (int)(uv.v * image.height);
        
        if (col >= image.width)
            col = image.width - 1;
        
        if (row >= image.height)
            row = image.height - 1;
        
        return image.get_pixel(col, row);
    }
}
