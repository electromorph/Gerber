using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures
{
    public class ObroundAperture : StandardAperture
    {
        public override GraphicsPath Draw(PointF where)
        {
            GraphicsPath path = new GraphicsPath();
            float innerRectTopX = 0;
            float innerRectTopY = 0;
            float innerRectHeight = 0;
            float innerRectWidth = 0;
            float outerRectTopX = 0;
            float outerRectTopY = 0;
            float outerRectWidth = 0;
            float outerRectHeight = 0;
            float holeX = 0F;
            float holeY = 0F;
            RectangleF outerRectangle;
            RectangleF innerRectangle;
            //Draw a circle of radius aperture.
            outerRectWidth = this.Parameters[0].Result();
            outerRectHeight = this.Parameters[1].Result();

            if (this.Parameters.Count > 2)
            {
                holeX = this.Parameters[2].Result();
            }
            if (this.Parameters.Count > 3)
            {
                holeY = this.Parameters[3].Result();
            }
            else
            {
                holeY = holeX;
            }
            innerRectTopX = where.X - holeX / 2;
            innerRectTopY = where.Y - holeY / 2;
            innerRectHeight = holeY;
            innerRectWidth = holeX;
            outerRectTopX = where.X - outerRectWidth / 2;
            outerRectTopY = where.Y - outerRectHeight / 2;
            outerRectangle = new RectangleF(new PointF(outerRectTopX, outerRectTopY), new SizeF(outerRectWidth, outerRectHeight));
            path.AddEllipse(outerRectangle);
            //Define inner rectangle
            innerRectangle = new RectangleF(innerRectTopX, innerRectTopY, innerRectWidth, innerRectHeight);
            if (this.Parameters.Count == 3)
            {
                //Add inner circle
                path.AddEllipse(innerRectangle);
            }
            else if (this.Parameters.Count == 4)
            {
                //Add inner rectangle
                path.AddRectangle(innerRectangle);
            }
            return path;
        }
    }
}
