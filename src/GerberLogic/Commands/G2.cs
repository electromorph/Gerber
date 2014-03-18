using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GerberLogic.Helper;
using GerberLogic.DrawingElements;

namespace GerberLogic.Commands
{
    /// <summary>
    /// Draw a circle
    /// </summary>
    public class G2 : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;

        public G2(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }

        public void Process()
        {
            DrawingElement cmp = null;
            //Clockwise Circular Interpolation
            PointF endPoint = new PointF(
                Helpers.ApplyDecimalFormat(_globals, "X", _commandArgs["X"]),
                Helpers.ApplyDecimalFormat(_globals, "Y", _commandArgs["Y"])
                );
            PointF centrePoint = new PointF(
                Helpers.ApplyDecimalFormat(_globals, "I", _commandArgs["I"]),
                Helpers.ApplyDecimalFormat(_globals, "J", _commandArgs["J"])
                );
            centrePoint.X = centrePoint.X + _globals.LastPoint.X;
            centrePoint.Y = centrePoint.Y + _globals.LastPoint.Y;
            if (_globals.Exposure == ExposureMode.On)
            {
                cmp = new Circle(_globals.LastPoint, endPoint, centrePoint, _globals.CirclesAreSingleQuadrant, true, _globals.LastStandardAperture, _globals.PolygonAreaFill);
            }
            _globals.LastPoint = endPoint;
            if (cmp != null)
            {
                _image.Layers[(_image.Layers.Count - 1)].Components.Add(cmp);
            }
            _globals.LastCommand = "G2";
        }
    }
}
