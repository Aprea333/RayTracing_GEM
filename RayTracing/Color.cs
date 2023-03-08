
public class Color
{
    float r;
    float g;
    float b;
    public Color(float rc, float gc, float bc)
    {
        r = rc;
        g = gc;
        b = bc;
    }

    public void print_color()
    {
        Console.WriteLine("Color: (" + r + ", " + g + ", " + b + ")");
    }
}
    