using System;
using System.Collections.Generic;
using System.Text;

namespace GerberLogic
{
    public class CommandNumberFormat
    {
        private NumberFormat _x = new NumberFormat();
        public NumberFormat X
        {
            get { return _x; }
            set { _x = value; }
        }

        private NumberFormat _y = new NumberFormat();
        public NumberFormat Y
        {
            get { return _y; }
            set { _y = value; }
        }
    }
}
