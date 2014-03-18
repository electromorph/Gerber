using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;
using System;
using System.Data;
using GerberLogic.Helper;
using GerberLogic.Commands;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GerberLogic
{
    public enum CommandTypes
    {
        Parameter,
        Command
    }

    public class FileParser
    {
        /// <summary>
        /// An object containing all global variables needed by Gerber.
        /// </summary>
        Globals globals = new Globals { LastCommand = "G01" };
                
        public Bitmap ProcessFile(string fileName)
        {
            Queue<string> originalFile = LoadFile(fileName);
            Queue<string> firstPass = GroupContentsOfPercentBracesOnIndividualLines(originalFile);
            Queue<string> secondPass = SplitMultiLinesInPercentBracesIntoSingleLines(firstPass);
            Queue<string> thirdPass = SplitCommandsIntoSingleLines(secondPass);
            
            var gerberImage = new GerberImage();
            Queue<iCommand> fourthPass = CreateCommandQueue(thirdPass, gerberImage);
            
            //Process the commands - and build a list of Components in gerberImage.Layers
            while (fourthPass.Count > 0)
            {
                iCommand command = fourthPass.Dequeue();
                command.Process();
            }
            
            //Turn the components into GraphicsPaths in gerberImage
            gerberImage = BitmapArtist.GeneratePathsForComponents(gerberImage);
            
            //Turn the paths into bitmaps.
            return BitmapArtist.GenerateBitmap(gerberImage);
        }

        public Queue<iCommand> CreateCommandQueue(Queue<string> fs, GerberImage gerberImage)
        {
            Queue<iCommand> commands = new Queue<iCommand>();
            while (fs.Count > 0)
            {
                foreach (iCommand command in CreateCommand(fs.Dequeue(), gerberImage))
                {
                    if (command != null)
                    {
                        commands.Enqueue(command);
                    }
                }
            }
            return commands;
        }
        
        public Dictionary<string, float> GetKeysAndValuesFromCommand(string Command)
        {
            Dictionary<string, float> commandParts = new Dictionary<string, float>();
            //Find the position of numeric and alpha characters
            string alphaMask = string.Empty;
            string currentLetter = "~";   //The tilde is not a Gerber character, and indicates that nothing has yet been found.
            string currentNumber = string.Empty;
            foreach (char character in Command.ToCharArray())
            {
                if (char.IsLetter(character))
                {
                    //If we have just read a character & number, then this is a change - write it out
                    if (currentLetter != "~")
                    {
                        currentLetter = currentLetter.ToUpper();
                        commandParts.Add(currentLetter, Helpers.PutCoordinatesToCorrectPower(globals.FormatG, currentNumber, currentLetter));
                        currentLetter = "~"; currentNumber = string.Empty;
                    }
                    if (!commandParts.ContainsKey(character.ToString().ToUpper()))
                    {
                        currentLetter = character.ToString();
                    }
                    //The following part added for long strings with X1Y1X2Y2X3Y3 etc.  (2012-01-11)
                    else
                    {

                    }
                }
                if ((char.IsNumber(character)) || (character == '.') || (character == '-'))
                {
                    currentNumber += character;
                }
            }
            if (currentLetter != "~")
            {
                float number = 0F;
                float.TryParse(currentNumber, out number);
                commandParts.Add(currentLetter, number);
                currentLetter = "~"; currentNumber = string.Empty;
            }
            return commandParts;
        }

        private iCommand GetCommand(string CommandCode, GerberImage gerberImage, object commandArgs)
        {
            Assembly a = Assembly.GetExecutingAssembly(); 
            iCommand o = (iCommand)a.CreateInstance("GerberLogic.Commands." + CommandCode, true, BindingFlags.InvokeMethod,
                null, new object[] { commandArgs, globals, gerberImage }, null, null);
            return o;
        }

        private Queue<iCommand> CreateCommand(string line, GerberImage gerberImage)
        {
            Queue<iCommand> lineToCommandList = new Queue<iCommand>();
            //If % at the start of cmd then we're dealing with an RS274X parameter, otherwise it's a G/M/D code line.
            if (line.StartsWith("%"))
            {
                //For parameters, it's easy to deduce the commands and the parameters because parameter codes
                //are all 2 characters long.
                line = line.Replace("%", string.Empty);
                string commandCode = line.Remove(2).ToUpper();
                string paramCommandArgs = line.Substring(2).ToUpper();
                lineToCommandList.Enqueue(GetCommand(commandCode, gerberImage, paramCommandArgs));
            }
            else
            {
                //It's a GCode style line.  This may equate to several real commands.
                //dict will contain every letter in the line.
                Dictionary<string, float> dict = GetKeysAndValuesFromCommand(line);
                //commandArgs will contains only the letters from dict that we're interested in.
                Dictionary<string, float> commandArgs = new Dictionary<string, float>();
                string commandCode = globals.LastCommand;
                float value;

                //If XYIJ values are supplied, then retrieve them, otherwise just use the last value used.
                commandArgs.Add("X", dict.TryGetValue("X", out value) ? value : globals.LastPoint.X);
                commandArgs.Add("Y", dict.TryGetValue("Y", out value) ? value : globals.LastPoint.Y);
                commandArgs.Add("I", dict.TryGetValue("I", out value) ? value : globals.LastIJValue.X);
                commandArgs.Add("J", dict.TryGetValue("J", out value) ? value : globals.LastIJValue.Y);
                //TODO:  Apply the X and Y decimal point stuff here...

                if (dict.TryGetValue("D", out value))
                {
                    if (value < 10)
                    {
                        //D1-9 are actually different commands.
                        commandCode = "D" + value.ToString();
                        if (commandCode == "D3")
                        {
                            globals.LastCommand = "D3";
                        }
                    }
                    else
                    {
                        //Dx where x > 9 means 'switch to aperture x'.  So same command different parameters.
                        commandArgs.Add("D", (dict.TryGetValue("D", out value)) ? value : 0F);
                        commandCode = "D";
                    }
                    lineToCommandList.Enqueue(GetCommand(commandCode, gerberImage, commandArgs));
                }

                //Gx are distinct commands.
                if (dict.TryGetValue("G", out value))
                {
                    commandCode = "G" + value.ToString();
                    lineToCommandList.Enqueue(GetCommand(commandCode, gerberImage, commandArgs));
                    if ((commandCode == "G0") || (commandCode == "G1") || (commandCode == "G2") || (commandCode == "G3"))
                    {
                        globals.LastCommand = commandCode;
                    }
                }
                else
                {
                    //If no G code specified, then use the last command unless it's D3 (in which case it's already been done above)
                    if (commandCode != "D3")
                    {
                        commandCode = globals.LastCommand;
                        lineToCommandList.Enqueue(GetCommand(globals.LastCommand, gerberImage, commandArgs));
                    }
                }

                //M command. In fact we're only interested in M3 (stop program).
                if (dict.TryGetValue("M", out value))
                {
                    if (value == 3)
                    {
                        commandCode = "M" + value.ToString();
                        lineToCommandList.Enqueue(GetCommand(commandCode, gerberImage, commandArgs));
                    }
                }
                globals.LastPoint = new PointF(commandArgs["X"],commandArgs["Y"]);
            }
            return lineToCommandList;
        }

        //****************************************************************************

        public Queue<string> LoadFile(string fileName)
        {
            Queue<string> originalFile = new Queue<string>();
            if (fileName != string.Empty)
            {
                TextReader tr = File.OpenText(fileName);
                
                while (tr.Peek() != -1)
                {
                    originalFile.Enqueue(tr.ReadLine());
                }
                tr.Close();
            }
            return originalFile;
        }

        public int AddOne(int a)
        {
            return a + 1;
        }

        public Queue<string> GroupContentsOfPercentBracesOnIndividualLines(Queue<string> tr)
        {
            Queue<string> tw = new Queue<string>();
            string line = string.Empty;
            while (tr.Count > 0)
            {
                line = tr.Dequeue();
                while (line.Length > 0)
                {
                    if (line.Contains('%'))
                    {
                        string partBeforePercent = line.Substring(0, line.IndexOf('%'));
                        if (partBeforePercent.Length > 0)
                        {
                            tw.Enqueue(partBeforePercent);
                        }
                        line = line.Remove(0, line.IndexOf('%') + 1);
                        //Does remainer contain %? If not, then keep reading & appending to it 'till it does.
                        while (!line.Contains('%'))
                        {
                            if (tr.Count == 0)
                            {
                                if (line.Length > 0)
                                {
                                    if (!string.IsNullOrEmpty(line))
                                    {
                                        tw.Enqueue(line);
                                    }
                                }
                                return tw;
                            }
                            else
                            {
                                string readLine = tr.Dequeue();
                                line = line + readLine;
                            }
                        }
                        if (line.Length == 0)
                        {
                            break;
                        }
                        partBeforePercent = line.Substring(0, line.IndexOf('%'));
                        if (partBeforePercent.Length > 0)
                        {
                            tw.Enqueue("%" + partBeforePercent + "%");
                            line = line.Remove(0, line.IndexOf('%') + 1);
                            if (line.Length == 0)
                                break;
                        }
                    }
                    else
                    {
                        tw.Enqueue(line);
                        line = string.Empty;
                    }
                }
            }
            return tw;
        }

        public Queue<string> SplitMultiLinesInPercentBracesIntoSingleLines(Queue<string> tr)
        {
            Queue<string> tw = new Queue<string>();
            string line = string.Empty;
            while (tr.Count > 0)
            {
                line = tr.Dequeue();
                
                //If the first character of the next line is numeric, append it because it's probably a continuation of this line.
                if (tr.Count > 0)
                {
                    if (char.IsNumber(tr.Peek().Substring(0, 1).ToCharArray()[0]))
                    {
                        line += tr.Dequeue();
                    }
                }

                if (line.Length > 0)
                {
                    if (line.StartsWith("%"))
                    {
                        //Remove % braces
                        int charsToRemoveFromEndOfString = line.EndsWith("%") ? 1 : 0;
                        line = line.Substring(1, line.Length - (1 + charsToRemoveFromEndOfString));
                        
                        //Create an array of commands separated by '*' and remove *s.
                        string[] arrayOfSubcommands = line.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                        int counter = 0;
                        
                        //If the second parameter starts with a number, then join first & second parameters together separated by a star.
                        do
                        {
                            string parameter = arrayOfSubcommands[counter];
                            string nextParameter;
                            bool nextParameterStartsNumeric = false;
                            nextParameter = (counter < (arrayOfSubcommands.Length - 1)) ? arrayOfSubcommands[counter+1] : string.Empty;
                            if (!string.IsNullOrEmpty(nextParameter))
                            {
                                nextParameterStartsNumeric = (string.IsNullOrEmpty(nextParameter)) ? false : char.IsNumber(nextParameter.Substring(0, 1).ToCharArray()[0]);
                            }
                            if ((parameter.Length > 0))
                            {
                                if (nextParameterStartsNumeric)
                                {
                                    tw.Enqueue("%" + parameter + "*" + nextParameter + "%");
                                }
                                else
                                {
                                    tw.Enqueue("%" + parameter + "%");
                                    if (!string.IsNullOrEmpty(nextParameter))
                                    {
                                        tw.Enqueue("%" + nextParameter + "%");
                                    }
                                }
                            }
                            counter += 2;
                        }
                        while (counter <= arrayOfSubcommands.Length - 1);
                    }
                    else
                    {
                        //It's not parameters - just write it.
                        tw.Enqueue(line);
                    }
                }
            }
            return tw;
        }

        public Queue<string> SplitCommandsIntoSingleLines(Queue<string> tr)
        {
            Queue<string> tw = new Queue<string>();
            string buffer = string.Empty;
            string line = string.Empty;
            while (tr.Count > 0)
            {
                line = tr.Dequeue();
                if (line.Length > 0)
                {
                    if (line.StartsWith("%"))
                    {
                        if (buffer.Length > 0)
                        {
                            tw.Enqueue(buffer);
                            buffer = string.Empty;
                        }
                        tw.Enqueue(line);
                    }
                    else
                    {
                        buffer += line;
                        if (buffer.Contains('*'))
                        {
                            var commands = buffer.Split('*').Where(command => command.Length > 0);
                            foreach (string command in commands)
                            {
                                tw.Enqueue(command);
                                buffer = string.Empty;
                            }
                        }
                    }
                }
            }
            if (buffer.Length > 0)
            {
                tw.Enqueue(buffer);
            }
            return tw;
        }


    }
}
