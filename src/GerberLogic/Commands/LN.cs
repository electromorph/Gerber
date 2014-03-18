using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class LN : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public LN(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            _image.Layers.Add(new Layer(_commandArgs));
        }
    }
}
