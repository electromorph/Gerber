using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Visualization
{
    public class Square : IComparable
    {
        public Square(Point TopLeft, Point BottomRight)
        {
            this.TopLeft = TopLeft;
            this.BottomRight = BottomRight;
        }
        
        public Point TopLeft { get; set; }
        public Point BottomRight { get; set; }
        public Point TopRight { get { return new Point(BottomRight.X, TopLeft.Y); } }
        public double Height { get { return (BottomRight.Y - TopLeft.Y); } }
        public double Width { get { return (BottomRight.X - TopLeft.X); } }
        public Point Center { get { return new Point((this.TopLeft.X + this.BottomRight.X) / 2, (this.TopLeft.Y + this.BottomRight.Y) / 2); } }

        public int CompareTo(object obj)
        {
            //Compares areas of squares.
            Square otherSquare = obj as Square;
            if (otherSquare != null)
            {
                return (this.Height * this.Width).CompareTo(otherSquare.Height * otherSquare.Width);
            }
            else
            {
                throw new ArgumentException("Object is not a Square");
            }
        }

        public override bool Equals(object obj)
        {
            //In this object, 'Equals' is taken to mean 'covered completely by'.
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;
            Square otherSquare = obj as Square;
            if ((this.TopLeft.X <= otherSquare.TopLeft.X)
                && (this.TopLeft.Y <= otherSquare.TopLeft.Y)
                && (this.BottomRight.X >= otherSquare.BottomRight.X)
                && (this.BottomRight.Y >= otherSquare.BottomRight.Y))
            // use this pattern to compare reference members
            //if (!Object.Equals(TopLeft, otherSquare.TopLeft)) return false;
            //if (!Object.Equals(BottomRight, otherSquare.BottomRight)) return false;
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public List<Point3D> Generate3DPositions()
        {
            var points = new List<Point3D>();
            
            //First Triangle
            points.Add(new Point3D(BottomRight.X, BottomRight.Y, 1)); 
            points.Add(new Point3D(BottomRight.X, TopLeft.Y, 1));
            points.Add(new Point3D(TopLeft.X, TopLeft.Y, 1));

            //Second Triangle
            points.Add(new Point3D(TopLeft.X, BottomRight.Y, 1));
            points.Add(new Point3D(BottomRight.X, BottomRight.Y, 1));
            points.Add(new Point3D(TopLeft.X, TopLeft.Y, 1));
            return points;
        }
    }
}
