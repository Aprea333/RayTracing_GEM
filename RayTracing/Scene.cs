using System.Net.WebSockets;

using System.Diagnostics;
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
  //saved_token

  public InputStream(Stream stream, string file_name = "", int tabulations = 8)
  {
    this.stream = stream;
    this.location = new SourceLocation(file_name, lineNum: 1, colNum: 1);
    this.saved_char = "";
    saved_location = location;
    this.tabulations = tabulations;
    //saved_token
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
    Assert.True(ch == "");
    saved_char = ch;
    location = saved_location;
  }


  public void skip_whitespaces_and_comments()
  {
    string ch = read_char();
    while (ch is "\t" or "\r" or "\n" or " " or "#")
    {
      //It's a comment! Keep reading until the end of the line
      while (read_char() is not ("\r" or "\n" or ""))
      {
        //do nothing
      }

      ch = read_char();
      if (ch == "") return;
    }
    unread_char(ch);
  }
  
  
  
  
}
