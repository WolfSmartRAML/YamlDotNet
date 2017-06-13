using System;
using System.Collections.Generic;
using System.Text;
using RAML.Net.YAML;

namespace RAML.Net.Types
{
    public class RequestBody
    {
        private Dictionary<string, IncludeNode> body { get; set; }
    }
}
