using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class LP : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public LP(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            switch (_commandArgs)
            {
                case "C":
                    _image.Layers[_image.Layers.Count - 1].Polarity = LayerPolarity.Clear;
                    break;
                case "D":
                    _image.Layers[_image.Layers.Count - 1].Polarity = LayerPolarity.Dark;
                    break;
                default:
                    throw new Exception("Polarity must be expressed as 'POS' or 'NEG'");
            }
        }
    }
}
