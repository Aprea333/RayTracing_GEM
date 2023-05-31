using System.Net.Sockets;
using RayTracing;

namespace RayTracing;

public class GrammarError: Exception
{
    public string message;
    public SourceLocation location;
}
public struct SourceLocation
{
  public string file_name = "";
  public int line_num = 0;
  public int col_num = 0;
  
  public SourceLocation(string fileName, int lineNum, int colNum)
  {
    file_name = fileName;
    line_num = lineNum;
    col_num = colNum;
  }
}

public abstract class Token
{

    public SourceLocation Location;

    
}


public enum EnumKeyword 
{
    New,
    Box,
    Brdf,
    Camera,
    Colour,
    Material,
    Diffuse,
    Uniform,
    Checkered,
    Image,
    Translation,
    Pigment,
    World,
    Orthogonal,
    Perspective,
    RotationX,
    RotationY,
    RotationZ,
    Float,
    Scaling
}

public class KeywordToken : Token
{
    public EnumKeyword keyword;

    public static IDictionary<string, EnumKeyword> Dict = new Dictionary<string, EnumKeyword>
    {
        { "new", EnumKeyword.New },
        { "Box", EnumKeyword.Box },
        { "Brdf", EnumKeyword.Brdf },
        { "Camera", EnumKeyword.Camera },
        { "Colour", EnumKeyword.Colour },
        { "Material", EnumKeyword.Material },
        { "Diffuse", EnumKeyword.Diffuse },
        { "Uniform", EnumKeyword.Uniform },
        { "Checkered", EnumKeyword.Checkered },
        { "Image", EnumKeyword.Image },
        { "Translation", EnumKeyword.Translation },
        { "Pigment", EnumKeyword.Pigment },
        { "World", EnumKeyword.World },
        { "Orthogonal", EnumKeyword.Orthogonal },
        { "Perspective", EnumKeyword.Perspective },
        { "rotation_x", EnumKeyword.RotationX },
        { "rotation_y", EnumKeyword.RotationY },
        { "rotation_z", EnumKeyword.RotationZ },
        { "float", EnumKeyword.Float },
        { "scale", EnumKeyword.Scaling }
    };

    public KeywordToken(SourceLocation Location,EnumKeyword keyword)
    {
        this.Location = Location;
        this.keyword = keyword;
    }
    
    public string Write()
    {
        return keyword.ToString();
    }
}




