using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using SixLabors.ImageSharp;

namespace RayTracing.Test;

public class HDRTests
{
    public static int wtest = 100;
    public static int htest = 40;
    public HDR testHdr = new HDR(wtest, htest); //Test per il costruttore che inizializza a qualcosa
    public HDR testHdr0 = new HDR(); // Test per il costruttore che inizializza tutto a 0

    [Test]
    public void CheckCapacityCostruttore()
    {
        Assert.True(htest * wtest == testHdr.hdr_image.Capacity);
    }

    [Test]
    public void CheckCostruttoreNullo()
    {
        Assert.True((testHdr0.height, testHdr0.width) == (0, 0));
    }

    [Test]
    public void CheckInizializzazioneVettoreColorCostruttore()
    {
        bool tuttizeri()
        {
            for (int i = 0; i < testHdr.hdr_image.Capacity; i++)
            {
                if (testHdr.hdr_image[i].r_c != 0 || testHdr.hdr_image[i].g_c != 0 || testHdr.hdr_image[i].b_c != 0)
                {
                    return false;
                }
            }

            return true;
        }

        Assert.True(tuttizeri());
        testHdr.hdr_image[1].b_c = 1;
        Assert.False(tuttizeri());
        testHdr.hdr_image[1].b_c = 0; //rimettiamolo a posto
    }

    [Test]
    public void test_read_line()
    {
        var line = Encoding.ASCII.GetBytes($"hello\nworld");
        MemoryStream stream = new MemoryStream(line);
        Assert.True(HDR.read_line(stream) == "hello");
        Assert.True(HDR.read_line(stream) == "world");
        Assert.True(HDR.read_line(stream) == "");
    }


    [Test]
    public static void Parseimagesize_Test()
    {
        Assert.True(HDR.Parse_Img_Size("3 2") == (3, 2));
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.Parse_Img_Size("-1 3 "));
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.Parse_Img_Size("3 2 1 "));
    }

    [Test]
    public void parse_endianness_isLittle_test()
    {
        Assert.True(HDR.parse_endianness_isLittle("-0.2"));
        Assert.False(HDR.parse_endianness_isLittle("10.2"));
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.parse_endianness_isLittle("0"));
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.parse_endianness_isLittle("0"));

    }


    /*[Test]
    
    public void read_pfm_image()
    {
        string[] path = new string[2];
        path[0] = @"C:\Users\miche\RayTracing_GEM\reference_le.pfm";
        path[1] = @"C:\Users\miche\RayTracing_GEM\reference_be.pfm";
        for (int i = 0; i < 2; i++)
        {
            HDR image = new HDR();
            FileStream reference_file_reader = File.Open(path[i], FileMode.Open);
            image.read_pfm_image(reference_file_reader);

            Assert.True(image.hdr_image.Capacity == 6);
            Assert.True(image.width == 3);
            Assert.True(image.height == 2);
            Assert.True(Colore.AreClose(image.get_pixel(0, 0), new Colore(10, 20, 30)));
            Assert.True(Colore.AreClose(image.get_pixel(1, 0), new Colore(40, 50, 60)));
            Assert.True(Colore.AreClose(image.get_pixel(2, 0), new Colore(70, 80, 90)));
            Assert.True(Colore.AreClose(image.get_pixel(0, 1), new Colore(100, 200, 300)));
            Assert.True(Colore.AreClose(image.get_pixel(1, 1), new Colore(400, 500, 600)));
            Assert.True(Colore.AreClose(image.get_pixel(2, 1), new Colore(700, 800, 900)));
            Assert.True(image.hdr_image.Capacity == 6);
        }


    }*/


    [Test]
    public void read_pfm_image2()
    {
        byte[] LE_REFERENCE_BYTES =
        {
            0x50, 0x46, 0x0a, 0x33, 0x20, 0x32, 0x0a, 0x2d, 0x31, 0x2e, 0x30, 0x0a,
            0x00, 0x00, 0xc8, 0x42, 0x00, 0x00, 0x48, 0x43, 0x00, 0x00, 0x96, 0x43,
            0x00, 0x00, 0xc8, 0x43, 0x00, 0x00, 0xfa, 0x43, 0x00, 0x00, 0x16, 0x44,
            0x00, 0x00, 0x2f, 0x44, 0x00, 0x00, 0x48, 0x44, 0x00, 0x00, 0x61, 0x44,
            0x00, 0x00, 0x20, 0x41, 0x00, 0x00, 0xa0, 0x41, 0x00, 0x00, 0xf0, 0x41,
            0x00, 0x00, 0x20, 0x42, 0x00, 0x00, 0x48, 0x42, 0x00, 0x00, 0x70, 0x42,
            0x00, 0x00, 0x8c, 0x42, 0x00, 0x00, 0xa0, 0x42, 0x00, 0x00, 0xb4, 0x42
        };
        byte[] BE_REFERENCE_BYTES =
        {
            0x50, 0x46, 0x0a, 0x33, 0x20, 0x32, 0x0a, 0x31, 0x2e, 0x30, 0x0a, 0x42,
            0xc8, 0x00, 0x00, 0x43, 0x48, 0x00, 0x00, 0x43, 0x96, 0x00, 0x00, 0x43,
            0xc8, 0x00, 0x00, 0x43, 0xfa, 0x00, 0x00, 0x44, 0x16, 0x00, 0x00, 0x44,
            0x2f, 0x00, 0x00, 0x44, 0x48, 0x00, 0x00, 0x44, 0x61, 0x00, 0x00, 0x41,
            0x20, 0x00, 0x00, 0x41, 0xa0, 0x00, 0x00, 0x41, 0xf0, 0x00, 0x00, 0x42,
            0x20, 0x00, 0x00, 0x42, 0x48, 0x00, 0x00, 0x42, 0x70, 0x00, 0x00, 0x42,
            0x8c, 0x00, 0x00, 0x42, 0xa0, 0x00, 0x00, 0x42, 0xb4, 0x00, 0x00
        };

        Stream streamLe = new MemoryStream(LE_REFERENCE_BYTES);
        Stream streamBe = new MemoryStream(BE_REFERENCE_BYTES);
        Stream[] mystream = { streamLe, streamBe };
        for (int i = 0; i < 2; i++)
        {
            HDR image = new HDR();
            image.read_pfm_image(mystream[i]);

            Assert.True(image.hdr_image.Capacity == 6);
            Assert.True(image.width == 3);
            Assert.True(image.height == 2);
            Assert.True(Colore.AreClose(image.get_pixel(0, 0), new Colore(10, 20, 30)));
            Assert.True(Colore.AreClose(image.get_pixel(1, 0), new Colore(40, 50, 60)));
            Assert.True(Colore.AreClose(image.get_pixel(2, 0), new Colore(70, 80, 90)));
            Assert.True(Colore.AreClose(image.get_pixel(0, 1), new Colore(100, 200, 300)));
            Assert.True(Colore.AreClose(image.get_pixel(1, 1), new Colore(400, 500, 600)));
            Assert.True(Colore.AreClose(image.get_pixel(2, 1), new Colore(700, 800, 900)));
        }

    }

    [Test]
    public void average_luminosity_test()
    {
        HDR Img = new HDR(2, 1);
        Img.set_pixel(new Colore(5.0f, 10.0f, 15.0f), 0, 0);
        Img.set_pixel(new Colore(500.0f, 1000.0f, 1500.0f), 1, 0);

        Assert.True(Math.Round(100 - Img.average_luminosity(0.0f), 5) == 0.0f);
    }

    
    [Test]
    public void NormalizeImageTest()
    {
        HDR image = new HDR(2, 1);
        image.set_pixel(new Colore((float)5.0, (float)10.0, (float)15.0), 0, 0);
        image.set_pixel(new Colore((float)500.0, (float)1000.0, (float)1500.0), 1,0);
        image.NormalizeImage((float)1000.0, (float)100.0);
        Assert.True(Colore.AreClose(image.get_pixel(0,0), new Colore((float)0.5e2, (float)1.0e2, (float)1.5e2)));
        Assert.True(Colore.AreClose(image.get_pixel(1,0), new Colore((float)0.5e4, (float)1.0e4, (float)1.5e4)));
    }    

    [Test]
    public void clamp_image()
    {
        HDR Img = new HDR(2, 1);
        Img.set_pixel(new Colore(5.0f, 10.0f, 15.0f), 0, 0);
        Img.set_pixel(new Colore(500.0f, 1000.0f, 1500.0f), 1, 0);
        
        Img.clamp_image();

        foreach (Colore pix in Img.hdr_image)
        {
            Assert.True(pix.r_c >= 0 && pix.r_c <= 1);  
            Assert.True(pix.g_c >= 0 && pix.g_c <= 1);  
            Assert.True(pix.b_c >= 0 && pix.b_c <= 1);  
        }

    }
}
