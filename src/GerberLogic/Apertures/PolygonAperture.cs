using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures
{
    public class PolygonAperture : StandardAperture
    {
        public override GraphicsPath Draw(PointF where)
        {
            GraphicsPath path = new GraphicsPath();
            float innerRectTopX = 0;
            float innerRectTopY = 0;
            float innerRectHeight = 0;
            float innerRectWidth = 0;
            float holeX = 0F;
            float holeY = 0F;
            RectangleF innerRectangle;
            PointF[] outerPoints = new PointF[] { };
            if (this.Parameters.Count == 2)
            {
                //Polygon.
                outerPoints = MakeRegularPolygon(new PointF(where.X, where.Y), this.Parameters[0].Result() / 2, Convert.ToInt32(this.Parameters[1].Result()), 0D);
            }
            if (this.Parameters.Count >= 3)
            {
                //Polygon with start angle
                outerPoints = MakeRegularPolygon(new PointF(where.X, where.Y), this.Parameters[0].Result() / 2, Convert.ToInt32(this.Parameters[1].Result()), Convert.ToDouble(this.Parameters[2].Result()));
            }
            path.AddPolygon(outerPoints);
            if (this.Parameters.Count > 3)
            {
                if (this.Parameters.Count > 2)
                {
                    holeX = this.Parameters[3].Result();
                }
                if (this.Parameters.Count > 3)
                {
                    holeY = this.Parameters[4].Result();
                }
                else
                {
                    holeY = holeX;
                }
                innerRectTopX = where.X - holeX / 2;
                innerRectTopY = where.Y - holeY / 2;
                innerRectHeight = holeY;
                innerRectWidth = holeX;
                innerRectangle = new RectangleF(innerRectTopX, innerRectTopY, innerRectWidth, innerRectHeight);
                if (this.Parameters.Count == 4)
                {
                    //Add inner circle
                    path.AddEllipse(innerRectangle);
                }
                else if (this.Parameters.Count == 5)
                {
                    //Add inner rectangle
                    path.AddRectangle(innerRectangle);
                }
            }
            return path;
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
