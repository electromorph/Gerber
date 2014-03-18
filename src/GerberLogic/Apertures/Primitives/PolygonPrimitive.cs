using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures.Primitives
{
    public class PolygonPrimitive : Primitive
    {
        public override GraphicsPath Draw(PointF where)
        {
            var gp = new GraphicsPath();
            //ExposureOnOffAlter, NumberOfVertices, CenterX, CenterY, Diameter, Rotation
            if (Parameters[0].Result() == 1)
            {
                int numberOfVertices = Convert.ToInt32(Parameters[1].Result());
                float polygonOffsetX = Parameters[2].Result();
                float polygonOffsetY = Parameters[3].Result();
                float polygonRadius = Parameters[4].Result() / 2;
                float polygonRotation = Parameters[5].Result();
                PointF polygonCenter = new PointF(where.X + polygonOffsetX, where.Y + polygonOffsetY);
                PointF[] polygonListPoints = MakeRegularPolygon(polygonCenter, polygonRadius, numberOfVertices, polygonRotation);
                gp.AddPolygon(polygonListPoints);
                Matrix polygonMatrix = new Matrix();
                polygonMatrix.Rotate(polygonRotation);
                gp.Transform(polygonMatrix);
            }
            return gp;
        }

        private PointF[] MakeRegularPolygon(PointF center, float radius, int nPoints, double startAngle)
        {
            PointF[] points = new PointF[nPoints];
            // we do everything in radians
            double angleStep = (2 * Math.PI / nPoints);

            double angle = startAngle;
            for (int i = 0; i < nPoints; i++)
            {
                points[i] = new PointF((float)(center.X + radius * Math.Cos(angle)), (float)(center.Y + radius * Math.Sin(angle)));
                angle += angleStep;
            }
            return points;
        }
    }
}
