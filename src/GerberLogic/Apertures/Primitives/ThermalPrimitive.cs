using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class ThermalPrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where)
        {
            var gp = new GraphicsPath();
            //XCenter, YCenter, OutsideDiameter, InsideDiameter, CrossHairThickness, Rotation
            float thermalOffsetX = Parameters[0].Result();
            float thermalOffsetY = Parameters[1].Result();
            float thermalOutsideDiameter = Parameters[2].Result();
            float thermalInsideDiameter = Parameters[3].Result();
            float thermalCrosshairThickness = Parameters[4].Result();
            float thermalRotation = Parameters[5].Result();
            RectangleF thermalOutsideDiameterRectangle = new RectangleF(thermalOffsetX - (thermalOutsideDiameter / 2), thermalOffsetY - (thermalOutsideDiameter / 2), thermalOutsideDiameter, thermalOutsideDiameter);
            RectangleF thermalInsideDiameterRectangle = new RectangleF(thermalOffsetX - (thermalInsideDiameter / 2), thermalOffsetY - (thermalInsideDiameter / 2), thermalInsideDiameter, thermalInsideDiameter);
            gp.StartFigure();
            gp.AddArc(thermalOutsideDiameterRectangle, 20, 60);
            gp.AddArc(thermalInsideDiameterRectangle, 80, -60);
            gp.CloseFigure();
            gp.AddArc(thermalOutsideDiameterRectangle, 110, 60);
            gp.AddArc(thermalInsideDiameterRectangle, 170, -60);
            gp.CloseFigure();
            gp.AddArc(thermalOutsideDiameterRectangle, 200, 60);
            gp.AddArc(thermalInsideDiameterRectangle, 260, -60);
            gp.CloseFigure();
            gp.AddArc(thermalOutsideDiameterRectangle, 290, 60);
            gp.AddArc(thermalInsideDiameterRectangle, 350, -60);
            gp.CloseFigure();
            Matrix thermalMatrix = new Matrix();
            thermalMatrix.Rotate(thermalRotation);
            gp.Transform(thermalMatrix);
            thermalMatrix = new Matrix();
            thermalMatrix.Translate(where.X, where.Y);
            gp.Transform(thermalMatrix);
            return gp;
        }
    }
}
