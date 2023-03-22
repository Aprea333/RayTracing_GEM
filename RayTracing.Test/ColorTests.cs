namespace RayTracing.Tests;

public class ColorTest
{
    public Colore a = new Colore(1.0f, 2.0f, 3.0f);
    public Colore b = new Colore(5.0f, 6.0f, 7.0f);
    public float f = 2.0f;
    
    [Test]
    public void Costruttore()
    {
        Assert.True(Colore.AreClose(new Colore(1.0f, 2.0f, 3.0f), a));
    }
    
    [Test]
    public void Somma()
    {
        Colore c = new Colore(6.0f, 8.0f, 10.0f);
        Assert.True(Colore.AreClose(c, a + b));
    }
    
    [Test]
    public void Prodotto()
    {
        Colore c = new Colore(5.0f, 12.0f, 21.0f);
        Assert.True(Colore.AreClose(c, a*b));
    }
    
    [Test]
    public void Prodotto_scalare()
    {
        Colore c = new Colore(2.0f, 4.0f, 6.0f);
        Assert.True(Colore.AreClose(c, a*f));
    }
    /// <summary>
    /// Test for the Luminosity
    /// </summary>
    [Test]
    public void LuminosityTest()
    {
        Colore c = new Colore(9.0f, 5.0f, 7.0f);
        Assert.True(Math.Abs(a.Luminosity() - 2.0) < 1e-5);
        Assert.True(Math.Abs(c.Luminosity() - 7.0) < 1e-5);
    }
}