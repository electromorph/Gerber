using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class IP : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public IP(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            switch (_commandArgs)
            {
                case "POS":
                    _image.Polarity = ImagePolarity.Positive;
                    break;
                case "NEG":
                    _image.Polarity = ImagePolarity.Negative;
                    break;
                default:
                    throw new Exception("Polarity must be expressed as 'POS' or 'NEG'");
            }
        }
    }
}
