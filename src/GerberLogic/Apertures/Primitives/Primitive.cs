using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using GerberLogic.Apertures.Primitives.PrimitiveParameters;

namespace GerberLogic.Apertures.Primitives
{
    public enum PrimitiveTypes
    {
        CirclePrimitive = 1,
        LinePrimitive = 2,   //Line can be 2 or 20, hence the two enums. They are the same.
        OutlinePrimitive = 4,
        PolygonPrimitive = 5,
        MoirePrimitive = 6,
        ThermalPrimitive = 7,
        RectangleCenterPrimitive = 21,
        RectangleLowerLeftPrimitive = 22
    }

    public abstract class Primitive
    {
        private List<iPrimitiveParameter> _parameters = new List<iPrimitiveParameter>();
        public List<iPrimitiveParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        public abstract GraphicsPath Draw(PointF center);
    }
}
