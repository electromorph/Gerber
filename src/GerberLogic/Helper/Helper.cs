using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Specialized;

namespace GerberLogic.Helper
{
    public static class Helpers
    {
        public static float GetParam(char Param, string CommandArgs, Globals Globals)
        {
            //If the parameter does not occur on the line, then return 0 unless it's X or Y
            //(where we return the last value)
            if (!CommandArgs.Contains(Param.ToString()))
            {
                switch (Param)
                {
                    case 'X':
                        return Globals.LastPoint.X;
                    case 'Y':
                        return Globals.LastPoint.Y;
                    default:
                        return 0;
                }
            }
            //This only works if only one instance of the parameter appears on the same line. That should be the case...
            char[] commandArgsArray = (CommandArgs + "Z").ToCharArray();
            char thisChar = ' '; string thisNumber = string.Empty; float thisDigit = 0;
            float returnNumber = 0;
            for (int iCount = CommandArgs.IndexOf(Param) + 1; iCount < commandArgsArray.Length; iCount++)
            {
                thisChar = commandArgsArray[iCount];
                //Get current character& add it to the number.
                if ((thisChar == '-') || (thisChar == '.'))
                {
                    thisNumber += thisChar;
                    continue;
                }
                if (float.TryParse(thisChar.ToString(), out thisDigit))
                {
                    thisNumber = thisNumber + thisChar;
                    continue;
                }
                //NonAlpha - time to stop parsing string.
                float rtn = 0;
                if (float.TryParse(thisNumber, out rtn))
                {
                    //We have to worry about where the decimal point is. If it's explicitly there,
                    //then we can just take it as said that the number is correct. If the decimal
                    //point is not there, then we have to look at pre-set assumptions (SF value)
                    //to deduce where it should be. The SF values for X and Y are independent.
                    if (thisNumber.Contains("."))
                    {
                        returnNumber = rtn;
                    }
                    else
                    {
                        float factor = 0;
                        if ((Param == 'X') || (Param == 'I'))
                        {
                            factor = Globals.FormatG.X.DecimalPart;
                        }
                        else if ((Param == 'Y') || (Param == 'J'))
                        {
                            factor = Globals.FormatG.Y.DecimalPart;
                        }
                        returnNumber = returnNumber = rtn / Convert.ToSingle(Math.Pow(10, factor));
                    }
                }
                break;
            }
            return returnNumber;
        }

        public static float PutCoordinatesToCorrectPower(CommandNumberFormat cnf, string inputNumber, string Coordinate)
        {
            //We have to worry about where the decimal point is. If it's explicitly there,
            //then we can just take it as said that the number is correct. If the decimal
            //point is not there, then we have to look at pre-set assumptions (SF value)
            //to deduce where it should be. The SF values for X and Y are independent.
            float outNumber = 0F;
            //if (inputNumber.Contains("."))
            //{
                //if (!float.TryParse(inputNumber, out outNumber))
                //{
                //    outNumber = 0F;
                //}
            //}
            //else
            //{
                //float factor = 0;
                //if ((Coordinate == "X") || (Coordinate == "I"))
                //{
                //    factor = cnf.X.DecimalPart;
                //}
                //else if ((Coordinate == "Y") || (Coordinate == "J"))
                //{
                //    factor = cnf.Y.DecimalPart;
                //}
                //if (float.TryParse(inputNumber, out outNumber))
                //{
                //    //outNumber = outNumber / Convert.ToSingle(Math.Pow(10, factor));
                //}
                //else
                //{
                //    outNumber = 0F;
                //}
            //}
            float.TryParse(inputNumber, out outNumber);
            return outNumber;
        }
        
        public static float ApplyDecimalFormat(Globals _globals, string Param, float inNumber)
        {
            float returnNumber = 0F;
            if (inNumber - Convert.ToInt32(inNumber) > 0)
            {
                //The number has a decimal point - leave it as is.
                return returnNumber;
            }
            else
            {
                float factor = 0;
                if ((Param == "X") || (Param == "I"))
                {
                    factor = _globals.FormatG.X.DecimalPart;
                }
                else if ((Param == "Y") || (Param == "J"))
                {
                    factor = _globals.FormatG.Y.DecimalPart;
                }
                returnNumber = returnNumber = inNumber / Convert.ToSingle(Math.Pow(10, factor));
            }
            return returnNumber;
        }
        
        public static void PopulateIndividualAxis(CommandNumberFormat cf, char axis, int integerLength, int doubleLength)
        {
            //Set defaults
            integerLength = (integerLength == -1) ? 6 : integerLength;
            doubleLength = (doubleLength == -1) ? 6 : doubleLength;
            if (axis == 'X')
            {
                cf.X.IntegerPart = integerLength;
                cf.X.DecimalPart = doubleLength;
            }
            if (axis == 'Y')
            {
                cf.Y.IntegerPart = integerLength;
                cf.Y.DecimalPart = doubleLength;
            }
        }

        public static void PopulateNumberFormat(CommandNumberFormat CommandToFill, string numberFormatCommand)
        {
            int integerLength = -1;
            int doubleLength = -1;
            char currentAxis = ' ';
            char[] formatArray = numberFormatCommand.ToCharArray();
            for (int iCount = 0; iCount < numberFormatCommand.Length; iCount++)
            {
                char thisChar = formatArray[iCount];
                if ((thisChar == 'X') || (thisChar == 'Y'))
                {
                    if (integerLength != -1)
                    {
                        if (currentAxis != ' ')
                        {
                            PopulateIndividualAxis(CommandToFill, currentAxis, integerLength, doubleLength);
                            //reset all variables.
                            integerLength = -1;
                            doubleLength = -1;
                            currentAxis = ' ';
                        }
                    }
                    currentAxis = thisChar;
                }
                else
                {
                    int number = 0;
                    //If numeric
                    if (int.TryParse(thisChar.ToString(), out number))
                    {
                        //If integerlength is not yet set then set it first.
                        if (integerLength == -1)
                        {
                            //This is the integer part
                            integerLength = number;
                        }
                        else
                        {
                            //It is the float part.
                            doubleLength = number;
                        }
                    }
                }
            }
            if (currentAxis != ' ')
            {
                PopulateIndividualAxis(CommandToFill, currentAxis, integerLength, doubleLength);
            }
        }

        public static string GetNumericPart(string theString)
        {
            bool foundOneNumber = false;
            char[] stringArray = theString.ToCharArray();
            string numericPartString = "";
            foreach (char c in stringArray)
            {
                string c2 = c.ToString();
                int result = 0;
                if (c == '.')
                {
                    numericPartString += c;
                    continue;
                }
                if (int.TryParse(c2, out result))
                {
                    foundOneNumber = true;
                    numericPartString += c2;
                }
                else
                {
                    if (foundOneNumber)
                        //The number has finished now. Further numbers are probably
                        //part of the aperture macro name.
                        break;
                }
            }
            return numericPartString;
        }
    }
}
