using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    /// <summary>
    /// Set linear scale factor (0.1)
    /// </summary>
    public class G11 : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;
                
        public G11(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            _globals.LinearScaleFactor = 0.1F;
        }
    }
}
