using System;
using System.Collections.Generic;
using System.Text;
using GerberLogic.DrawingElements;

namespace GerberLogic
{
    public class Layer
    {
        private bool _isDefaultLayer = false;
        public bool IsDefaultLayer
        { 
            get { return _isDefaultLayer; }
            set { _isDefaultLayer = value; }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private LayerPolarity _polarity = LayerPolarity.Clear;
        public LayerPolarity Polarity
        {
            get { return _polarity; }
            set { _polarity = value; }
        }

        private List<DrawingElement> _components = new List<DrawingElement>();
        public List<DrawingElement> Components
        {
            get { return _components; }
            set { _components = value; }
        }

        public Layer()
        {
        }

        public Layer(string LayerName)
        {
            Name = LayerName;
        }
    }
}
