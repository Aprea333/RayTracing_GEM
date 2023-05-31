namespace RayTracing.Test;

public class PigmentTest
{
    [Test]
    public void UniformPigment()
    {
        Colour color = new Colour(1f, 2f, 3f);
        Pigment pig = new UniformPigment(color);
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(0f, 0f)), color), "Test UniformPigment Vec(0,0)");
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(0f, 1f)), color), "Test UniformPigment Vec(0,1)");
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(1f, 0f)), color), "Test UniformPigment Vec(1,0)");
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(1f, 1f)), color), "Test UniformPigment Vec(1,1)");
    }
    
    [Test]
    public void ImagePigment()
    {
        HdrImage image = new HdrImage(2, 2);
        image.set_pixel(new Colour(1f,2f,3f), 0,0);
        image.set_pixel(new Colour(2f,3f,1f), 1,0);
        image.set_pixel(new Colour(2f,1f,3f), 0,1);
        image.set_pixel(new Colour(3f,2f,1f), 1,1);
        Pigment pig_image = new ImagePigment(image);
        Assert.True(Colour.are_close(pig_image.get_colour(new Vec2D(0f,0f)), new Colour(1f,2f,3f)), "Test ImagePigment Vec(0,0)");
        Assert.True(Colour.are_close(pig_image.get_colour(new Vec2D(0f,1f)), new Colour(2f,1f,3f)), "Test ImagePigment 2Vec(0,1)");
        Assert.True(Colour.are_close(pig_image.get_colour(new Vec2D(1f,0f)), new Colour(2f,3f,1f)), "Test ImagePigment Vec(1,0)");
        Assert.True(Colour.are_close(pig_image.get_colour(new Vec2D(1f,1f)), new Colour(3f,2f,1f)), "Test ImagePigment Vec(1,1)");
    }

    [Test]
    public void CheckeredPigment()
    {
        Colour color1 = new Colour(1f, 2f, 3f);
        Colour color2 = new Colour(10f, 20f, 30f);

        Pigment pig = new CheckeredPigment(color1, color2);
        /*With 2 steps the pattern should be like this:
                 (0.5,0)
        (0,0)+------+------+(1,0)
             | col1 | col2 |
      (0,0.5)+------+------+(1,0.5)
             | col2 | col1 |
        (0,1)+------+------+ (1,1)
        */
        
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(0.25f,0.25f)), color1), "Test CheckeredPigment in (0.25, 0.25)");
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(0.75f,0.25f)), color2), "Test CheckeredPigment in (0.75, 0.25)");
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(0.25f,0.75f)), color2), "Test CheckeredPigment in (0.25, 0.75)");
        Assert.True(Colour.are_close(pig.get_colour(new Vec2D(0.75f,0.75f)), color1), "Test CheckeredPigment in (0.75, 0.75)");

        
        
    }
    
}