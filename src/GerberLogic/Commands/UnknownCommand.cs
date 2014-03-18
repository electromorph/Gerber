using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class UnknownCommand : iCommand
    {
        Dictionary<string, float> _commandArgs;
        string _commandCode = string.Empty;
        Globals _globals;
        GerberImage _image;

        public UnknownCommand(string CommandCode, Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
            _commandCode = CommandCode;
        }

        public void Process()
        {
            
        }
    }
}
