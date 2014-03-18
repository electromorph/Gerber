using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using GerberLogic.Apertures;

namespace GerberLogic.DrawingElements
{
    public class Line : DrawingElement
    {
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        public bool LastComponentPolygonFill { get; set; }

        public Line(PointF StartPoint, PointF EndPoint, StandardAperture ApertureUsed, bool PolygonAreaFill)
        {
            this.StartPoint = StartPoint;
            this.EndPoint = EndPoint;
            this.ApertureUsed = ApertureUsed;
            this.PolygonAreaFill = PolygonAreaFill;
        }

        public override GraphicsPath Draw()
        {
            GraphicsPath path = new GraphicsPath();
            GraphicsPath pathLine = new GraphicsPath();
            GraphicsPath ggp = this.ApertureUsed.Draw(this.StartPoint);
            pathLine.AddLine(this.StartPoint.X, this.StartPoint.Y, this.EndPoint.X, this.EndPoint.Y);
            GraphicsPath ggp2 = this.ApertureUsed.Draw(this.EndPoint);
            if (!PolygonAreaFill)
            {
                path.AddPath(pathLine, false);
            }
            else
            {
                path.AddPath(pathLine, true);
            }
            return path;
        }
    }
}
