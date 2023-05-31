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