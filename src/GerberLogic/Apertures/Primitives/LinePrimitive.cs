using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class LinePrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where)
        {
            var gp = new GraphicsPath();
            //Exposure, width, startX, startY, endX, endY, rotation (- = clockwise)
            if (Parameters[0].Result() == 1)
            {
                float linewidth = Parameters[1].Result();
                float lineStartX = Parameters[2].Result();
                float lineStartY = Parameters[3].Result();
                float lineEndX = Parameters[4].Result();
                float lineEndY = Parameters[5].Result();
                float lineRotation = Parameters[6].Result();
                gp.AddLine(new PointF(where.X + lineStartX, where.Y + lineStartY), new PointF(where.X + lineEndX, where.Y + lineEndY));
                Matrix lineMatrix = new Matrix();
                lineMatrix.Rotate(lineRotation);
                gp.Transform(lineMatrix);
            }
            return gp;
        }
    }
}
