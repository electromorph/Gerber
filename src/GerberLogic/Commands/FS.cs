using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GerberLogic.Helper;

namespace GerberLogic.Commands
{
    public class FS : iCommand
    {
        string _commandArgs;
        Globals _globals;
        GerberImage _image;

        public FS(string CommandArgs, Globals Globals, GerberImage Image)
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
                char c = commandArgsArray[iCount];
                if (c == 'L')
                {
                    _globals.OmitLeadingZeros = true;
                    commandArgsArray[iCount] = ' ';
                }
                if (c == 'T')
                {
                    _globals.OmitLeadingZeros = false;
                }
                if (c == 'A')
                {
                    _globals.Coordinates = Coordinates.Absolute;
                    commandArgsArray[iCount] = ' ';
                }
                if (c == 'I')
                {
                    _globals.Coordinates = Coordinates.Incremental;
                }
                if (c == 'N')
                {
                    //This is an internal command which needs to be built up.
                    //Iterate through 'till we get the next commmand;
                    string buildSFCommand = "";
                    char d = ' ';
                    do
                    {
                        iCount++;
                        if (iCount < commandArgsArray.Length)
                        {
                            d = commandArgsArray[iCount];
                            buildSFCommand += d;
                        }
                        else
                        {
                            break;
                        }
                    } while ((d != 'G') && (d != 'L') && (d != 'A') && (d != 'N') && (d != 'D') && (d != 'M'));
                    Helpers.PopulateNumberFormat(_globals.FormatN, buildSFCommand);
                    //We're on the next command now - back up one character so that
                    //the for loop has a chance to parse it.
                    iCount--;
                }
                if (_commandArgs.Contains("G"))
                {
                    //This is an internal command which needs to be built up.
                    //Iterate through 'till we get the next commmand;
                    string buildSFCommand = "";
                    char d = ' ';
                    do
                    {
                        iCount++;
                        if (iCount < commandArgsArray.Length)
                        {
                            d = commandArgsArray[iCount];
                            buildSFCommand += d;
                        }
                        else
                        {
                            break;
                        }
                    } while ((d != 'G') && (d != 'L') && (d != 'A') && (d != 'N') && (d != 'D') && (d != 'M'));
                    Helpers.PopulateNumberFormat(_globals.FormatG, buildSFCommand);
                    //We're on the next command now - back up one character so that
                    //the for loop has a chance to parse it.
                    iCount--;
                }
                if (_commandArgs.Contains("D"))
                {
                    //This is an internal command which needs to be built up.
                    //Iterate through 'till we get the next commmand;
                    string buildSFCommand = "";
                    char d = ' ';
                    do
                    {
                        iCount++;
                        if (iCount < commandArgsArray.Length)
                        {
                            d = commandArgsArray[iCount];
                            buildSFCommand += d;
                        }
                        else
                        {
                            break;
                        }
                    } while ((d != 'G') && (d != 'L') && (d != 'A') && (d != 'N') && (d != 'D') && (d != 'M'));
                    Helpers.PopulateNumberFormat(_globals.FormatD, buildSFCommand);
                    //We're on the next command now - back up one character so that
                    //the for loop has a chance to parse it.
                    iCount--;
                }
                if (_commandArgs.Contains("M"))
                {
                    //This is an internal command which needs to be built up.
                    //Iterate through 'till we get the next commmand;
                    string buildSFCommand = "";
                    char d = ' ';
                    do
                    {
                        iCount++;
                        if (iCount < commandArgsArray.Length)
                        {
                            d = commandArgsArray[iCount];
                            buildSFCommand += d;
                        }
                        else
                        {
                            break;
                        }
                    } while ((d != 'G') && (d != 'L') && (d != 'A') && (d != 'N') && (d != 'D') && (d != 'M'));
                    Helpers.PopulateNumberFormat(_globals.FormatM, buildSFCommand);
                    //We're on the next command now - back up one character so that
                    //the for loop has a chance to parse it.
                    iCount--;
                }
                if (c == 'X')
                {
                    //This is an internal command which needs to be built up.
                    //Iterate through 'till we get the next commmand;
                    string buildSFCommand = "";
                    char d = ' ';
                    do
                    {
                        if (iCount < commandArgsArray.Length)
                        {
                            d = commandArgsArray[iCount];
                            buildSFCommand += d;
                        }
                        else
                        {
                            break;
                        }
                        iCount++;
                    } while ((d != 'G') && (d != 'L') && (d != 'A') && (d != 'N') && (d != 'D') && (d != 'M'));
                    Helpers.PopulateNumberFormat(_globals.FormatN, buildSFCommand);
                    Helpers.PopulateNumberFormat(_globals.FormatG, buildSFCommand);
                    Helpers.PopulateNumberFormat(_globals.FormatD, buildSFCommand);
                    Helpers.PopulateNumberFormat(_globals.FormatM, buildSFCommand);

                    //We're on the next command now - back up one character so that
                    //the for loop has a chance to parse it.
                    iCount--;
                }
                if (c == 'Y')
                {
                    //This is an internal command which needs to be built up.
                    //Iterate through 'till we get the next commmand;
                    string buildSFCommand = "";
                    char d = ' ';
                    do
                    {
                        if (iCount < commandArgsArray.Length)
                        {
                            d = commandArgsArray[iCount];
                            buildSFCommand += d;
                        }
                        else
                        {
                            break;
                        }
                        iCount++;
                    } while ((d != 'G') && (d != 'L') && (d != 'A') && (d != 'N') && (d != 'D') && (d != 'M'));
                    Helpers.PopulateNumberFormat(_globals.FormatN, buildSFCommand);
                    Helpers.PopulateNumberFormat(_globals.FormatG, buildSFCommand);
                    Helpers.PopulateNumberFormat(_globals.FormatD, buildSFCommand);
                    Helpers.PopulateNumberFormat(_globals.FormatM, buildSFCommand);
                    //We're on the next command now - back up one character so that
                    //the for loop has a chance to parse it.
                    iCount--;
                }
            }
        }
    }
}
