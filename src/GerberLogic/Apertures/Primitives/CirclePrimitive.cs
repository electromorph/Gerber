using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class CirclePrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where)
        {
            var gp = new GraphicsPath();
            //Exposure, Diameter, CenterX, CenterY
            if (Parameters[0].Result() == 1)
            {
                float circleCenterX = Parameters[1].Result();
                float circleCenterY = Parameters[2].Result();
                float circleDiameter = Parameters[3].Result();
                float circleRadius = circleDiameter / 2;
                RectangleF circleRect = new RectangleF(circleCenterX - circleRadius, circleCenterY - circleRadius, circleDiameter, circleDiameter);
                gp.AddEllipse(circleRect);
            }
            return gp;
        }
    }
}
