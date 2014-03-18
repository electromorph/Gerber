using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GerberLogic.Apertures;
using System.Drawing.Drawing2D;

namespace GerberLogic.DrawingElements
{
    public class Flash : DrawingElement
    {
        public PointF StartPoint { get; set; }

        public Flash(PointF StartPoint, StandardAperture aperture)
        {
            this.StartPoint = StartPoint;
            this.ApertureUsed = aperture;
        }

        public override GraphicsPath Draw()
        {
            return ApertureUsed.Draw(StartPoint);
        }
    }
}
