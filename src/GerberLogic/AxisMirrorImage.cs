using System;
using System.Collections.Generic;
using System.Text;

namespace GerberLogic
{
    public class AxisMirrorImage
    {
        private bool _a = false;
        public bool A
        {
            get { return _a; }
            set { _a = value; } 
        }
        
        private bool _b = false;
        public bool B
        { 
            get { return _b; }
            set { _b = value; } 
        }
    }
}
