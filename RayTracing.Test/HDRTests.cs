using System.Runtime.InteropServices;
using System.Text;

namespace RayTracing.Test;

public class HDRTests
{
    public static int wtest = 100;
    public static int htest = 40;
    public HDR testHdr = new HDR(wtest,htest); //Test per il costruttore che inizializza a qualcosa
    public HDR testHdr0 = new HDR();           // Test per il costruttore che inizializza tutto a 0
    
    [Test]
    public void CheckCapacityCostruttore()
    {
        Assert.True(htest*wtest == testHdr.hdr_image.Capacity);
    }
    
    [Test]
    public void CheckCostruttoreNullo()
    {
        Assert.True((testHdr0.height,testHdr0.width) == (0,0));
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
        Assert.True(HDR.Parse_Img_Size("3 2") == (3,2) );
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.Parse_Img_Size("-1 3 "));
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.Parse_Img_Size("3 2 1 "));
    }
    
    [Test]
    public void parse_endianness_isLittle_test()
    {
        Assert.True(HDR.parse_endianness_isLittle("-0.2"));
        Assert.False(HDR.parse_endianness_isLittle("10.2"));
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.parse_endianness_isLittle("culo"));
        Assert.Throws<InvalidPfmFileFormatException>(() => HDR.parse_endianness_isLittle("0"));

    }

    [Test]
    public void read_pfm_image()
    {
        HDR culo = new HDR();
        string path = @"C:\Users\Utente\Desktop\reference_le.pfm"; //mio percorso per il file, da cambiare
        FileStream reference_file_reader =  File.Open(path, FileMode.Open);
        culo.read_pfm_image(reference_file_reader);

        Assert.True(culo.hdr_image.Capacity == 6);
        Assert.True(culo.width == 3);
        Assert.True(culo.height == 2);
        Assert.True(Colore.AreClose(culo.get_pixel(0,0), new Colore(10,20,30)));
        Assert.True(Colore.AreClose(culo.get_pixel(1,0), new Colore(40,50,60)));
        Assert.True(Colore.AreClose(culo.get_pixel(2,0), new Colore(70,80,90)));
        Assert.True(Colore.AreClose(culo.get_pixel(0,1), new Colore(100,200,300)));
        Assert.True(Colore.AreClose(culo.get_pixel(1,1), new Colore(400,500,600)));
        Assert.True(Colore.AreClose(culo.get_pixel(2,1), new Colore(700,800,900))); 
        Assert.True(culo.hdr_image.Capacity == 6);
    }
}
