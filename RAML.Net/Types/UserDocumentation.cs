﻿using System;
using System.Collections.Generic;
using System.Text;
using RAML.Net.YAML;

namespace RAML.Net.Types
{
    public class UserDocumentation
    {
        public string title { get; set; }
        //public string content { get; set; }
        public IncludeNode content { get; set; }
    }
}