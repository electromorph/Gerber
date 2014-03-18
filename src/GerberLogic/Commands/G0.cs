using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GerberLogic.Helper;
using System.Drawing;

namespace GerberLogic.Commands
{
    /// <summary>
    /// Move Quickly in a straight line
    /// </summary>
    public class G0 : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;

        public G0(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }

        public void Process()
        {
            _globals.LastPoint = new PointF(
                                            Helpers.ApplyDecimalFormat(_globals, "X", _commandArgs["X"]),
                                            Helpers.ApplyDecimalFormat(_globals, "Y", _commandArgs["Y"])       
                                            );
            _globals.LastCommand = "G0";
        }
    }
}
