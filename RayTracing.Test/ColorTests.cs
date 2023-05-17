namespace RayTracing.Test;

public class ColorTest
{
    public Colour a = new Colour(1.0f, 2.0f, 3.0f);
    public Colour b = new Colour(5.0f, 6.0f, 7.0f);
    public float f = 2.0f;
    
    [Test]
    public void Costruttore()
    {
        Assert.True(Colour.are_close(new Colour(1.0f, 2.0f, 3.0f), a));
    }
    
    [Test]
    public void Somma()
    {
        Colour c = new Colour(6.0f, 8.0f, 10.0f);
        Assert.True(Colour.are_close(c, a + b));
    }
    
    [Test]
    public void Prodotto()
    {
        Colour c = new Colour(5.0f, 12.0f, 21.0f);
        Assert.True(Colour.are_close(c, a*b));
    }
    
    [Test]
    public void Prodotto_scalare()
    {
        Colour c = new Colour(2.0f, 4.0f, 6.0f);
        Assert.True(Colour.are_close(c, a*f));
    }
    /// <summary>
    /// Test for the Luminosity
    /// </summary>
    [Test]
    public void LuminosityTest()
    {
        Colour c = new Colour(9.0f, 5.0f, 7.0f);
        Assert.True(Math.Abs(a.luminosity() - 2.0) < 1e-5);
        Assert.True(Math.Abs(c.luminosity() - 7.0) < 1e-5);
    }
}