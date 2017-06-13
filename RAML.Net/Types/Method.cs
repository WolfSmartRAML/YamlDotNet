using System;
using System.Collections.Generic;
using System.Text;
using RAML.Net.YAML;

namespace RAML.Net.Types
{

    public class Method
    {
        public class SecuredBy
        {
            public string Name { get; set; }
            public string[] scopes { get; set; }
        }

        public string Verb { get; set; }

        public string displayName { get; set; }
        public Dictionary<string, Annotation> annotations { get; set; }

        public Dictionary<string, UriParameter> queryParameters { get; set; }
        public string queryString { get; set; }
        public List<Header> headers { get; set; }
        public Dictionary<string, Response> responses { get; set; }
        //public RequestBody body { get; set; }
        public Dictionary<string, IncludeNode> body { get; set; }

        public List<string> protocols { get; set; }
        public Dictionary<string, Trait> traits { get; set; }

        //public List<SecuredBy> securedBy { get; set; }

    }
}
