using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GerberLogic.Helper;
using GerberLogic.Apertures;

namespace GerberLogic.Commands
{
    public class D : iCommand
    {
        Dictionary<string, float> _commandArgs;
        Globals _globals;
        GerberImage _image;

        public D(Dictionary<string, float> CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            //select an aperture
            _globals.CurrentAperture = ApertureFactory.GetApertureFromId(Convert.ToInt32(_commandArgs["D"]), _globals.Apertures);
            if ((_globals.CurrentAperture != null) && (_globals.CurrentAperture.GetType() != typeof(CustomAperture)))
            {
                _globals.LastStandardAperture = _globals.CurrentAperture;
            }
            _globals.CurrentAperture = _globals.CurrentAperture ?? _globals.LastStandardAperture;
        }
    }
}
