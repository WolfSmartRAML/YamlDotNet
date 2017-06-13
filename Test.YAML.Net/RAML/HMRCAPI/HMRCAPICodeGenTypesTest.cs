using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using NJsonSchema.CodeGeneration.TypeScript;
using RAML.Net;

namespace Test.YAML.Net.RAML.HMRCAPI
{
    [TestClass]
    public class HMRCAPICodeGenTypesTest
    {
        [TestMethod]
        public async Task A001()
        {
            try
            {
                if (Directory.Exists("./Out"))
                {
                    Directory.Delete("./Out", true);
                }
                Directory.CreateDirectory("./Out");

                var doc = RAMLBuilder.Build(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml");

                var jsonTypes = doc.JsonDataTypes;
                foreach (var t in doc.JsonDataTypes.Values)
                {
                    var ns = $"Bosh.{t.IncludeNode.Namespace}";
                    //var schema = new JsonSchema4() { };
                    //var schema = JsonSchema4.FromData(t.IncludeNode.Content); excp!
                    var schema = await JsonSchema4.FromFileAsync(t.IncludeNode.FilePath);

                    var genCS = new CSharpGenerator(schema, new CSharpGeneratorSettings()
                    {
                        Namespace = ns,
                        ClassStyle = CSharpClassStyle.Poco,
                        ArrayType = $"IEnumerable" //<{t.IncludeNode.Name}>"

                    });
                    var fileCS = genCS.GenerateFile();
                    File.WriteAllText(Path.Combine("./Out", t.IncludeNode.Name + ".cs"), fileCS);
 
                    var genTS = new TypeScriptGenerator(schema, new TypeScriptGeneratorSettings()
                    {
                        Namespace = "Bosh"
                    });
                    var fileTS = genTS.GenerateFile();
                    File.WriteAllText(Path.Combine("./Out", t.IncludeNode.Name + ".ts"), fileTS);

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
