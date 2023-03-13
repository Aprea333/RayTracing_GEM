using System;
using RayTracing

namespace RayTracing.TestProjec
{

    public class Tests
    {
        [Fact]
        public void Test1()
        {
            Color color1 = new Color(1.0f, 2.0f, 3.0f);
            Color color2 = new Color(5.0f, 6.0f, 7.0f);
            Assert.True(Color.AreClose(new Color(7.0f, 8.0f, 10.0f), color1 + color2, 1e-5));
        }

        public void Test2()
        {
            Color color1 = new Color(1.0f, 2.0f, 3.0f);
            Color color2 = new Color(5.0f, 6.0f, 7.0f);
            Assert.True(Color.Areclose(new Color(5.0f, 12.0f, 21.0f), color1 * color2, 1e-5));
        }

    }
    
}