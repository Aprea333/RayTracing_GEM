
using System.Globalization;
using System.Runtime.InteropServices;

namespace RayTracing;

public class HDR
{
    public int width;
    public int height;
    public List<Colore> hdr_image = new List<Colore> ();
    
    public HDR()
    {
        width = 0;
        height = 0;
    }

    public HDR(int w, int h)
    {
        width = w;
        height = h;
        hdr_image.Capacity = width * height;
        for (int i = 0; i < w*h; i++)
        {
            Colore c = new Colore();
            hdr_image.Insert(i, c);
        }
    }
    
    public void set_pixel(Colore c, int x, int y)
    {
        hdr_image.Insert(y*width+x,c); 
    }

    public Colore get_pixel(int x, int y)
    {
        return hdr_image[y * width + x];
    }

    public float whatEndianness(string line3)
    {
        float end;
        try
        { 
            float.Parse(line3, CultureInfo.InvariantCulture.NumberFormat);
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Impossibile convertire a float " + line3); //throw new INvalidpfmfortmatexcepion
        }

        end = float.Parse(line3 ,CultureInfo.InvariantCulture.NumberFormat);
        //Console.WriteLine(end);
        if (end == 0)
        {
            throw new ArgumentOutOfRangeException("Gli unici valori validi sono +/- 1");
            //throw new INvalidpfmfortmatexcepion
        }
        return end;

    }
}


