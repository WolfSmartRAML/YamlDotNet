using System;
using System.Collections.Generic;
using System.Text;
using RAML.Net.YAML;

namespace RAML.Net.Types
{
    public class DataType
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
    }

    public class JsonSchemaDataType : DataType
    {
        public IncludeNode IncludeNode { get; set; }
    }
    public class XmlSchemaDataType : DataType
    {
        public IncludeNode IncludeNode { get; set; }
    }
    public class RAMLType : DataType
    {

    }

    public class XDataType
    {
        public string name { get; set; }
        public string type { get; set; }
        public Dictionary<string, string> properties { get; set; }
        public string defaultVl { get; set; }
        public string example { get; set; }
        public string examples { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public Dictionary<string, string> facets { get; set; }
        public string xml { get; set; }
        public string[] Enum { get; set; }
    }
}
