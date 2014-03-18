using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcellonLogic
{
    public class DrillBit
    {
        int _toolNumber = 0;
        double _diameter = 0;

        public int ToolNumber { get { return _toolNumber; } set { _toolNumber = value; } }
        public double Diameter { get { return _diameter; } set { _diameter = value; } }

        public DrillBit() { }

        public DrillBit(int ToolNumber, double Diameter)
        {
            _toolNumber = ToolNumber;
            _diameter = Diameter;
        }
    }
}
