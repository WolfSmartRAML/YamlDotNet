using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoreLinq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAML.Net.Types;
using RAML.Net.YAML;
using YamlDotNet.Serialization;

namespace RAML.Net
{
    public static class RAMLBuilder
    {
        static string[] METHODS = new string[]
        {
            "get",
            "post",
            "patch",
            "put",
            "delete",
            "options",
            "head"
        };

        public static Document Build(string ramlFile)
        {
            var doc = new Document();

            try
            {
                var raml1 = RAMLBuilder.CreateRAMLObjectGraph(ramlFile);

                var raml2 = RAMLBuilder.CreateJsonRAMLObjectGraph(raml1);

                // get all json datatypes
                doc.JsonDataTypes = ((JObject)raml2.Root).FindJsonSchemaDataTypes().ToDictionary(t => $"{t.Namespace}.{t.Name}");

                // get top level nodes
                var all = raml2.Properties().ToDictionary(p => p.Name);

                foreach (var key in all.Keys)
                {
                    switch (key)
                    {
                        case "title":
                        {
                            doc.title = all["title"].StringNode();
                        }
                            break;
                        case "description":
                        {
                            doc.description = all["description"].StringNode();
                        }
                            break;
                        case "version":
                        {
                            doc.version = all["version"].StringNode();
                        }
                            break;
                        case "baseUri":
                        {
                            doc.baseUri = all["baseUri"].StringNode();
                        }
                            break;
                        case "baseUriParameters":
                        {
                            doc.baseUriParameters = all["baseUriParameters"].UriParameters();
                        }
                            break;
                        case "protocols":
                        {
                            doc.protocols = all["protocols"].StringArrayNode();
                        }
                            break;
                        case "mediaType":
                        {
                            doc.mediaType = all["mediaType"].StringArrayNode();
                        }
                            break;
                        case "documentation":
                        {
                            doc.documentation = all["documentation"].DocumentationNode();
                        }
                            break;

                        case "schemas":
                            break;

                        case "types":
                            break;

                        case "traits":
                            break;

                        case "resourceTypes":
                            break;

                        case "annotationTypes":
                            break;

                        case "securitySchemes":
                        {
                            doc.securitySchemes = all["securitySchemes"].SecuritySchemes();
                        }
                            break;

                        case "securedBy":
                        {
                            doc.securedBy = all["securedBy"].StringArrayNode();
                        }
                            break;

                        case "uses":
                            break;

                        default:
                        {
                            foreach(var kv in all.Where(k => k.Key.StartsWith("/")))
                            {
                                var graph = kv.Value.BuildResourceGraph();
                                //var graph = ((JObject)kv.Value.Value).BuildResourceGraph();
                                doc.resources.Add(graph);     
                            }
                                            
                            var annotations = all.Where(k => k.Key.StartsWith("(") && k.Key.EndsWith(")")).ToList();
                        }
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return doc;
        }

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
            var jsonTypes = node.SelectTokens("$..type")
                .Where(t => t.GetType() == typeof(JObject) && (((JObject) t)["Extension"]).Value<string>() == "json")
                //.Select(t => ((JObject) t).ToObject<JsonSchemaDataType>())
                //.DistinctBy(t => $"{t.Namespace}.{t.Name}");
                .Select(t => new JsonSchemaDataType()
                {
                    //                    Name = ((JObject)t).Properties().First(p => p.Name == "Name")
                    Namespace = ((JObject)t).Properties().First(p => p.Name == "Namespace").Value.ToString(), //(((JObject)t)["Namspace"]).Value<string>(),
                    Name = ((JObject)t).Properties().First(p => p.Name == "Name").Value.ToString(),
                    IncludeNode = ((JObject)t).ToObject<IncludeNode>()
                })
                .DistinctBy(t => $"{t.Namespace}.{t.Name}");

            return jsonTypes;
        }

        public static ResourceNode BuildResourceGraph(this JProperty res, ResourceNode parent = null)
        //public static ResourceNode BuildResourceGraph(this JObject res, ResourceNode parent = null)
        {
            var obj = res.Value as JObject;

            var node = new ResourceNode()
            {
                Parent = parent,
                Resource = obj.ToObject<Resource>()
            };
            node.Resource.relativeUri = res.Name;

            foreach (var p in obj.Properties())
            {
                if (p.Name.StartsWith("/"))
                {
                    node.Children.Add(p.BuildResourceGraph(node));
                }
                else
                {
                    var n = p.Name;

                    if (METHODS.Any(m => m == n))
                    {
                        var m = p.Value as JObject;
                        node.Resource.Methods.Add(p.Name, m.ToObject<Method>());

                    }
                }
            }

            return node;
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

        //public static Dictionary<string, UriParameter> BaseUriParameters(this JProperty node)
        public static Dictionary<string, UriParameter> UriParameters(this JProperty node)
        {
            //var d = node.Values().Select(t => t.ToObject<UriParameter>())
            //    .ToDictionary(up => up.displayName);

            var dict = node.Values().Select(t => new UriParameter()
                {
                    displayName = ((JProperty)t).Name,
                    type = ((JProperty)t).Value["type"].Value<string>(),
                    description = ((JProperty)t).Value["description"].Value<string>()
                })
                .ToDictionary(up => up.displayName);

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