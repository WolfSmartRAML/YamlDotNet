using System;
using System.Collections.Generic;
using System.Text;

namespace RAML.Net.Types
{
    public class Header
    {
        public string Name { get; set; }
        public string description { get; set; }
        public string type { get; set; }

        public string example { get; set; }
    }
}
