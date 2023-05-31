using System.Text;
using System.IO;
using NuGet.Frameworks;

namespace RayTracing.Test;

public class SceneTest
{
    [Test]
    public void Inputstream_Test()
    {
        string inputString = "abc   \nd\nef";
        Stream Streamline = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(inputString));
        var stream = new InputStream(Streamline);
        
        Assert.True(stream.location.line_num ==1);
        Assert.True(stream.location.col_num==1);



        Assert.True(stream.read_char() == "a");
        Assert.True(stream.location.line_num==1);
        Assert.True(stream.location.col_num==2);

        stream.unread_char("A");
        Assert.True(stream.read_char() == "A");
        Assert.True(stream.location.line_num == 1);
        Assert.True(stream.location.col_num == 2);
        
        Assert.True(stream.read_char()=="b");
        Assert.True(stream.location.line_num==1);
        Assert.True(stream.location.col_num==3);
        
        Assert.True(stream.read_char()=="c");
        Assert.True(stream.location.line_num==1);
        Assert.True(stream.location.col_num==4);
        
        stream.skip_whitespaces_and_comments();
        
        Assert.True(stream.read_char()=="d");
        Assert.True(stream.location.line_num==2);
        Assert.True(stream.location.col_num==2);
        
        Assert.True(stream.read_char()=="\n");
        Assert.True(stream.location.line_num==3);
        Assert.True(stream.location.col_num==1);
        
        Assert.True(stream.read_char()=="e");
        Assert.True(stream.location.line_num==3);
        Assert.True(stream.location.col_num==2);
        
        Assert.True(stream.read_char()=="f");
        Assert.True(stream.location.line_num==3);
        Assert.True(stream.location.col_num==3);
        
        Assert.True(stream.read_char()=="");
    }
}