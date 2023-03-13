namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1(Color a, Color b)
    {
        Color color1 = new Color(1.0, 2.0, 3.0)
        Color color2 = new Color(5.0, 6.0, 7.0)
        Debug.Assert(Color.AreClose(color1+color2, (6.0, 8.0, 10.0), 1e-5));
    }
    public void Test2(Color a, Color b)
    {
        Color color1 = new Color(1.0, 2.0, 3.0)
        Color color2 = new Color(5.0, 6.0, 7.0)
        Debug.Assert(Color.AreClose(color1*color2, (5.0, 12.0, 21.0), 1e-5)

    }