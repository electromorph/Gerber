using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GerberLogic.Commands
{
    public class KO : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public KO(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            char[] commandArgsArray = (_commandArgs + "Z").ToCharArray();
            char thisChar = ' ', thisAxis = 'Z'; string thisNumber = ""; float thisDigit = 0;
            Knockout ko = new Knockout();
            for (int iCount = 0; iCount < commandArgsArray.Length; iCount++)
            {
                //Get current character
                thisChar = commandArgsArray[iCount];
                if (thisChar == 'D')
                {
                    ko.Polarity = LayerPolarity.Dark;
                }
                if (thisChar == 'C')
                {
                    ko.Polarity = LayerPolarity.Clear;
                }
                if (thisChar == 'X' || thisChar == 'Y' || thisChar == 'I' || thisChar == 'J' || thisChar == 'Z')
                {
                    float startFromX = ko.StartFrom.X;
                    float startFromY = ko.StartFrom.Y;
                    if (thisAxis == 'X' && (thisNumber.Length > 0))
                    {
                        startFromX = Convert.ToSingle(thisNumber);
                        thisNumber = "";
                        thisAxis = thisChar;
                    }
                    if ((thisAxis == 'Y') && (thisNumber.Length > 0))
                    {
                        startFromY = Convert.ToSingle(thisNumber);
                        thisNumber = "";
                        thisAxis = thisChar;
                    }
                    ko.StartFrom = new PointF(startFromX, startFromY);
                    if (thisAxis == 'I' && (thisNumber.Length > 0))
                    {
                        ko.Width = Convert.ToSingle(thisNumber);
                        thisNumber = "";
                        thisAxis = thisChar;
                    }
                    if ((thisAxis == 'J') && (thisNumber.Length > 0))
                    {
                        ko.Height = Convert.ToSingle(thisNumber);
                        thisNumber = "";
                        thisAxis = thisChar;
                    }
                    if (thisAxis != 'Z')
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
