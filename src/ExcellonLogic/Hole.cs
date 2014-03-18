using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace ExcellonLogic
{
    public class Hole
    {
        public Point3D Position { get; set; }
        public DrillBit Tool { get; set; }
    }
}
