using System;
using System.Collections.Generic;
using System.Text;

namespace RAML.Net.Types
{
    public class SecurityScheme
    {
        public class Header
        {
            public string Name { get; set; }
            public string description { get; set; }
            public string type { get; set; }

            public string example { get; set; }
        }
        public class Response
        {
            public string Code { get; set; }
            public string description { get; set; }
        }

        public class DescribedBy
        {
            public Dictionary<string, Header> headers { get; set; }
            public Dictionary<string, Response> responses { get; set; }
        }

        public class Settings
        {
            public string authorizationUri { get; set; }
            public string accessTokenUri { get; set; }
            public string[] authorizationGrants { get; set; }

            public string[] scopes { get; set; }
        }

        public string type { get; set; }
        public string description { get; set; }

        public DescribedBy describedBy { get; set; }
        public Settings settings { get; set; }
    }
}
