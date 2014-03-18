using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GerberLogic
{
    public class PCB
    {
        public Bitmap TopSide;
        public Bitmap BottomSide;
        public Bitmap TopSolderMask;
        public Bitmap BottomSolderMask;
        public List<PointF> DrillPlot;
    }
}
