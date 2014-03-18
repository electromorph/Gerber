using System;
using System.Collections.Generic;
using System.Text;

namespace gerber
{
    enum Justification
	{
	    Left,
        Center
	}

    enum PolarityValue
    {
        Positive,
        Negative
    }

    enum RotationAmount
    {
        Deg0,
        Deg90,
        Deg180,
        Deg270
    }
    
    class Image
    {
        private Justification _justifyAAxis;
        public Justification JustifyAAxis
        {
            get { return _justifyAAxis; }
            set { _justifyAAxis = value; }
        }

        private Justification _justifyBAxis;
        public Justification JustifyBAxis
        {
            get { return _justifyBAxis; }
            set { _justifyBAxis = value; }
        }

        private Offset _imageOffset;
        public Offset ImageOffset
        {
            get { return _imageOffset; }
            set { _imageOffset = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private PolarityValue _polarity;
        public PolarityValue Polarity
        {
            get { return _polarity; }
            set { _polarity = value; }
        }

        private RotationAmount _rotation;
        public RotationAmount Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        private List<Layer> _layers;
        public List<Layer> Layers
        {
            get { return _layers; }
            set { _layers = value; }
        }

        private List<Knockout> _knockouts;
        public List<Knockout> Knockouts
        {
            get { return _knockouts; }
            set { _knockouts = value; }
        }
	
    }
}
