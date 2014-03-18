using System;
using System.Collections.Generic;
using System.Text;

namespace GerberLogic
{
    public class NumberFormat
    {
        private int _integerPart = 0;
        public int IntegerPart
        {
            get { return _integerPart; }
            set { _integerPart = value; }
        }

        private int _decimalPart = 0;
        public int DecimalPart
        {
            get { return _decimalPart; }
            set { _decimalPart = value; }
        }
	
    }
}
