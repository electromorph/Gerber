using System;
using System.Collections.Generic;
using System.Text;
using GerberLogic.Apertures;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.DrawingElements
{
    public abstract class DrawingElement
    {
        public StandardAperture ApertureUsed { get; set; }
        public bool PolygonAreaFill { get; set; }
        public int Id { get; set; }
        public abstract GraphicsPath Draw();
    }
}
