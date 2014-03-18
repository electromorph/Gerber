using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    /// <summary>
    /// Make circles multi-quadrant
    /// </summary>
    public class G75 : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;

        public G75(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }

        public void Process()
        {
            _globals.CirclesAreSingleQuadrant = false;
        }
    }
}
