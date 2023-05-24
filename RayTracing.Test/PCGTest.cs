namespace RayTracing.Test;

public class PCGTest
{
    public PCG pcg = new PCG();
    
    
    [Test]
    public void TestRandom()
    {
        Assert.True(pcg.state == 1753877967969059832);
        Assert.True(pcg.inc == 109);

        foreach (var expected in new uint[] {2707161783, 2068313097, 3122475824, 2211639955,3215226955, 3421331566})
        {
            Assert.That(pcg.random(), Is.EqualTo(expected));
        }
    }
}