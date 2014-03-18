using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class MoirePrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where) 
        {
            var gp = new GraphicsPath();
            //XCenter, YCenter, OutsideDiameter,CircleThickness, GapBetweenCircles,
            //NumberOfCircles, CrosshairThickness, CrosshairLength, Rotation (-=cw)
            float moireOffsetX = Parameters[0].Result();
            float moireOffsetY = Parameters[1].Result();
            float moireOutsideDiameter = Parameters[2].Result();
            float moireCircleThickness = Parameters[3].Result();
            float moireGapBetweenCircles = Parameters[4].Result();
            float moireNumberOfCircles = Parameters[5].Result();
            float moireCrosshairThickness = Parameters[6].Result();
            float moireCrosshairLength = Parameters[7].Result();
            float moireRotation = Parameters[8].Result();
            for (int moireCount = 1; moireCount <= moireNumberOfCircles; moireCount++)
            {
                gp.StartFigure();
                gp.AddEllipse(new RectangleF(where.X - moireOffsetX - (moireOutsideDiameter / 2 - ((moireCount - 1) * moireGapBetweenCircles)), where.Y - moireOffsetY - (moireOutsideDiameter / 2 - ((moireCount - 1) * moireGapBetweenCircles)), moireOutsideDiameter - ((moireCount - 1) * 2 * moireGapBetweenCircles), moireOutsideDiameter - ((moireCount - 1) * 2 * moireGapBetweenCircles)));
                gp.CloseFigure();
            }
            //Draw the crosshair
            PointF moireHLineLeft = new PointF(where.X - moireOffsetX - (moireCrosshairLength / 2), where.Y);
            PointF moireHLineRight = new PointF(where.X - moireOffsetX + (moireCrosshairLength / 2), where.Y);
            PointF moireVLineTop = new PointF(where.X, where.Y - moireOffsetY - (moireCrosshairLength / 2));
            PointF moireVLineBottom = new PointF(where.X, where.Y - moireOffsetY + (moireCrosshairLength / 2));
            gp.StartFigure();
            gp.AddLine(moireHLineLeft, moireHLineRight);
            gp.StartFigure();
            gp.AddLine(moireVLineTop, moireVLineBottom);
            Matrix moireMatrix = new Matrix();
            moireMatrix.Rotate(moireRotation);
            gp.Transform(moireMatrix);
            return gp;
        }
    }
}
