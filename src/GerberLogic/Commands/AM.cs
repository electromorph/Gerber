using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GerberLogic.Apertures;
using GerberLogic.Apertures.Primitives;
using GerberLogic.Apertures.Primitives.PrimitiveParameters;
using System.Reflection;

namespace GerberLogic.Commands
{
    public class AM : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public AM (string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }

        private Primitive GetPrimitive(int primitiveNumber)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            //For some stupid reason, LinePrimitive can be 2 *OR* 20. We only want to deal with one class, so if it's 20 then make it 2.
            primitiveNumber = (primitiveNumber == 20) ? 2 : primitiveNumber;
            string primitiveType = ((PrimitiveTypes)primitiveNumber).ToString();
            var o = (Primitive)a.CreateInstance("GerberLogic.Apertures.Primitives." + primitiveType, true, BindingFlags.InvokeMethod, null, new object[] { }, null, null);
            return o;
        }

        public void Process()
        {
            List<string> macroParts = _commandArgs.Split(new char[] {'*'}, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            //2 dimensional List-array containing parameter 
            List<List<string>> primitiveDefinitions = new List<List<string>>();
            CustomAperture macro;
            if (macroParts.Count >= 2)
            {
                macro = new CustomAperture(macroParts[0]);
                macroParts.Remove(macro.Name);
                foreach (string primitiveDefinitionString in macroParts)
                    primitiveDefinitions.Add(primitiveDefinitionString.Split(',').ToList<string>());
            }
            else
            {
                return;
            }
            int number = 0;
            float floatNumber = 0F;
            foreach (List<string> primitiveDefinition in primitiveDefinitions)
            {
                if (int.TryParse(primitiveDefinition[0], out number))
                {
                    //The first value in a primitive definition is always the primitive type.  Use it...
                    Primitive primitive = GetPrimitive(number);
                    //...and lose it.
                    primitiveDefinition.RemoveAt(0);
                    //The other values are parameters
                    foreach (string primitiveParameter in primitiveDefinition)
                    {
                        if (float.TryParse(primitiveParameter, out floatNumber))
                        {
                            //The number conversion was successful.  Put as a numeric value.
                            primitive.Parameters.Add(new ValuePrimitiveParameter(floatNumber));
                        }
                        else
                        {
                            //It is a passed-in parameter of some kind.
                            primitive.Parameters.Add(new CalculatedPrimitiveParameter(primitiveParameter));
                        }
                    }
                macro.Primitives.Add(primitive);
                }
            }
            _globals.ApertureMacros.Add(macro.Name, macro);
        }
    }
}
