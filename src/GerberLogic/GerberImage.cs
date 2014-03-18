using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic
{
    public enum Justification
	{
	    None,
        Left,
        Center
	}

    //Polarity of image
    public enum ImagePolarity
    {
        Positive,
        Negative
    }

    //Layer polarity - clear or dark
    public enum LayerPolarity
    {
        Clear,
        Dark
    }

    public enum Rotation
    {
        Deg0,
        Deg90,
        Deg180,
        Deg270
    }

    public class GerberImage
    {
        private PointF _topLeftCorner;
        public PointF TopLeftCorner
        {
            get { return _topLeftCorner; }
            set { _topLeftCorner = value; }
        }
        
        private PointF _bottomRightCorner;
        public PointF BottomRightCorner
        {
            get { return _bottomRightCorner; }
            set { _bottomRightCorner = value; }
        }

        private float _minimumFeatureSize = 1;
        public float MinimumFeatureSize
        {
            get { return _minimumFeatureSize; }
            set { _minimumFeatureSize = value; }
        }

        private AxisJustification _justifyAAxis = new AxisJustification();
        public AxisJustification JustifyAAxis
        {
            get { return _justifyAAxis; }
            set { _justifyAAxis = value; }
        }

        private AxisJustification _justifyBAxis = new AxisJustification();
        public AxisJustification JustifyBAxis
        {
            get { return _justifyBAxis; }
            set { _justifyBAxis = value; }
        }

        private Offset _Offset = new Offset();
        public Offset Offset
        {
            get { return _Offset; }
            set { _Offset = value; }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private ImagePolarity _polarity = ImagePolarity.Negative;
        public ImagePolarity Polarity
        {
            get { return _polarity; }
            set { _polarity = value; }
        }

        private Rotation _rotation = Rotation.Deg0;
        public Rotation Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        private List<Layer> _layers = new List<Layer>();
        public List<Layer> Layers
        {
            get { return _layers; }
            set { _layers = value; }
        }

        private List<Knockout> _knockouts = new List<Knockout>();
        public List<Knockout> Knockouts
        {
            get { return _knockouts; }
            set { _knockouts = value; }
        }

        public GerberImage()
        {
            //Make sure there is at least one layer.
            this.Layers.Add(new Layer("Default"));
            this.Layers[0].IsDefaultLayer = true;
        }

        List<GerberGraphicsPath> _imageGraphicsPaths = new List<GerberGraphicsPath>();
        public List<GerberGraphicsPath> ImageGraphicsPaths
        {
            get { return _imageGraphicsPaths; }
            set { _imageGraphicsPaths = value; }
        }

        public void CheckIfPathIncreasesImageSize(GraphicsPath componentPath)
        {
            float lowestXCoordOnThisPath = componentPath.GetBounds().Left;
            float lowestYCoordOnThisPath = componentPath.GetBounds().Top;
            float highestXCoordOnThisPath = componentPath.GetBounds().Right;
            float highestYCoordOnThisPath = componentPath.GetBounds().Bottom;
            _topLeftCorner.X = (lowestXCoordOnThisPath < _topLeftCorner.X) ? lowestXCoordOnThisPath : _topLeftCorner.X;
            _topLeftCorner.Y = (lowestYCoordOnThisPath < _topLeftCorner.Y) ? lowestYCoordOnThisPath : _topLeftCorner.Y;
            _bottomRightCorner.X = (highestXCoordOnThisPath > _bottomRightCorner.X) ? highestXCoordOnThisPath : _bottomRightCorner.X;
            _bottomRightCorner.Y = (highestYCoordOnThisPath > _bottomRightCorner.Y) ? highestYCoordOnThisPath : _bottomRightCorner.Y;
        }
    }
}
