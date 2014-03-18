using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class RectangleLowerLeftPrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where)
        {
            var gp = new GraphicsPath();
            //ExposureOnOffAlter, Width, Height, LowerLeftX, LowerLeftY, Rotation
            if (Parameters[0].Result() == 1)
            {
                float rectanglelowerleftWidth = Parameters[1].Result();
                float rectanglelowerleftHeight = Parameters[2].Result();
                float rectanglelowerleftOffsetX = Parameters[3].Result();
                float rectanglelowerleftOffsetY = Parameters[4].Result();
                float rectanglelowerleftRotation = Parameters[5].Result();
                PointF rectanglelowerleftTopLeftPoint = new PointF(where.X + rectanglelowerleftOffsetX - (rectanglelowerleftWidth), where.Y + rectanglelowerleftOffsetY - (rectanglelowerleftHeight));
                SizeF rectanglelowerleftSize = new SizeF(rectanglelowerleftWidth, rectanglelowerleftHeight);
                RectangleF rectanglelowerleft = new RectangleF(rectanglelowerleftTopLeftPoint, rectanglelowerleftSize);
                gp.AddRectangle(rectanglelowerleft);
                Matrix rectanglelowerleftMatrix = new Matrix();
                rectanglelowerleftMatrix.Rotate(rectanglelowerleftRotation);
                gp.Transform(rectanglelowerleftMatrix);
            }
            return gp;
        }
    }
}
