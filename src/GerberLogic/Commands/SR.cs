using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class SR : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public SR(string CommandArgs, Globals Globals, GerberImage Image)
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
                if (thisChar == 'X' || thisChar == 'Y' || thisChar == 'I' || thisChar == 'J' || thisChar == 'Z')
                {
                    WriteSR(thisAxis, thisNumber);
                    //Start next one.
                    thisAxis = thisChar;
                    thisNumber = "";
                }
                else if (thisChar == '.')
                {
                    thisNumber += thisChar;
                }
                else if (float.TryParse(thisNumber + thisChar, out thisDigit))
                {
                    thisNumber = thisNumber + thisDigit.ToString();
                }
            }
            WriteSR(thisAxis, thisNumber);
        }

        private void WriteSR(char thisAxis, string thisNumber)
        {
            //Write the last number.
            switch (thisAxis)
            {
                case 'X':
                    _globals.StepAndRepeat.NumberOfRepetitionsAlongX = Convert.ToInt32(thisNumber);
                    break;
                case 'Y':
                    _globals.StepAndRepeat.NumberOfRepetitionsAlongY = Convert.ToInt32(thisNumber);
                    break;
                case 'I':
                    _globals.StepAndRepeat.StepDistanceOnX = Convert.ToInt32(thisNumber);
                    break;
                case 'J':
                    _globals.StepAndRepeat.StepDistanceOnY = Convert.ToInt32(thisNumber);
                    break;
                default:
                    break;
            }
        }
    }
}
