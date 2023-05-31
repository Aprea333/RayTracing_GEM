using System.Net.WebSockets;

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