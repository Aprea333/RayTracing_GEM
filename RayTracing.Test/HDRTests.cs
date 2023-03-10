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
}