using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class IN : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public IN(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            _image.Name = _commandArgs;
        }
    }
}
