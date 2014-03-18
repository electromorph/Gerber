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
    /// Move slowly in a straight line
    /// </summary>
    public class G1 : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;

        public G1(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            DrawingElement cmp = null;
            PointF endPoint = new PointF(
                Helpers.ApplyDecimalFormat(_globals, "X", _commandArgs["X"]),
                Helpers.ApplyDecimalFormat(_globals, "Y", _commandArgs["Y"])
                                        );
            if ((_globals.Exposure == ExposureMode.On) && (_globals.LastPoint != endPoint))
            {
                    cmp = new Line(_globals.LastPoint, endPoint, _globals.LastStandardAperture, _globals.PolygonAreaFill);
                    _image.Layers[(_image.Layers.Count - 1)].Components.Add(cmp);
            }
            _globals.LastPoint = endPoint;
            _globals.LastCommand = "G1";
        }
    }
}
