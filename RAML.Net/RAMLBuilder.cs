using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAML.Net.Types;
using RAML.Net.YAML;
using YamlDotNet.Serialization;

namespace RAML.Net
{
    public static class RAMLBuilder
    {
        public static object CreateRAMLObjectGraph(string ramlFile)
        {
            try
            {
                using (var sr =
                    new StringReader(new StreamReader(
                        new FileStream(ramlFile,
                            FileMode.Open, FileAccess.Read, FileShare.Read)).ReadToEnd()))
                {
                    // Create RAML Object Graph
                    var ser = new DeserializerBuilder()
                        //.IgnoreUnmatchedProperties()
                        .WithTagMapping("!include", typeof(IncludeNode))
                        .WithNodeDeserializer(
                            new IncludeNodeDeserializer(Path.GetDirectoryName(ramlFile)))
                        .Build();
                    var raml = ser.Deserialize(sr);

                    return raml;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static JObject CreateJsonRAMLObjectGraph(object raml)
        {
            try
            {
                var serializer = new SerializerBuilder()
                    .JsonCompatible()
                    .Build();
                var json = serializer.Serialize(raml);

                var jser = new JsonSerializer()
                {

                };
                using (var jr = new JsonTextReader(new StringReader(json)))
                {
                    var jsonRAML = jser.Deserialize(jr);
                    return jsonRAML as JObject;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static IEnumerable<JsonSchemaDataType> FindJsonSchemaDataTypes(this JObject node)
        {
            var jsonTypes0 = node.SelectTokens("$..type").ToList();
            var jsonTypes1 = node.SelectTokens("$..type[?(@.Extension == 'json')]").ToList();
            var jsonTypes2 = node.SelectTokens("$..type").Where(n => n["Extension"].Value<string>() == "json").ToList();

            //    .Select(t => new JsonSchemaDataType()
            //{
            //    Name = ((JObject)t).Properties().First(p => p.Name == "Name")
            //});

            return new List<JsonSchemaDataType>();
        }




        public static string StringNode(this JProperty node)
        {
            //var vl = node.Value<string>();
            var vl = node.Value.ToString();

            return vl;
        }
        public static string[] StringArrayNode(this JProperty node)
        {
            string[] vl = null;

            try
            {
                vl = node.Value.Select(p => (string)p).ToArray();

                if (vl.Length == 0)
                {
                    // to cope with mediatype (others?) being either a single string or an array of strings ...
                    var p = node.StringNode();
                    vl = new string[] { p };
                }
            }
            catch (Exception e)
            {
                // to cope with mediatype (others?) being either a single string or an array of strings ...
                var p = node.StringNode();
                vl = new string[] { p };
            }

            return vl;
        }

        public static Dictionary<string, UserDocumentation> DocumentationNode(this JProperty node)
        {
            var dict = node.Values().Select(t => new UserDocumentation()
                {
                    title = ((JObject)t).Properties().Where(p => p.Name == "title").Select(p => (string)p/*.Value*/).First(),
                    content = ((JObject)t).Properties().First(p => p.Name == "content").Value.ToObject<IncludeNode>()
                })
                .ToDictionary(ud => ud.title);

            return dict;
        }

        public static Dictionary<string, UriParameter> BaseUriParameters(this JProperty node)
        {
            var dict = node.Values().Select(t => new UriParameter()
                {
                    name = ((JProperty)t).Name,
                    type = ((JProperty)t).Value["type"].Value<string>(),
                    description = ((JProperty)t).Value["description"].Value<string>()
                })
                .ToDictionary(up => up.name);

            return dict;
        }

        public static Dictionary<string, SecurityScheme> SecuritySchemes(this JProperty node)
        {
            var dict = new Dictionary<string, SecurityScheme>();

            var data = (JObject)node.Value;

            foreach (var sec in node.Value.Children<JProperty>())
            {
                var name = sec.Name;

                var sch = sec.Value.ToObject<SecurityScheme>();

                dict[name] = sch;
            }

            return dict;
        }
    }

}