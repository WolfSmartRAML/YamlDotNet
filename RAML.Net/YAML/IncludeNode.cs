using System;
using System.Collections.Generic;
using System.Text;

namespace RAML.Net.YAML
{
    public class IncludeNode
    {
        #region State

        public string File { get; set; }

        public string FilePath { get; set; }

        public string Content { get; set; }

        public string Extension { get; set; }

        #endregion

        #region CtorDtor

        public IncludeNode()
        {
            
        }

        #endregion
    }
}
