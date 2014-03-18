using System;
using System.Collections.Generic;
using System.Text;

namespace GerberLogic
{
    public class AxisJustification
    {
        private Justification _justification = Justification.None;
        public Justification Justification
        {
            get { return _justification; }
            set { _justification = value; }
        }

        private float _amount = 0;
        public float Amount 
        { 
            get {return _amount;}
            set {_amount = value;}
        }
    }
}
