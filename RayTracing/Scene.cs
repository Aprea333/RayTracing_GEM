using System.Buffers;
using System.Net.WebSockets;

using System.Net.Sockets;

using System.Net.WebSockets;


using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;
using NUnit.Framework;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace RayTracing;

//=============================================================
//SOURCE LOCATION
//============================================================
public class GrammarError: Exception
{
    public string message;
    public SourceLocation location;


    public GrammarError(string message, SourceLocation location)
    {
        this.message = message;
        this.location = location;
    }
    public GrammarError(string message)
    {
        this.message = message;
    }
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


//==============================================================================================

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
    Specular,
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
    Scaling,
    Sphere,
    Plane,
    CsgUnion, 
    CsgDifference,
    CsgIntersection
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
        { "material", EnumKeyword.Material },
        { "diffuse", EnumKeyword.Diffuse },
        { "diffuse", EnumKeyword.Specular },
        { "Uniform", EnumKeyword.Uniform },
        { "Checkered", EnumKeyword.Checkered },
        { "image", EnumKeyword.Image },
        { "Translation", EnumKeyword.Translation },
        { "Pigment", EnumKeyword.Pigment },
        { "World", EnumKeyword.World },
        { "Orthogonal", EnumKeyword.Orthogonal },
        { "Perspective", EnumKeyword.Perspective },
        { "rotation_x", EnumKeyword.RotationX },
        { "rotation_y", EnumKeyword.RotationY },
        { "rotation_z", EnumKeyword.RotationZ },
        { "float", EnumKeyword.Float },
        { "scale", EnumKeyword.Scaling },
        {"Sphere", EnumKeyword.Sphere},
        {"Sphere", EnumKeyword.Plane},
        {"CsgUnion", EnumKeyword.CsgUnion},
        {"CsgDifference", EnumKeyword.CsgDifference},
        {"CsgIntersection", EnumKeyword.CsgIntersection}
    };

    public KeywordToken(SourceLocation Location,EnumKeyword keyword)
    {
        this.Location = Location;
        this.keyword = keyword;
    }

    public override string ToString()
    {
        return keyword.ToString();
    }
    /*public string Write()
    {
        return keyword.ToString();
    }*/
}

public class LiteralNumberToken : Token
{
    public float Value;

    public LiteralNumberToken(float value, SourceLocation location)
    {
        Value = value;
        Location = location;
    }

    public string Write()
    {
        return Value.ToString();
    }
}

public class SymbolToken : Token
{
    public string Symbol;

    public SymbolToken(string symbol, SourceLocation location)
    {
        Symbol = symbol;
        Location = location;
    }

    public string Write()
    {
        return Symbol;
    }
}

public class StopToken : Token
{
    public StopToken(SourceLocation location)
    {
        Location = location;
    }
}

public class IdentifierToken : Token
{
    public string Identifier;

    public IdentifierToken(SourceLocation location, string s)
    {
        Location = location;
        Identifier = s;
    }

    public string Write()
    {
        return Identifier;
    }
}

public class StringToken : Token
{
    public string Str;
    public StringToken(SourceLocation location, string str)
    {
        Location = location;
        Str = str;
    }

    public string Write()
    {
        return Str;
    }
}

//================================================================================================
public class InputStream
{
  public Stream stream;
  public string saved_char;
  public SourceLocation location;
  public SourceLocation saved_location;
  public int tabulations;
  public Token? saved_token;
  public EnumKeyword EnumKeyword { get; }

  string SYMBOLS = "()<>[],*";
  
  public InputStream(Stream stream, string file_name = "", int tabulations = 8)
  {
    this.stream = stream;
    this.location = new SourceLocation(file_name, lineNum: 1, colNum: 1);
    this.saved_char = "";
    saved_location = location;
    this.tabulations = tabulations;
    saved_token = null;
  }

  public void update_pos(string ch)
  {
    if (ch == "")
    {
      return;
    }else if (ch == "\n")
    {
      location.line_num++;
      location.col_num = 1;
    }else if (ch == "\t")
    {
      location.col_num += tabulations;
    }
    else
    {
      location.col_num++;
    }
  }

  public string read_char()
  {
    string ch = "";
    if (saved_char != "")
    {
      ch = saved_char;
      saved_char = "";
    }
    else
    {
      var r_byte = stream.ReadByte();
      if (r_byte != -1) ch += (char)r_byte;
    }

    saved_location = location;
    update_pos(ch);
    return ch;
  }

  public void unread_char(string ch)
  {
      Assert.True(saved_char == "");
    saved_char = ch;
    location = saved_location;
  }


  public void skip_whitespaces_and_comments()
  {
    string ch = read_char();
    while (ch is "\t" or "\r" or "\n" or " " or "#")
    {
        if (ch == "#")
        {
            //It's a comment! Keep reading until the end of the line
            while (read_char() is not ("\r" or "\n" or ""))
            {
                //do nothing
            }
        }

        ch = read_char();
       if (ch ==  "") return;
    }
    unread_char(ch);
  }


  public StringToken parse_string_token(SourceLocation token_location)
  {
      var token = "";
      while (true)
      {
          string ch = read_char();
          if (ch == "\"")
          {
              break;
          }

          if (ch == "")
          {
              throw new GrammarError("Unterminated string",token_location);
          }

          token += ch;
      }

      return new StringToken(token_location, token);
  }

  public LiteralNumberToken parse_float_token(string first_char, SourceLocation token_location)
  {
      var token = first_char;
      float val;
      while (true)
      {
          var ch = read_char();
          bool all = Char.IsDigit(Convert.ToChar(ch, CultureInfo.InvariantCulture)) | ch == "." | ch == "e" | ch == "E";
         //bool all = Char.IsDigit(Convert.ToChar(ch)) |ch == "." | ch == "e" | ch == "E";
          if (all != true)
          {
              unread_char(ch);
              break;
          }

          token += ch;
      }

      try
      {
          var value = float.Parse(token, CultureInfo.InvariantCulture);
          //var value = float.Parse(token);
          val = value;
      }
      catch
      {
          throw new GrammarError($"'{token}' i and invalid floating-point number", token_location);
      }

      return new LiteralNumberToken(val, token_location);
  }
  public Token parse_keyword_or_identifier_token(string first_char, SourceLocation token_location)
  {
      var token = first_char;
      while (true)
      {
          string ch = read_char();
          if ((Char.IsLetterOrDigit(Convert.ToChar(ch)) | ch == "_")!= true)
          {
              unread_char(ch);
              break;
          }

          token += ch;
      }

      try
      {
          //return new KeywordToken(token_location, EnumKeyword);
          return new KeywordToken(token_location, KeywordToken.Dict[token]);
      }
      catch(KeyNotFoundException)
      {
          return new IdentifierToken(token_location, token);
      }
  }



  public Token read_token()
  {
      if (saved_token != null)
      {
          var result = saved_token;
          saved_token = null;
          return result;
      }

      skip_whitespaces_and_comments();
      string ch = read_char();

      if (ch == "")
          return new StopToken(location);

      var token_location = location;

      if (SYMBOLS.Contains(ch))
      {
          return new SymbolToken(ch, token_location);
      }

      if (ch == "\"")
      {
          return parse_string_token(token_location);
      }
     
      if (Decimal.TryParse(ch, out decimal number) | (new [] { "+", "-", "." }.Contains(ch)))
      

      {
          return parse_float_token(ch, token_location);
      }

      if (Char.IsLetter(ch[0]) || ch == "_")
      {
          return parse_keyword_or_identifier_token(ch, token_location);
      }

      throw new GrammarError("Invalid character" + ch, location);

  }

  public void unreadToken(Token tok)
  {
      Assert.True(saved_token != null);
      saved_token = tok;
  }
  }


public class Scene
{
    public World world;
    public IDictionary<string, Material> material;
    public Camera? cam;
    public IDictionary<string, float> float_variable;
    public List<string> overriden_variable;
    
    public Scene( World world, IDictionary<string,Material> material, Camera cam, IDictionary<string, float> float_variable,List<string> overriden_variable)
    {
        this.world = world;
        this.material = material;
        this.cam = cam;
        this.float_variable = float_variable;
        this.overriden_variable = overriden_variable;
    }

    public float expect_number(InputStream inputFile, Scene scene)
    {
        Token token = inputFile.read_token();
        if (token.GetType() == typeof(LiteralNumberToken))
            return ((LiteralNumberToken)token).Value;
        else if (token.GetType() == typeof(IdentifierToken))
        {
            string variable_name = ((IdentifierToken)token).Identifier;
            float appo;
            if (float_variable.TryGetValue(variable_name, out appo))
            {
                throw new GrammarError("Unknown variable" + token, token.Location);
            }

            return scene.float_variable[variable_name];

        }

        throw new GrammarError("got" + token + " instead of a number", token.Location);

    }

    public EnumKeyword expect_keywords(InputStream input_file, EnumKeyword[] keyword)
    {
        Token token = input_file.read_token();
        if (token is not KeywordToken)
        {
            throw new GrammarError(message: $"Expected a keyword instead of {token}", token.Location);
        }
        
        if (!keyword.Contains(((KeywordToken)token).keyword))
            throw new GrammarError(message: $"Expected one of the keywords {String.Join(',', keyword)}",
                token.Location);

        return ((KeywordToken)token).keyword;
    }

    public string expect_string(InputStream input_file)
    {
        Token token = input_file.read_token();
        if (token is not StringToken)
        {
            throw new GrammarError($"got  '{token}' instead of a string", token.Location);
        }

        return token.ToString();
    }

    public void expect_symbol(InputStream inputStream, string symbol)
    {
        Token tok = inputStream.read_token();
        
        if (tok is not SymbolToken)
        {

            throw new GrammarError($" {tok} is not a Symbol", tok.Location);
        }

        if (((SymbolToken)tok).Symbol != symbol)
        {
            throw new GrammarError($"got {tok} instead of {symbol}", tok.Location);
        }
    }

    public string? expect_identifier(InputStream inputStream)
    {
        Token tok = inputStream.read_token();
        if (tok is not IdentifierToken)
        {
            throw new GrammarError($"{tok} is not a identifier", tok.Location);
        }

        return tok.ToString();
        
    }
    public Vec parse_vector(InputStream inputStream, Scene scene)
    {
        expect_symbol(inputStream,"[");
        float x = expect_number(inputStream, scene);
        expect_symbol(inputStream,",");
        float y = expect_number(inputStream, scene);
        expect_symbol(inputStream ,",");
        float z = expect_number(inputStream, scene);
        expect_symbol(inputStream,"]");

        return new Vec(x, y, z);
    }
    
    
    public Colour parse_color(InputStream input_file, Scene scene)
    {
        expect_symbol(input_file, "<");
        float red = expect_number(input_file, scene);
        expect_symbol(input_file, ",");
        float green = expect_number(input_file, scene);
        expect_symbol(input_file, ",");
        float blue = expect_number(input_file, scene);
        expect_symbol(input_file, ">");
        return new Colour(red, green, blue);
    }


    public Pigment parse_pigment(InputStream input_file, Scene scene)
    {
        EnumKeyword[] key = { EnumKeyword.Checkered, EnumKeyword.Checkered, EnumKeyword.Image };
        var keyword = expect_keywords(input_file, key);
        Pigment result = null;
        expect_symbol(input_file, ")");
        if (keyword == EnumKeyword.Uniform)
        {
            Colour color = parse_color(input_file, scene);
            result = new UniformPigment(color);
        }
        else if (keyword == EnumKeyword.Checkered)
        {
            Colour color1 = parse_color(input_file, scene);
            expect_symbol(input_file, ",");
            Colour color2 = parse_color(input_file, scene);
            var num_of_steps = (int)expect_number(input_file, scene);
            result = new CheckeredPigment(color1, color2, num_of_steps);
        }
        else if (keyword == EnumKeyword.Image)
        {
            var file_name = expect_string(input_file);
            HdrImage img = new HdrImage();
            using (FileStream in_pfm = File.Open(file_name, FileMode.Open))
            {
                img.read_pfm_image(in_pfm);
            }

            result = new ImagePigment(img);
        }
        else
            throw new Exception("This line should be unreachable");

        expect_symbol(input_file, ")");
        return result;
    }

    public Transformation parse_transformation(InputStream input_file, Scene scene)
    {
        Transformation result = new Transformation();

        while (true)
        {
            EnumKeyword[] list =
            {
                EnumKeyword.Translation,
                EnumKeyword.Scaling,
                EnumKeyword.RotationX,
                EnumKeyword.RotationY,
                EnumKeyword.RotationZ
            };
        
            EnumKeyword keyword = expect_keywords(input_file, list);
        
            if (keyword == EnumKeyword.Translation)
            {
                expect_symbol(input_file, "(");
                result *= Transformation.translation(parse_vector(input_file, scene));
                expect_symbol(input_file, ")");
            }else if (keyword == EnumKeyword.Scaling)
            {
                expect_symbol(input_file, "(");
                float a = expect_number(input_file, scene);
                float b = expect_number(input_file, scene);
                float c = expect_number(input_file, scene);
                result *= Transformation.scaling(a, b, c);
                expect_symbol(input_file, ")");
            }else if (keyword == EnumKeyword.RotationX)
            {
                expect_symbol(input_file, "(");
                result *= Transformation.rotation_x(expect_number(input_file, scene));
                expect_symbol(input_file, ")");
            }else if (keyword == EnumKeyword.RotationY)
            {
                expect_symbol(input_file, "(");
                result *= Transformation.rotation_y(expect_number(input_file, scene));
                expect_symbol(input_file, ")");
            }else if (keyword == EnumKeyword.RotationZ)
            {
                expect_symbol(input_file, "(");
                result *= Transformation.rotation_z(expect_number(input_file, scene));
                expect_symbol(input_file, ")");
            }

            Token token = input_file.read_token();
            if (token is not SymbolToken || ((SymbolToken)token).Symbol != "*")
            {
                input_file.unreadToken(token);
            }
            return result;
        }
    }


    public Brdf parse_brdf(InputStream input_file, Scene scene)
    {
        EnumKeyword[] list = {EnumKeyword.Diffuse, EnumKeyword.Specular };
        EnumKeyword keyword = expect_keywords(input_file, list);
        expect_symbol(input_file, "(");
        Pigment pigment = parse_pigment(input_file, scene);
        expect_symbol(input_file, ")");
        if (keyword == EnumKeyword.Diffuse) return new DiffuseBrdf(pigment);
        else if (keyword == EnumKeyword.Specular) return new SpecularBrdf(pigment);

        throw new GrammarError("Expect Diffuse or Specular Pigment");
    }


    public (string?, Material) parse_material(InputStream input_file, Scene scene)
    {
        string? name = expect_identifier(input_file);
        expect_symbol(input_file, "(");
        Brdf brdf = parse_brdf(input_file, scene);
        expect_symbol(input_file, ",");
        Pigment pigment = parse_pigment(input_file, scene);
        expect_symbol(input_file, ")");
        return (name, new Material(brdf, pigment));
    }


    public Plane parse_plane(InputStream input_file, Scene scene)
    {
        expect_symbol(input_file,"(");
        Transformation tran = parse_transformation(input_file, scene);
        expect_symbol(input_file,",");
        (string name, Material material) = parse_material(input_file, scene);
        expect_symbol(input_file, ")");
        return new Plane(tran, material);
    }

    public Box parse_box(InputStream input_file, Scene scene)
    {
        expect_symbol(input_file,"(");
        Point max = parse_vector(input_file, scene).to_point();
        expect_symbol(input_file,",");
        Point min = parse_vector(input_file, scene).to_point();
        expect_symbol(input_file,",");
        Transformation tran = parse_transformation(input_file, scene);
        expect_symbol(input_file,",");
        (string name, Material material) = parse_material(input_file, scene);
        expect_symbol(input_file, ")");
        return new Box(max, min, tran, material);
    }
    public Camera parse_camera(InputStream input_file, Scene scene)
    {
        Camera result = new PerspectiveCamera();
        expect_symbol(input_file,"(");
        EnumKeyword[] list = {EnumKeyword.Perspective, EnumKeyword.Orthogonal};
        EnumKeyword keyword = expect_keywords(input_file, list);
        expect_symbol(input_file,",");
        Transformation transformation = parse_transformation(input_file, scene);
        expect_symbol(input_file,",");
        float aspect_ratio = expect_number(input_file, scene);
        expect_symbol(input_file,",");
        float distance = expect_number(input_file, scene);
        expect_symbol(input_file,")");

        if (keyword == EnumKeyword.Perspective)
        {
            return new PerspectiveCamera(distance, aspect_ratio, transformation);
        }else if (keyword == EnumKeyword.Orthogonal) return new OrthogonalCamera(transformation, aspect_ratio);

        throw new GrammarError("Expected Orthogonal or Perspective Camera");
    }


    public Sphere parse_sphere(InputStream input_file, Scene scene)
    {
        expect_symbol(input_file, "(");
        Transformation transformation = parse_transformation(input_file, scene);
        expect_symbol(input_file, ",");
        (string? name ,Material material) = parse_material(input_file, scene);
        expect_symbol(input_file, ")");
        return new Sphere(transformation, material);
    }

    public Shape parse_shape(InputStream input_file, Scene scene)
    {
        EnumKeyword[] list =
        {
            EnumKeyword.Sphere, EnumKeyword.Plane, EnumKeyword.Box, EnumKeyword.CsgDifference, EnumKeyword.CsgIntersection,
            EnumKeyword.CsgUnion
        };
        var keyword = expect_keywords(input_file, list);
        Shape shape = new Sphere();
        switch (keyword)
        {
            case EnumKeyword.Sphere:
                shape = parse_sphere(input_file, scene);
                break;
            case EnumKeyword.Plane:
                shape = parse_plane(input_file, scene);
                break;
            case EnumKeyword.Box:
                shape = parse_box(input_file, scene);
                break;
            case EnumKeyword.CsgDifference:
                shape = parse_difference(input_file, scene);
                break;
            case EnumKeyword.CsgIntersection:
                shape = parse_intersection(input_file, scene);
                break;
            case EnumKeyword.CsgUnion:
                shape = parse_union(input_file, scene);
                break;
            default:
                Assert.False(true, "Expected a shape");
                break;
        }
        return shape;
    }
    public CsgUnion parse_union(InputStream input_file, Scene scene)
    {
        expect_symbol(input_file, "(");
        Shape shape1 = parse_shape(input_file, scene);
        expect_symbol(input_file, ",");
        Shape shape2 = parse_shape(input_file, scene);
        expect_symbol(input_file, ",");
        Transformation transformation = parse_transformation(input_file, scene);
        expect_symbol(input_file, ",");
        (string? name ,Material material) = parse_material(input_file, scene);
        expect_symbol(input_file, ")");
        return new CsgUnion(shape1, shape2,transformation, material);
    }
    
    public CgsDifference parse_difference(InputStream input_file, Scene scene)
    {
        expect_symbol(input_file, "(");
        Shape shape1 = parse_shape(input_file, scene);
        expect_symbol(input_file, ",");
        Shape shape2 = parse_shape(input_file, scene);
        expect_symbol(input_file, ",");
        Transformation transformation = parse_transformation(input_file, scene);
        expect_symbol(input_file, ",");
        (string? name ,Material material) = parse_material(input_file, scene);
        expect_symbol(input_file, ")");
        return new CgsDifference(shape1, shape2,transformation, material);
    }
    public CsgIntersection parse_intersection(InputStream input_file, Scene scene)
    {
        expect_symbol(input_file, "(");
        Shape shape1 = parse_shape(input_file, scene);
        expect_symbol(input_file, ",");
        Shape shape2 = parse_shape(input_file, scene);
        expect_symbol(input_file, ",");
        Transformation transformation = parse_transformation(input_file, scene);
        expect_symbol(input_file, ",");
        (string? name ,Material material) = parse_material(input_file, scene);
        expect_symbol(input_file, ")");
        return new CsgIntersection(shape1, shape2,transformation, material);
    }
    
}