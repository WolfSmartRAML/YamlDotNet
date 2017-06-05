using System;
using System.Collections.Generic;
using System.Text;

namespace RAML.Net.Types
{
    public class DataType
    {
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
