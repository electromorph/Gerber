using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Commands
{
    public class MI : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public MI(string CommandArgs, Globals Globals, GerberImage Image)
        {
            _commandArgs = CommandArgs;
            _globals = Globals;
            _image = Image;
        }
        
        public void Process()
        {
            char[] commandArgsArray = _commandArgs.ToCharArray();
            for (int iCount = 0; iCount < commandArgsArray.Length; iCount++)
            {
                bool result = false; int value = 0;
                if ((iCount + 1) < commandArgsArray.Length)
                {
                    if (int.TryParse(commandArgsArray[iCount++].ToString(), out value))
                    {
                        result = (value == 1);
                    }
                }
                if (commandArgsArray[iCount - 1] == 'A')
                {
                    _globals.MirrorImage.A = result;
                }
                if (commandArgsArray[iCount - 1] == 'B')
                {
                    _globals.MirrorImage.B = result;
                }
            }
        }
    }
}
