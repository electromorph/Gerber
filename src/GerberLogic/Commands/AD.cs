using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GerberLogic.Helper;
using GerberLogic.Apertures;
using GerberLogic.Apertures.Primitives.PrimitiveParameters;

namespace GerberLogic.Commands
{
    public class AD : iCommand
    {
       string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public AD (string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }

        public StandardAperture TranslateApertureType(string type)
        {
            switch (type)
            {
                case "C":
                    return new CircleAperture();
                case "R":
                    return new RectangleAperture();
                case "O":
                    return new ObroundAperture();
                case "P":
                    return new PolygonAperture();
                default:
                    return new CustomAperture(type);
            }
        }

        public void Process()
        {
            string[] commands = _commandArgs.Split(new char[] { ',', 'X' }, StringSplitOptions.RemoveEmptyEntries);
            //d<0>n<a>n
            string apertureDef = commands[0].Remove(0, 1);
            //Get the numeric part.
            int apertureNumber = int.Parse(Helpers.GetNumericPart(apertureDef));
            string apertureName = apertureDef.Replace(apertureNumber.ToString(),"");
            var aperture = TranslateApertureType(apertureName);
            if (aperture.GetType() == typeof(CustomAperture))
            {
                //Check that the name exists, and if so, clone it.
                if (_globals.ApertureMacros[apertureName] != null)
                {
                    aperture = (CustomAperture)((CustomAperture)_globals.ApertureMacros[apertureName]).Clone();
                }
            }
            
            //Assign the macro parameters
            for (int i = 1; i < commands.Length; i++)
            {
                float result = 0.0F;
                if (float.TryParse(commands[i], out result))
                {
                    aperture.Parameters.Add(new ValuePrimitiveParameter(result));
                    _image.MinimumFeatureSize = (Math.Abs(result) < _image.MinimumFeatureSize) ? Math.Abs(result) : _image.MinimumFeatureSize;
                }
            }
            aperture.Id = apertureNumber;
            
            if (!_globals.Apertures.ContainsKey(apertureNumber))
            {
                _globals.Apertures.Add(apertureNumber, aperture);
            }
        }
    }
}
