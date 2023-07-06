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
        Assert.True(stream.location.line_num == 1);
        Assert.True(stream.location.col_num == 1);

        
        Assert.True(stream.read_char() == "a");
        Assert.True(stream.location.line_num == 1);
        Assert.True(stream.location.col_num == 2);

        stream.unread_char("A");
        Assert.True(stream.read_char() == "A");
        Assert.True(stream.location.line_num == 1);
        Assert.True(stream.location.col_num == 2);

        Assert.True(stream.read_char() == "b");
        Assert.True(stream.location.line_num == 1);
        Assert.True(stream.location.col_num == 3);

        Assert.True(stream.read_char() == "c");
        Assert.True(stream.location.line_num == 1);
        Assert.True(stream.location.col_num == 4);

        stream.skip_whitespaces_and_comments();
        Assert.True(stream.read_char() == "d");
        Assert.True(stream.location.line_num == 2);
        Assert.True(stream.location.col_num == 2);
        

        Assert.True(stream.read_char() == "\n");
        Assert.True(stream.location.line_num == 3);
        Assert.True(stream.location.col_num == 1);

        Assert.True(stream.read_char() == "e");
        Assert.True(stream.location.line_num == 3);
        Assert.True(stream.location.col_num == 2);

        Assert.True(stream.read_char() == "f");
        Assert.True(stream.location.line_num == 3);
        Assert.True(stream.location.col_num == 3);

        Assert.True(stream.read_char() == "");
    }


    public class AssertToken
    {
        public static void AssertIsKeyword(Token tok, EnumKeyword keyword)
        {
            Assert.IsTrue(tok is KeywordToken);
            Assert.True(((KeywordToken)tok).keyword == keyword);
        }

        public static void AssertIsIdentifier(Token tok, string ident)
        {
            Assert.IsTrue(tok is IdentifierToken);
            Assert.True(((IdentifierToken)tok).Identifier==ident);
        }

        public static void AssertIsStoptoken(Token tok)
        {
            Assert.IsTrue(tok is StopToken);
        }
        public static void AssertIsSymbol(Token tok, string symb)
        {
            Assert.IsTrue(tok is SymbolToken);
            Assert.True(((SymbolToken)tok).Symbol==symb);
        }

        public static void AssertLiteralnumber(Token tok, float a)
        {
            Assert.IsTrue(tok is LiteralNumberToken);
            Assert.True(Math.Abs(((LiteralNumberToken)tok).Value - a) < 1e-5);
        }
        public static void AssertIsString(Token tok, string stri)
        {
            Assert.That(tok is StringToken, Is.True);
            Assert.True(((StringToken)tok).Str==stri);
        }
        
        
        
        [Test]
        public void ReadToken_Test()
        {
            string inputString =@"
# This is a comment
# This is another comment
new material sky_material(
diffuse(image(""my file.pfm"")),
<5.0, 500.0, 300.0 >
) # Comment at the end of the line";

            Stream Streamline = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(inputString));
            var stream = new InputStream(Streamline);
            
            AssertIsKeyword(stream.read_token(), EnumKeyword.New);
            AssertIsKeyword(stream.read_token(),EnumKeyword.Material);
            AssertIsIdentifier(stream.read_token(),"sky_material");
            AssertIsSymbol(stream.read_token(),"(");
            AssertIsKeyword(stream.read_token(),EnumKeyword.Diffuse);
            AssertIsSymbol(stream.read_token(),"(");
            AssertIsKeyword(stream.read_token(),EnumKeyword.Image);
            AssertIsSymbol(stream.read_token(),"(");
            AssertIsString(stream.read_token(),"my file.pfm");
            AssertIsSymbol(stream.read_token(),")");
            AssertIsSymbol(stream.read_token(),")");
            AssertIsSymbol(stream.read_token(),",");
            AssertIsSymbol(stream.read_token(),"<");
            AssertLiteralnumber(stream.read_token(),5.0f);
            AssertIsSymbol(stream.read_token(),",");
            AssertLiteralnumber(stream.read_token(),500.0f);
            AssertIsSymbol(stream.read_token(),",");
            AssertLiteralnumber(stream.read_token(),300.0f);
            AssertIsSymbol(stream.read_token(),">");
            AssertIsSymbol(stream.read_token(),")");
            AssertIsStoptoken(stream.read_token());
            

        }

    }
}