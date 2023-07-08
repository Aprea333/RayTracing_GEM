using System.Text;
using System.IO;
using NuGet.Frameworks;
using NUnit.Framework.Constraints;
using static RayTracing.Scene;

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
            Assert.True(((IdentifierToken)tok).Identifier == ident);
        }

        public static void AssertIsStoptoken(Token tok)
        {
            Assert.IsTrue(tok is StopToken);
        }

        public static void AssertIsSymbol(Token tok, string symb)
        {
            Assert.IsTrue(tok is SymbolToken);
            Assert.True(((SymbolToken)tok).Symbol == symb);
        }

        public static void AssertLiteralnumber(Token tok, float a)
        {
            Assert.IsTrue(tok is LiteralNumberToken);
            Assert.True(Math.Abs(((LiteralNumberToken)tok).Value - a) < 1e-5);
        }

        public static void AssertIsString(Token tok, string stri)
        {
            Assert.That(tok is StringToken, Is.True);
            Assert.True(((StringToken)tok).Str == stri);
        }
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
            
            AssertToken.AssertIsKeyword(stream.read_token(), EnumKeyword.New);
            AssertToken.AssertIsKeyword(stream.read_token(),EnumKeyword.Material);
            AssertToken.AssertIsIdentifier(stream.read_token(),"sky_material");
            AssertToken.AssertIsSymbol(stream.read_token(),"(");
            AssertToken.AssertIsKeyword(stream.read_token(),EnumKeyword.Diffuse);
            AssertToken.AssertIsSymbol(stream.read_token(),"(");
            AssertToken.AssertIsKeyword(stream.read_token(),EnumKeyword.Image);
            AssertToken.AssertIsSymbol(stream.read_token(),"(");
            AssertToken.AssertIsString(stream.read_token(),"my file.pfm");
            AssertToken.AssertIsSymbol(stream.read_token(),")");
            AssertToken.AssertIsSymbol(stream.read_token(),")");
            AssertToken.AssertIsSymbol(stream.read_token(),",");
            AssertToken.AssertIsSymbol(stream.read_token(),"<");
            AssertToken.AssertLiteralnumber(stream.read_token(),5.0f);
            AssertToken.AssertIsSymbol(stream.read_token(),",");
            AssertToken.AssertLiteralnumber(stream.read_token(),500.0f);
            AssertToken.AssertIsSymbol(stream.read_token(),",");
            AssertToken.AssertLiteralnumber(stream.read_token(),300.0f);
            Token tok = stream.read_token();
            AssertToken.AssertIsSymbol(tok,">");
            stream.unreadToken(tok);
            AssertToken.AssertIsSymbol(stream.read_token(),">");
            AssertToken.AssertIsSymbol(stream.read_token(),")");
            AssertToken.AssertIsStoptoken(stream.read_token());
            

        }

        [Test]
        public void TestParser()
        {
            var test = @"
                float clock(150)
                camera(perspective, rotation_z(30)* translation([-4, 0, 1]) , 1.0, 2.0)
                material sky_material(
                 diffuse(uniform(<0, 0, 0>)),
                 uniform(<0.7, 0.5, 1.0>)
                ) 
                
                # here is a comment
                
                material ground_material(
                 diffuse(checkered(<0.3, 0.5, 0.1>, 
                                    <0.1, 0.2, 0.5>, 4)),
                 uniform(<0, 0, 0>)
                )
    
                material sphere_material(
                 specular(uniform(<0.5, 0.5, 0.5>)),
                 uniform(<0, 0, 0>)
                )
   
                plane (sky_material, translation([0.0, 0, 100.000]) * rotation_y(clock))
                plane (ground_material, identity)
                # hi
                sphere(sphere_material, translation([0.0000, 0, 1.00]))
    
                "u8.ToArray();
            
            Stream stream = new MemoryStream(test);
            var inputStream = new InputStream(stream);
            var scene = Scene.parse_scene(inputFile:inputStream);
            //Check float variables
            Assert.True(scene.FloatVariables.Count == 1, "Test FloatVariables length");
            Assert.True(scene.FloatVariables.ContainsKey("clock"), "Test FloatVariables contains");
            Assert.True(Functions.are_close(scene.FloatVariables["clock"],150.0f), "Test FloatVariables value");

            //Check materials
            Assert.True(scene.Materials.Count == 3, "Test Materials length");
            Assert.True(scene.Materials.ContainsKey("sphere_material"), "Test Materials contains 1");
            Assert.True(scene.Materials.ContainsKey("sky_material"), "Test Materials contains 2");
            Assert.True(scene.Materials.ContainsKey("ground_material"), "Test Materials contains 3");

            var sphereMaterial = scene.Materials["sphere_material"];
            var skyMaterial = scene.Materials["sky_material"];
            var groundMaterial = scene.Materials["ground_material"];
            
            //Check skyMaterial
            Assert.That(skyMaterial.brdf, Is.TypeOf(typeof(DiffuseBrdf)));
            Assert.That(skyMaterial.brdf.Pig, Is.TypeOf(typeof(UniformPigment)));
            Colour c = ((UniformPigment)skyMaterial.brdf.Pig).color;
            Assert.True(Colour.are_close(c, new Colour()));
            
            //Check groundMaterial
            Assert.That(groundMaterial.brdf, Is.TypeOf(typeof(DiffuseBrdf)));
            Assert.That(groundMaterial.brdf.Pig, Is.TypeOf(typeof(CheckeredPigment)));
            Colour c1 = ((CheckeredPigment)groundMaterial.brdf.Pig).color1;
            Colour c2 = ((CheckeredPigment)groundMaterial.brdf.Pig).color2;
            Assert.True(Colour.are_close(c1, new Colour(0.3f,0.5f,0.1f)));
            Assert.True(Colour.are_close(c2, new Colour(0.1f,0.2f,0.5f)));
            Assert.True(((CheckeredPigment)groundMaterial.brdf.Pig).num_of_steps == 4);
            
            //Check sphereMaterial
            Assert.That(sphereMaterial.brdf, Is.TypeOf(typeof(SpecularBrdf)));
            Assert.That(sphereMaterial.brdf.Pig, Is.TypeOf(typeof(UniformPigment)));
            Colour c_sph = ((UniformPigment)sphereMaterial.brdf.Pig).color;
            Assert.True(Colour.are_close(c_sph, new Colour(0.5f,0.5f,0.5f)));

            //Check emitted radiance
            Assert.That(skyMaterial.emitted_radiance, Is.TypeOf(typeof(UniformPigment)));
            Colour em_sky = ((UniformPigment)skyMaterial.emitted_radiance).color;
            Assert.True(Colour.are_close(em_sky, new Colour(0.7f, 0.5f, 1.0f)));
            Assert.That(groundMaterial.emitted_radiance, Is.TypeOf(typeof(UniformPigment)));
            Colour em_ground = ((UniformPigment)groundMaterial.emitted_radiance).color;
            Assert.True(Colour.are_close(em_ground, new Colour()));
            Assert.That(sphereMaterial.emitted_radiance, Is.TypeOf(typeof(UniformPigment)));
            Colour em_sphere = ((UniformPigment)sphereMaterial.emitted_radiance).color;
            Assert.True(Colour.are_close(em_sphere, new Colour()));
            
            //Check the shapes
            Assert.True(scene.Wd.shapes.Count == 3, "World lenght");
            Assert.That(scene.Wd.shapes[0], Is.TypeOf(typeof(Plane)));
            Transformation tr_shape1 = Transformation.translation(new Vec(0, 0, 100)) * Transformation.rotation_y(150);
            Assert.True(Transformation.are_close(scene.Wd.shapes[0].transformation, tr_shape1));
            Assert.That(scene.Wd.shapes[1], Is.TypeOf(typeof(Plane)));
            Assert.True(Transformation.are_close(scene.Wd.shapes[1].transformation, new Transformation()));
            Assert.That(scene.Wd.shapes[2], Is.TypeOf(typeof(Sphere)));
            Assert.True(Transformation.are_close(scene.Wd.shapes[2].transformation, Transformation.translation(new Vec(0,0,1))));
            
            //Check camera
            Assert.That(scene.Camera, Is.TypeOf(typeof(PerspectiveCamera)));
            Transformation tr_camera = Transformation.rotation_z(30)*Transformation.translation(new Vec(-4,0,1));
            Transformation tr_test = Transformation.translation(new Vec(-4,0,1))*Transformation.rotation_z(30);
            Assert.True(Transformation.are_close(tr_camera, ((PerspectiveCamera)scene.Camera).T));
            Assert.False(Transformation.are_close(tr_test, ((PerspectiveCamera)scene.Camera).T));
            Assert.True(Functions.are_close(((PerspectiveCamera)scene.Camera).aspect_ratio, 1));
            Assert.True(Functions.are_close(((PerspectiveCamera)scene.Camera).distance, 2));
            
        }

    }
