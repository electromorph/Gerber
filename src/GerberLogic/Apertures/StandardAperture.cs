using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using GerberLogic.Apertures.Primitives.PrimitiveParameters;

namespace GerberLogic.Apertures
{
    public abstract class StandardAperture : ICloneable
    {
        private List<iPrimitiveParameter> _parameters = new List<iPrimitiveParameter>();
        public List<iPrimitiveParameter> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
        
        public int Id { get; set; }

        public virtual Object Clone()
        {
            return this.Clone();
        }

        public abstract GraphicsPath Draw(PointF where);

        private float _width = 0F;
        public float Width
        {
            get
            {
                if (_width == 0)
                {
                    _width = (this.Draw(new PointF(0, 0))).GetBounds().Width;
                }
                return _width;
            }
        }

        private float _height = 0F;
        public float Height
        {
            get
            {
                if (_height == 0)
                {
                    _height = (this.Draw(new PointF(0, 0))).GetBounds().Width;
                }
                return _height;
            }
        }

        public float AverageApertureWidth()
        {
            return Convert.ToSingle((_width + _height) / 2);
        }

        public float ApertureWidthSeenFromAnAngle(float AngleToX)
        {
            return Convert.ToSingle(Math.Sin(AngleToX) * _height + Math.Cos(AngleToX) * _width);
        }
    }
}
