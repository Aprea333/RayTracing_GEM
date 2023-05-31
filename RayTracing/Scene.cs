namespace RayTracing;

public class GrammarError: Exception
{
    public string message;
    public SourceLocation location;
}