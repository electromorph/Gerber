using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class IR : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public IR(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            switch (_commandArgs)
            {
                case "0":
                    _image.Rotation = Rotation.Deg0;
                    break;
                case "90":
                    _image.Rotation = Rotation.Deg90;
                    break;
                case "180":
                    _image.Rotation = Rotation.Deg180;
                    break;
                case "270":
                    _image.Rotation = Rotation.Deg270;
                    break;
                default:
                    throw new Exception("Image rotation can only be 0, 90, 180 or 270");
            }
        }
    }
}
