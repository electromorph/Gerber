using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class MO : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public MO(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        public void Process()
        {
            if (_commandArgs == "IN")
            {
                _globals.Units = Units.Inches;
            }
            else
            {
                _globals.Units = Units.mm;
            }
        }
    }
}
