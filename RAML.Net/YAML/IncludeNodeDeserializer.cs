using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace RAML.Net.YAML
{
    public class IncludeNodeDeserializer : INodeDeserializer
    {
        #region State

        public string DocumentPath { get; set; }

        #endregion

        #region CtorDtor

        public IncludeNodeDeserializer(string documentPath)
        {
            DocumentPath = documentPath;
        }

    #endregion

        #region INodeDeserializer

        public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
        {
            value = null;

            if (typeof(IncludeNode) != expectedType)
            {
                value = null;
                return false;
            }

            var node = parser.Expect<Scalar>();


            var file = Path.Combine(DocumentPath, node.Value);
            using (var sr = new StringReader(new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)).ReadToEnd()))
            {
                //value = sr.ReadToEnd();
                value = new IncludeNode()
                {
                    Content = sr.ReadToEnd(),
                    File = Path.GetFileName(file),
                    FilePath = file,
                    Extension = Path.GetExtension(file)
            };
            }

            return true;
        }

        #endregion
    }
}
