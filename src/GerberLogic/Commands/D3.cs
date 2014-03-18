using System;
using System.Collections.Generic;
using System.Drawing;
using GerberLogic.DrawingElements;
using GerberLogic.Helper;

namespace GerberLogic.Commands
{
    public class D3 : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;

        public D3 (Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }

        public void Process()
        {

            DrawingElement cmpFlash = new Flash(new PointF(
                Helpers.ApplyDecimalFormat(_globals, "X", _commandArgs["X"]),
                Helpers.ApplyDecimalFormat(_globals, "Y", _commandArgs["Y"])),
                _globals.CurrentAperture);
            _image.Layers[(_image.Layers.Count - 1)].Components.Add(cmpFlash);
            _globals.LastCommand = "D3";
        }
    }
}
