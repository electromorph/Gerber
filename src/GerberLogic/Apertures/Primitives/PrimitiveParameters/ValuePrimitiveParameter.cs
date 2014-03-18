using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Apertures.Primitives.PrimitiveParameters
{
    public class ValuePrimitiveParameter : iPrimitiveParameter
    {
        public float Value { get; set; }
        
        public float Result()
        {
            return Value;
        }

        public ValuePrimitiveParameter(float Value)
        {
            this.Value = Value;
        }
    }
}
