using System.Runtime.InteropServices;
using RayTracing;

HDR culo = new HDR();

string path = @"C:\Users\Utente\Desktop\reference_le.pfm";
FileStream reference_file_reader =  File.Open(path, FileMode.Open);
culo.read_pfm_image(reference_file_reader);

for (int i = 0; i < 2; i++)
{
    for (int j = 0; j < 3; j++)
    {
        Console.WriteLine(culo.get_pixel(j,i).r_c);
        Console.WriteLine(culo.get_pixel(j,i).g_c);
        Console.WriteLine(culo.get_pixel(j,i).b_c);
    }
}

