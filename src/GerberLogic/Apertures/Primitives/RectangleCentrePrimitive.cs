using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class RectangleCentrePrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where)
        {
            var gp = new GraphicsPath();
            //ExposureOnOffAlter, Width, Height, CenterX, CenterY, Rotation
            if (Parameters[0].Result() == 1)
            {
                float rectanglecenterWidth = Parameters[1].Result();
                float rectanglecenterHeight = Parameters[2].Result();
                float rectanglecenterOffsetX = Parameters[3].Result();
                float rectanglecenterOffsetY = Parameters[4].Result();
                float rectanglecenterRotation = Parameters[5].Result();
                PointF rectanglecenterTopLeftPoint = new PointF(where.X + rectanglecenterOffsetX - (rectanglecenterWidth / 2), where.Y + rectanglecenterOffsetY - (rectanglecenterHeight / 2));
                SizeF rectanglecenterSize = new SizeF(rectanglecenterWidth, rectanglecenterHeight);
                RectangleF rectangleCenter = new RectangleF(rectanglecenterTopLeftPoint, rectanglecenterSize);
                gp.AddRectangle(rectangleCenter);
                Matrix polygonMatrix = new Matrix();
                polygonMatrix.Rotate(rectanglecenterRotation);
                gp.Transform(polygonMatrix);
            }
            return gp;
        }
    }
}
