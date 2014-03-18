using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using GerberLogic.Apertures;

namespace GerberLogic.DrawingElements
{
    class Circle : DrawingElement
    {
        public bool IsClockwise { get; set; }
        public bool IsSingleQuadrant  { get; set; }
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        public PointF Center { get; set; }
        
        public Circle(PointF startPoint, PointF endPoint, PointF centre, bool isSingleQuadrant, bool isClockwise, StandardAperture apertureUsed, bool polygonAreaFill)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
            Center = centre;
            IsSingleQuadrant = isSingleQuadrant;
            IsClockwise = isClockwise;
            if (apertureUsed == null)
            {
                throw new Exception("Must specify an aperture");
            }
            ApertureUsed = apertureUsed;
            PolygonAreaFill = polygonAreaFill;
        }

        public override GraphicsPath Draw()
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            PointF distance = new PointF(Center.X - StartPoint.X, Center.Y - StartPoint.Y);
            float radius = Convert.ToSingle(Math.Sqrt(Math.Pow(distance.X, 2) + Math.Pow(distance.Y, 2)));
            PointF circleTopLeft = new PointF(Center.X - radius, Center.Y - radius);
            SizeF circleSize = new SizeF(radius * 2, radius * 2);
            float startAngle = GetAngleToXAxis(Center, StartPoint, radius);
            float endAngle = GetAngleToXAxis(Center, EndPoint, radius);
            float sweepAngle = endAngle - startAngle;
            RectangleF circleBox = new RectangleF(circleTopLeft, circleSize);
            if (StartPoint == EndPoint)
            {
                path.AddEllipse(circleBox);
            }
            else
            {
                path.AddArc(circleBox, startAngle, sweepAngle);
            }
            return path;
        }

        public float GetAngleToXAxis(PointF CircleCenter, PointF PointOnCircle, float Radius)
        {
            float vectorX = PointOnCircle.X - CircleCenter.X;
            float vectorY = PointOnCircle.Y - CircleCenter.Y;
            double angle = Math.Asin(vectorY / Radius);
            return Convert.ToSingle((360D / (2 * Math.PI)) * ((vectorX < 0) ? (Math.PI + ((vectorY < 0) ? -angle : angle)) : angle));
        }
    }
}
