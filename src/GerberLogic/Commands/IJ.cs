using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class IJ : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public IJ(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            char[] commandArgsArray = (_commandArgs + "Z").ToCharArray();
            char thisChar = ' ', thisAxis = ' '; string thisNumber = ""; float thisDigit = 0;
            for (int iCount = 0; iCount < commandArgsArray.Length; iCount++)
            {
                //Get current character
                thisChar = commandArgsArray[iCount];
                if (thisChar == 'A' || thisChar == 'B' || thisChar == 'C' || thisChar == 'L' || thisChar == 'Z')
                {
                    if (thisAxis == 'A' && (thisNumber.Length > 0) && (_image.JustifyAAxis.Justification != Justification.None))
                    {
                        _image.JustifyAAxis.Amount = Convert.ToSingle(thisNumber);
                    }
                    if ((thisAxis == 'B') && (thisNumber.Length > 0) && (_image.JustifyAAxis.Justification != Justification.None))
                    {
                        _image.JustifyBAxis.Amount = Convert.ToSingle(thisNumber);
                    }
                    if (thisChar == 'C')
                    {
                        if (thisAxis == 'A')
                        {
                            _image.JustifyAAxis.Justification = Justification.Center;
                            thisNumber = "";
                        }
                        if (thisAxis == 'B')
                        {
                            _image.JustifyBAxis.Justification = Justification.Center;
                            thisNumber = "";
                        }

                    }
                    if (thisChar == 'L')
                    {
                        if (thisAxis == 'A')
                        {
                            _image.JustifyBAxis.Justification = Justification.Left;
                            thisNumber = "";
                            thisAxis = thisChar;
                        }
                        if (thisAxis == 'B')
                        {
                            _image.JustifyBAxis.Justification = Justification.Left;
                            thisNumber = "";
                            thisAxis = thisChar;
                        }
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
