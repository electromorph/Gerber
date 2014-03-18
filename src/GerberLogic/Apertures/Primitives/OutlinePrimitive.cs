using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class OutlinePrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where)
        {
            var gp = new GraphicsPath();
            //ExposureOnOffAlter, NumberOfPoints, Point1X, Point1Y,
            //Point2X,Point2Y,...,Rotation (-=cw)
            List<PointF> outlinePointList = new List<PointF>();
            if (Parameters[0].Result() == 1)
            {
                int outlineNumberOfPoints = Convert.ToInt32(Parameters[1].Result());
                for (int outlineCount = 2; outlineCount < outlineNumberOfPoints * 2; outlineCount++)
                {
                    PointF p = new PointF();
                    p.X = Parameters[outlineCount].Result();
                    p.Y = Parameters[1].Result();
                    outlinePointList.Add(p);
                }
                float outlineRotation = Parameters[Parameters.Count - 1].Result();
                gp.AddLines(outlinePointList.ToArray());
                Matrix outlineMatrix = new Matrix();
                outlineMatrix.Rotate(outlineRotation);
                gp.Transform(outlineMatrix);
            }
            return gp;
        }
    }
}
