using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class OF : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public OF(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            char[] commandArgsArray = (_commandArgs + "Z").ToCharArray();
            char thisChar = ' ', thisAxis = 'Z'; string thisNumber = ""; float thisDigit = 0;
            for (int iCount = 0; iCount < commandArgsArray.Length; iCount++)
            {
                //Get current character
                thisChar = commandArgsArray[iCount];
                if (thisChar == 'A' || thisChar == 'B' || thisChar == 'Z')
                {
                    if (thisAxis == 'A' && (thisNumber.Length > 0))
                    {
                        _globals.Offset.X = Convert.ToSingle(thisNumber);
                        thisNumber = "";
                        thisAxis = thisChar;
                    }
                    if ((thisAxis == 'B') && (thisNumber.Length > 0))
                    {
                        _globals.Offset.Y = Convert.ToSingle(thisNumber);
                        thisNumber = "";
                        thisAxis = thisChar;
                    }
                    if (thisChar != 'Z')
                    {
                        thisAxis = thisChar;
                    }
                }
                else if (float.TryParse(thisNumber + thisChar, out thisDigit))
                {
                    thisNumber = thisNumber + thisDigit.ToString();
                }
            }
        }
    }
}
