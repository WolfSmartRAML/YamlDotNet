using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RAML.Net.Types
{
    public class Document
    {
        public Document()
        {
            resources = new List<ResourceNode>();
        }

        // DataTypes
        public Dictionary<string, JsonSchemaDataType> JsonDataTypes { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public string version { get; set; }
        public string baseUri { get; set; }
        //public Uri baseUri { get; set; }      // custom conv or stick with string?
        public Dictionary<string, UriParameter> baseUriParameters { get; set; }
        public string[] protocols { get; set; }
        public string[] mediaType { get; set; }
        //public string[] mediaType { get; set; } // how to manage string or string[] as legit values for this
        public Dictionary<string, UserDocumentation> documentation { get; set; }
        public Dictionary<string, DataType> types { get; set; }
        //public Dictionary<string, Trait> traits { get; set; }
        public Dictionary<string, object> traits { get; set; }
        //public ResourceType[] resourceTypes { get; set; }
        //public AnnotationType[] annotationTypes { get; set; }
        //public Dictionary<string, Annotation> annotations { get; set; }
        public Dictionary<string, SecurityScheme> securitySchemes { get; set; }
        public string[] securedBy { get; set; }
        //public Dictionary<string, Library> uses { get; set; }
        public Dictionary<string, dynamic> uses { get; set; }
        public List<ResourceNode> resources { get; set; } 
    }
}
