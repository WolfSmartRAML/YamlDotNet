using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAML.Net;
using RAML.Net.Types;
using RAML.Net.YAML;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;

namespace Test.YAML.Net.RAML.HMRCAPI
{
    [TestClass]
    public class UnitTest1
    {
        #region Load HMRC RAML Documents

        [TestMethod]
        public void A001()
        {
            try
            {
                using (var sr = new StreamReader(new FileStream(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml", FileMode.Open, FileAccess.Read, FileShare.Read)))
                //using(var ys = new YamlStream(sr))
                {
                    //var ys = new YamlStream(sr);
                    //var ys = new YamlDocument();
                    var ys = new YamlStream();
                    ys.Load(sr);

                    var doc = ys.Documents[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //[TestMethod]
        //public void A002()
        //{
        //    try
        //    {
        //        using (var sr = new StreamReader(new FileStream(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml", FileMode.Open, FileAccess.Read, FileShare.Read)))
        //            //using(var ys = new YamlStream(sr))
        //        {
        //            //var ys = new YamlStream(sr);
        //            var y = sr.ReadToEnd();
        //            var yd = new YamlDocument(y);
        //            var ys = new YamlStream(); ys.
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        [TestMethod]
        public void A003()
        {
            try
            {
                using (var sr = new StreamReader(new FileStream(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml", FileMode.Open, FileAccess.Read, FileShare.Read)))
                    //using(var ys = new YamlStream(sr))
                {
                    //var ys = new YamlStream(sr);
                    //var ys = new YamlDocument();
                    var ys = new YamlStream();
                    ys.Load(sr);

                    var doc = ys.Documents[0];

                    foreach (var item in doc.RootNode.AllNodes)
                    {
                        var x = item;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Deserialize HMRC RAML Documents

        [TestMethod]
        public void B001()
        {
            try
            {
                using (var sr = new StringReader(new StreamReader(new FileStream(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml", FileMode.Open, FileAccess.Read, FileShare.Read)).ReadToEnd()))
                {
                    var ser = new DeserializerBuilder()
                                    .IgnoreUnmatchedProperties()
                                    .WithTagMapping("!include", typeof(IncludeNode)) 
                                    .WithNodeDeserializer(new IncludeNodeDeserializer(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA"))
                                    .Build();

                    var raml = ser.Deserialize<Document>(sr);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Deserialize HMRC RAML Documents

        [TestMethod]
        public void C001()
        {
            try
            {
                object raml = null;

                using (var sr = new StringReader(new StreamReader(new FileStream(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml", FileMode.Open, FileAccess.Read, FileShare.Read)).ReadToEnd()))
                {
                    // Create RAML Object Graph
                    var ser = new DeserializerBuilder()
                        //.IgnoreUnmatchedProperties()
                        .WithTagMapping("!include", typeof(IncludeNode))
                        .WithNodeDeserializer(new IncludeNodeDeserializer(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA"))
                        .Build();
                    var ramlstr = ser.Deserialize(sr);

                    // Convert to Json Object Graph
                    var serializer = new SerializerBuilder()
                        .JsonCompatible()
                        .Build();
                    var json = serializer.Serialize(ramlstr);

                    var jser = new JsonSerializer()
                    {

                    };
                    using (var jr = new JsonTextReader(new StringReader(json)))
                    {
                        raml = jser.Deserialize(jr);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [TestMethod]
        public void C002()
        {
            try
            {
                var raml1 = RAMLBuilder.CreateRAMLObjectGraph(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml");

                var raml2 = RAMLBuilder.CreateJsonRAMLObjectGraph(raml1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [TestMethod]
        public void C003()
        {
            try
            {
                var raml1 = RAMLBuilder.CreateRAMLObjectGraph(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml");

                var raml2 = RAMLBuilder.CreateJsonRAMLObjectGraph(raml1);

                var x1 = raml2.Properties().ToList();
                var x12 = raml2.Properties().ToDictionary(p => p.Name);
                var x13 = x12.Where(k => k.Key.StartsWith("/")).ToList();

                var x2 = raml2.PropertyValues().ToList();
                var x3 = raml2.Children().ToList();
                //var x4 = raml2.SelectTokens("$[starts-with('/')]").ToList();
                var x42 = raml2.SelectTokens("$.'/*'").ToList();
                var x43 = raml2.SelectTokens("/*").ToList();
                var x5 = raml2.AsJEnumerable();
                var x6 = raml2.Values();
                //var x7 = raml2.ToObject<XXXX>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [TestMethod]
        public void C004()
        {
            try
            {
                var raml1 = RAMLBuilder.CreateRAMLObjectGraph(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml");

                var raml2 = RAMLBuilder.CreateJsonRAMLObjectGraph(raml1);

                var doc = new Document();

                var all = raml2.Properties().ToDictionary(p => p.Name);

                doc.title = all["title"].Value.ToString();
                doc.description = all["description"].Value.ToString();
                doc.version = all["version"].Value.ToString();
                doc.baseUri = all["baseUri"].Value.ToString();
                var x0 = all["baseUriParameters"].Values().Select(t => new UriParameter()
                    {
                        displayName = ((JProperty)t).Name,
                        type = ((JProperty)t).Value["type"].Value<string>(),
                        description = ((JProperty)t).Value["description"].Value<string>()
                    })
                    .ToDictionary(up => up.displayName);
                doc.protocols = all["protocols"].Value.Select(p => (string)p).ToArray();

                var resources = all.Where(k => k.Key.StartsWith("/")).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [TestMethod]
        public void C005()
        {
            try
            {
                var raml1 = RAMLBuilder.CreateRAMLObjectGraph(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml");

                var raml2 = RAMLBuilder.CreateJsonRAMLObjectGraph(raml1);

                var doc = new Document();

                // get all json datatypes
                //var x = (JObject)raml2.Root;
                var z = ((JObject)raml2.Root).FindJsonSchemaDataTypes().ToDictionary(t => $"{t.Namespace}.{t.Name}");

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
                                var resources = all.Where(k => k.Key.StartsWith("/")).ToList();
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
        }

        [TestMethod]
        public void C006()
        {
            try
            {
                var doc = RAMLBuilder.Build(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.00.raml");
            }
            catch (Exception e)
            {
                throw;
            }
        }


        #endregion
    }

}