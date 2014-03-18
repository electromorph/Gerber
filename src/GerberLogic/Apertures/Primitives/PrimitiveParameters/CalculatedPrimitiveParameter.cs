using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GerberLogic.Apertures.Primitives.PrimitiveParameters
{
    public class CalculatedPrimitiveParameter : iPrimitiveParameter
    {
        public string Calculation { get; set; }
        
        public float Result()
        {
            //Parse the calculation
            string tempChar = Calculation;
            char[] splitString = { '+', '-', '*', '/' };
            string[] calcParts = tempChar.Split(splitString, StringSplitOptions.RemoveEmptyEntries);
            string[,] calcAnalysis = new string[calcParts.Length, 2];
            for (int i = 0; i < calcParts.Length - 1; i++)
            {
                int nextOperatorIndex = tempChar.IndexOfAny(splitString);
                calcAnalysis[i, 0] = calcParts[i];
                calcAnalysis[i, 1] = tempChar.Substring(nextOperatorIndex, 1);
                tempChar = tempChar.Remove(0, nextOperatorIndex);
            }
            return 0;
        }

        public CalculatedPrimitiveParameter(string Calculation)
        {
            this.Calculation = Calculation;
        }
    }
}
