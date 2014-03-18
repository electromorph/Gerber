using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GerberLogic
{
    public class Knockout
    {
        private LayerPolarity _polarity;
        public LayerPolarity Polarity
        {
            get { return _polarity; }
            set { _polarity = value; }
        }

        private PointF _startFrom;
        public PointF StartFrom  
        {
            get { return _startFrom; }
            set { _startFrom = value; }
        }

        private float _width;
        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private float _height;
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }

        private float _borderWidth;
        public float BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }
	
    }
}
