using System.Buffers;
using System.Net.WebSockets;

using System.Net.Sockets;

using System.Net.WebSockets;


using System.Diagnostics;
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
        { "Bbox", EnumKeyword.Box },
        { "Brdf", EnumKeyword.Brdf },
        { "Camera", EnumKeyword.Camera },
        { "Colour", EnumKeyword.Colour },
        { "material", EnumKeyword.Material },
        { "diffuse", EnumKeyword.Diffuse },
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
        { "scale", EnumKeyword.Scaling }
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
    public IDictionary<string, Material> dictionary;
    public Camera? cam;
    public IDictionary<string, float> float_variable;
    public List<string> overriden_variable;
    
    public Scene( World world, IDictionary<string,Material> dictionary, Camera cam, IDictionary<string, float> float_variable,List<string> overriden_variable)
    {
        this.world = world;
        this.dictionary = dictionary;
        this.cam = cam;
        this.float_variable = float_variable;
        this.overriden_variable = overriden_variable;
    }

    public float expectNumber(InputStream inputFile, Scene scene)
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

    public EnumKeyword expect_keywords(InputStream input_file, EnumKeyword keyword)
    {
        Token token = input_file.read_token();
        if (token is not KeywordToken)
        {
            throw new GrammarError(message: $"Expected a keyword instead of {token}", token.Location);
        }

        if (((KeywordToken)token).keyword != keyword)
        {
            throw new GrammarError(message: $"Expected one of the keywords {String.Join(',', keyword)}",
                token.Location);
        }

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

    public Pigment parse_pigment(InputStream input_file, Scene scene)
    {
        EnumKeyword [] key = {EnumKeyword.Checkered, EnumKeyword.Checkered, EnumKeyword.Image};
        var keyword = expect_keywords(input_file, key);
        
        expect_symbol(input_file, ")");
        if (keyword == EnumKeyword.Box)
    }
}