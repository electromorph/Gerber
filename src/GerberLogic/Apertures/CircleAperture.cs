using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures
{
    public class CircleAperture : StandardAperture
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
            RectangleF outerRectangle;
            RectangleF innerRectangle;
            //Draw a circle of radius aperture.
            float outerRadius = this.Parameters[0].Result() / 2;
            if (this.Parameters.Count > 1)
            {
                holeX = this.Parameters[1].Result();
            }
            if (this.Parameters.Count > 2)
            {
                holeY = this.Parameters[2].Result();
            }
            innerRectTopX = where.X - holeX;
            if (this.Parameters.Count > 2)
            {
                innerRectTopY = where.Y - holeY;
                innerRectHeight = 2 * holeY;
            }
            else
            {
                innerRectTopY = where.Y - holeX;
                innerRectHeight = 2 * holeX;
            }
            innerRectWidth = 2 * holeX;
            outerRectangle = new RectangleF(new PointF(where.X - outerRadius, where.Y - outerRadius), new SizeF(outerRadius * 2, outerRadius * 2));
            path.AddEllipse(outerRectangle);
            //Define inner rectangle
            innerRectangle = new RectangleF(innerRectTopX, innerRectTopY, innerRectWidth, innerRectHeight);
            if (this.Parameters.Count == 2)
            {
                //Add inner circle
                path.AddEllipse(innerRectangle);
            }
            else if (this.Parameters.Count == 3)
            {
                //Add inner rectangle
                path.AddRectangle(innerRectangle);
            }
            return path;
        }
    }
}
