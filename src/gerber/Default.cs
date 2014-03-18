using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using GerberLogic;
using BoardForge;

namespace UI
{
    public partial class Default : Form
    {
        //Represents a Top/Bottom dot pair on the PCB
        private class DotPair
        {
            public bool Top { get; set; }
            public bool Bottom { get; set; }
        }

        private enum Direction
        {
            Left = 0,
            Right = 1
        }

        GerberImage im = new GerberImage();
        
        readonly Forge boardScribe = new Forge();

        //usb_interface usb_int = new usb_interface();

        public Default()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private string GetFileName(string FileName, string Tag)
        {
            //Path.GetTempFileName()
            string s = Path.GetDirectoryName(FileName) + "\\" + Path.GetFileNameWithoutExtension(FileName) + Tag + Path.GetExtension(FileName);
            return s;
        }
        
        private void btnProcess_Click(object sender, EventArgs e)
        {
            FileParser fp = new FileParser();
            Bitmap bmp = fp.ProcessFile(txtFileName.Text);
            Refresh();
            SaveBmp(bmp);
            //WriteTextFile(bmp);
            //WriteFileToBoardScribe(@"c:\gerber\textoutput.txt");
            Application.Exit();
        }

        private void SaveBmp(Bitmap bmp)
        {
            bmp.Save(@"C:\_MAX\Gerber\TestFiles\m.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// Write a file with 1s and zeros that is easily transmissible to the BoardForge.
        /// </summary>
        /// <param name="bmp"></param>
        protected void WriteTextFile(Bitmap bmp)
        {
            string s = "";
            using (TextWriter fs = new StreamWriter(@"c:\gerber\textoutput.txt"))
            {
                for (int i = 0; i < bmp.Height; i++)
                {
                    for (int j = 0; j < bmp.Width; j++)
                    {
                        Color c = bmp.GetPixel(i, j);
                        s += ((c.R + c.G + c.B) == 0) ? "." : "O";
                    }
                    lblLineCount.Text = "GetPixel " + i;
                    Refresh();
                    fs.WriteLine(s);
                }
            }
        }

        private void WriteFileToBoardScribe(string FileNameTop, string FileNameBottom)
        {
            Direction direction = Direction.Left;
            boardScribe.MoveFarLeft();

            //open txt file containing . and O
            TextReader fsTop = File.OpenText(FileNameTop);
            TextReader fsBottom = File.OpenText(FileNameBottom);

            string fileLineTop = fsTop.ReadLine();
            string fileLineBottom = fsBottom.ReadLine();

            while (fileLineTop != null || fileLineBottom != null)
            {
                //Quit when BOTH files have no more lines.
                if (fsBottom.Peek() == -1 && fsBottom.Peek() == -1)
                {
                    break;
                }
                else
                {
                    //If one file is at an end (returning null) then return an empty string.
                    fileLineTop = (fileLineTop == null) ? String.Empty : fileLineTop;
                    fileLineBottom = (fileLineBottom == null) ? String.Empty : fileLineBottom;
                }
                
                //Pad: find the greater of the line widths, and pad the one that is shorter to the length of the longer.
                int greatestWidth = fileLineTop.Length > fileLineBottom.Length ? fileLineTop.Length : fileLineBottom.Length;
                fileLineTop.PadRight(greatestWidth);
                fileLineBottom.PadRight(greatestWidth);

                //Toggle direction
                direction = (direction == Direction.Left) ? Direction.Right : Direction.Left;
                
                //DrawBmpLine the line
                DrawBmpLine(fileLineTop, fileLineBottom, direction);
                
                //Read next line
                fileLineTop = (fsTop.Peek() == -1) ? string.Empty : fsTop.ReadLine();
                fileLineBottom = (fsBottom.Peek() == -1) ? string.Empty : fsBottom.ReadLine();
            }
            
            fsTop.Close();
            fsBottom.Close();
        }
        
        private void DrawBmpLine(string lineTop, string lineBottom, Direction direction)
        {
            var dotPairs = new List<DotPair>();

            char[] lineArrayTop = lineTop.ToCharArray();
            char[] lineArrayBottom = lineBottom.ToCharArray();

            if (direction == Direction.Left)
            {
                Array.Reverse(lineArrayBottom);
            }
            else
            {
                Array.Reverse(lineArrayTop);
            }

            for (int count = 0; count < lineArrayTop.Length; count++)
            {
                dotPairs.Add(new DotPair() { Top = lineArrayTop[count] == 'O', Bottom = lineArrayBottom[count] == 'O' });
            }

            foreach (DotPair dotPair in dotPairs)
            {
                boardScribe.LightTop(false);
                boardScribe.LightBottom(false);
                
                if (direction == Direction.Left)
                {
                    boardScribe.MoveRight();
                }
                else
                {
                    boardScribe.MoveLeft();
                }
                
                boardScribe.LightTop(dotPair.Top);
                boardScribe.LightBottom(dotPair.Bottom);

                //Dwell a while
                Thread.Sleep(100);
            }
            boardScribe.MoveForward();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            ofDialog.ShowDialog();
            txtFileName.Text = ofDialog.FileName;
            btnProcess.Enabled = txtFileName.Text.Length > 0;
        }

        private void btnIndexLeft_Click(object sender, EventArgs e)
        {
            boardScribe.MoveLeft();
        }

        private void btnIndexRight_Click(object sender, EventArgs e)
        {
            boardScribe.MoveRight();
        }

        private void btnLEDOn_Click(object sender, EventArgs e)
        {
            boardScribe.LightTop(true);
        }

        private void btnLEDOff_Click(object sender, EventArgs e)
        {
            boardScribe.LightBottom(false);
        }

    }
}