using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Media.Media3D;

namespace ExcellonLogic
{
    /// <summary>
    /// This class scans the file to pull out all of the tools and their sizes.
    /// </summary>
    public class ExcellonFile
    {
        List<string> errors = new List<string>();
        List<DrillBit> drillsSpecified = new List<DrillBit>();
        List<string> commandList = new List<string>();
        List<Hole> holeList = new List<Hole>();
        List<DrillBit> drillsUsed = new List<DrillBit>();

        public List<DrillBit> DrillsSpecified { get { return drillsSpecified; } }
        
        public List<DrillBit> DrillsUsed { get { return DrillsUsed;  } }

        public List<string> Errors { get { return errors; } }

        public List<Hole> ThroughHoles { get { return holeList; } }

        public void ProcessFile(string fileName)
        {
            commandList = LoadFile(fileName);
            drillsSpecified = GetDrillSpecifications(commandList);
            holeList = GetHoleList(commandList, drillsSpecified);
        }

        public List<DrillBit> GetDrillSpecifications(List<string> commandList)
        {
            //Look for lines with T and C in them.  Extract the drill number & size information.
            //We don't care where in the file the tool is defined.
            return ( from line in commandList
                     where ( line.Contains('C') && line.Contains('T') )
                     select new DrillBit {
                             ToolNumber = Convert.ToInt32(GetNumberAfter(line, "T").Value),
                             Diameter = GetNumberAfter(line, "C") ?? 0
                                         }
                           ).ToList();
        }

        public List<Hole> GetHoleList(List<string> commandList, List<DrillBit> drillList)
        {
            List<Hole> listOfHoles = new List<Hole>();
            
            double xPosition = 0;
            double yPosition = 0;
            DrillBit currentDrill = null;

            foreach (string line in commandList)
            {
                if (line.Contains("C"))
                {
                    continue;
                }
                //Get tool number if specified (if not yet specified, we'll use Tool#-1 - the default with diameter 0)
                double? toolSpecifiedOnLine = GetNumberAfter(line, "T");
                
                if (toolSpecifiedOnLine.HasValue)
                {
                    //We're being asked to change the drill.
                    int toolSpecified = Convert.ToInt32(toolSpecifiedOnLine.Value);
                    DrillBit newDrillBit = drillList.FirstOrDefault(d => d.ToolNumber == toolSpecifiedOnLine.Value);
                    if (newDrillBit == null)
                    {
                        //Tool requested doesn't exist - add it with a diameter of -1 (so we know it's missing its diameter).
                        drillList.Add(new DrillBit(toolSpecified, -1));
                        currentDrill = drillList.FirstOrDefault(d => d.ToolNumber == toolSpecified);
                    }
                    else
                    {
                        //Use the new drill as specified.
                        currentDrill = newDrillBit;
                    }
                }
                xPosition = GetNumberAfter(line, "X") ?? xPosition;
                yPosition = GetNumberAfter(line, "Y") ?? yPosition;
                listOfHoles.Add(new Hole() { Position = new Point3D(xPosition, yPosition, 0), Tool = currentDrill });
            }
            drillsUsed = drillList;
            return listOfHoles;
        }

        public double? GetNumberAfter(string line, string character)
        {
            Match T = Regex.Match(line, character + @"[\d]*[\.]*[\d]");
            if (T.Length > 0)
            {
                double res = 0;
                if (double.TryParse(T.Value.Substring(1), out res))
                    return res;
            }
            return null;
        }

        public List<string> ErrorCheckFile(List<string> commandList)
        {
            //Return lines with duplicate X, Y or T definitions.
            return (from lineToCheck in commandList
                    where (
                            //Regex strips out numbers & decimals, which can legitimately repeat.
                            from character in Regex.Replace(lineToCheck, @"[\d]*[\.]*", "").ToCharArray()
                            group character by character into groupedCharacter
                            where groupedCharacter.Count() > 1
                            select new { Letter = groupedCharacter }).Count() > 0
                    select lineToCheck).ToList();
        }

        public List<string> LoadFile(string fileName)
        {
            List<string> originalFile = new List<string>();
            if (fileName != string.Empty)
            {
                TextReader tr = File.OpenText(fileName);
                while (tr.Peek() != -1)
                {
                    originalFile.Add(tr.ReadLine().ToUpper());
                }
                tr.Close();
            }
            return originalFile;
        }
    }
}
