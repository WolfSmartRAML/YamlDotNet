using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                using (var sr = new StreamReader(new FileStream(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.raml", FileMode.Open, FileAccess.Read, FileShare.Read)))
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

        [TestMethod]
        public void A002()
        {
            try
            {
                using (var sr = new StreamReader(new FileStream(@"C:\WIP\Projects\hmrc\githubWS\hmrc-api\apis\SA\application.raml", FileMode.Open, FileAccess.Read, FileShare.Read)))
                    //using(var ys = new YamlStream(sr))
                {
                    //var ys = new YamlStream(sr);
                    var y = sr.ReadToEnd();
                    var ys = new YamlDocument(y);
                    //var ys = new YamlStream();
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
    }
}
