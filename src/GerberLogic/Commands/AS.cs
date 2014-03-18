using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class AS : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public AS (string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        } 

        public void Process()
        {
            if (_commandArgs == "AXBY")
            {
                _globals.Axes = AxisSelect.AxBy;
            }
            else
            {
                _globals.Axes = AxisSelect.AyBx;
            }
        }
    }
}
