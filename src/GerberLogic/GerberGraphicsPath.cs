using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace GerberLogic
{
    public class GerberGraphicsPath
    {
        public GraphicsPath GraphicsPath { get; set; }
        public float Width { get; set; }
        public Color FillColor { get; set; }
        public Color LineColor { get; set; }
        public Brush Brush { get; set; }

        public GerberGraphicsPath(GraphicsPath GraphicsPath, float Width, bool LineColorBlack)
        {
            this.Width = Width;
            this.GraphicsPath = GraphicsPath;
            this.LineColor = LineColorBlack ? Color.White : Color.Black;
            this.FillColor = LineColorBlack ? Color.White : Color.Black;
        }
    }
}
