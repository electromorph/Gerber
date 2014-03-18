using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GerberLogic.Apertures.Primitives;
using GerberLogic.Apertures;
using GerberLogic.Apertures.Primitives.PrimitiveParameters;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GerberLogic.Apertures
{
    public class CustomAperture : StandardAperture
    {
        public CustomAperture() { }
        public CustomAperture(string name) { Name = name; }
        
        #region Properties
        public string Name { get; set; }
        
        /// The collection of primitives making up the macro
        private List<Primitive> _primitives = new List<Primitive>();
        public List<Primitive> Primitives
        {
            get { return _primitives; }
            set { _primitives = value; }
        }
        #endregion

        /// <summary>
        /// Draw a Macro to the specification at a location.
        /// NB:  Aperture parameters are for the $optional parameters
        ///      Macro parameters contain the design-time params including $optional definitions
        /// NB2: The parameters are all scaled to the size/format for bitmaps here, so despite
        ///      being floats in the data model, we only deal with integers here.
        /// </summary>
        public override GraphicsPath Draw(PointF where)
        {
            var path = new GraphicsPath();
            foreach (Primitive primitive in this.Primitives)
            {
                path.AddPath(primitive.Draw(where), false);
            }
            return path;
        }
        
        public override object Clone()
        {
            CustomAperture ca = new CustomAperture(this.Name);
            ca.Parameters = this.Parameters;
            ca.Primitives = this.Primitives;
            return ca;
        }        
    }
}
