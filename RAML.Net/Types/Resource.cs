using System;
using System.Collections.Generic;
using System.Text;

namespace RAML.Net.Types
{
    public class ResourceNode
    {
        public ResourceNode()
        {
            Children = new List<ResourceNode>();
        }
        public ResourceNode Parent { get; set; }
        public List<ResourceNode> Children { get; set; }

        public Resource Resource { get; set; }
    }

    public class Resource
    {
        public Resource()
        {
            annotations = new Dictionary<string, Annotation>();
            Methods = new Dictionary<string, Method>();
            Traits = new Dictionary<string, Trait>();
            securedBy = new Dictionary<string, SecurityScheme>();
            uriParameters = new Dictionary<string, UriParameter>();
        }

        public string relativeUri { get; set; }

        public string displayName { get; set; }
        public string description { get; set; }
        public Dictionary<string, Annotation> annotations { get; set; }

        public Dictionary<string, Method> Methods { get; set; }

        public Dictionary<string, Trait> Traits { get; set; }

        public ResourceType type { get; set; }
        public Dictionary<string, SecurityScheme> securedBy { get; set; }

        public Dictionary<string, UriParameter> uriParameters { get; set; }

    }
}
