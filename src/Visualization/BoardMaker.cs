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
    public enum Side { Front, Back }

    public class Via
    {
        
        Point _centre = new Point(0,0);
        double _diameter = 0;

        public Point Centre { get { return _centre; } set { _centre = value; } }
        public double Diameter { get { return _diameter; } set { _diameter = value; } }

        public Via(Point Centre, double Diameter)
        {
            _centre = Centre;
            _diameter = Diameter;
        }
    }

    public class BoardMaker
    {
        List<Via> _vias;
        Square _boardOutline;

        public Square BoardOutline { get { return _boardOutline; } set { _boardOutline = value; } }
        public List<Via> Vias { get { return _vias; } set { _vias = value; } }

        public BoardMaker(Square BoardOutline, List<Via> Vias)
        {
            _vias = Vias;
            _boardOutline = BoardOutline;
        }

        public BoardMaker()
        {
            _vias = new List<Via>();
        }

        public void Add(Via Via)
        {
            _vias.Add(Via);
        }

        public void Add(Point Centre, double Diameter)
        {
            _vias.Add(new Via(Centre, Diameter));
        }

        public static MeshGeometry3D Generate(Square BoardOutline, List<Via> Vias, Side side)
        {
            return new BoardMaker(BoardOutline, Vias).Generate(side);
        }

        public MeshGeometry3D Generate(Side side)
        {
            var mesh = new MeshGeometry3D();
            //Draw the holes
            int largestTriangleIndexForLastHole = 0;
            foreach (Via via in Vias)
            {
                var viaGenerator = new SimpleHoleGenerator();
                MeshGeometry3D holeMesh = viaGenerator.GetMeshGeometry(via.Centre, via.Diameter, side);
                
                foreach (Point3D point in holeMesh.Positions)
                {
                    mesh.Positions.Add(new Point3D(point.X, point.Z, point.Y));
                    mesh.TextureCoordinates.Add(new Point(point.X, point.Z));
                }
                foreach (int triangleIndex in holeMesh.TriangleIndices)
                {
                    mesh.TriangleIndices.Add(triangleIndex + largestTriangleIndexForLastHole);
                }
                largestTriangleIndexForLastHole = mesh.Positions.Count;
            }
            //Fill in gaps between vias.
            var listOfVias = TurnViasIntoSquares(_vias);
            List<Square> squares = BoardMaker.GenerateMissingSquares(listOfVias, this.BoardOutline);
            var squarePositionsList = squares.Select(x => x.Generate3DPositions());
            int index = largestTriangleIndexForLastHole;
            foreach (List<Point3D> squarePositions in squarePositionsList)
            {
                foreach(Point3D point in squarePositions)
                {
                    mesh.Positions.Add(point);
                    mesh.TriangleIndices.Add(index++);
                    mesh.TextureCoordinates.Add(new Point(point.X, point.Y));
                }
            }
            return mesh;          
        }

        public static List<Square> TurnViasIntoSquares(List<Via> Vias)
        {
            List<Square> squares = new List<Square>();
            foreach (Via via in Vias)
            {
                double radius = via.Diameter/2;
                Point topLeft = new Point(via.Centre.X - radius, via.Centre.Y - radius);
                Point bottomRight = new Point(via.Centre.X + radius, via.Centre.Y + radius);
                squares.Add(new Square(topLeft, bottomRight));
            }
            return squares;
        }
        
        public static List<Square> GenerateMissingSquares(List<Square> vias, Square board)
        {
            var missingSquares = new List<Square>();
            
            //Get X Breaks
            var xBreaks = new List<double>();
            xBreaks.Add(board.TopLeft.X);
            xBreaks.AddRange(vias.Select(c => c.TopLeft.X).Distinct().ToList<double>());
            xBreaks.AddRange(vias.Select(c => c.BottomRight.X).Distinct().ToList<double>());
            xBreaks.Add(board.BottomRight.X);
            xBreaks.Sort();

            //Get Y Breaks
            var yBreaks = new List<double>();
            yBreaks.Add(board.TopLeft.X);
            yBreaks.AddRange(vias.Select(c => c.TopLeft.Y).Distinct().ToList<double>());
            yBreaks.AddRange(vias.Select(c => c.BottomRight.Y).Distinct().ToList<double>());
            yBreaks.Add(board.BottomRight.Y);
            yBreaks.Sort();

            //You need two points for a square! So we can't generate anything 'till second pass.
            bool firstPassThroughXBreaks = true;
            bool firstPassThroughYBreaks = true;
            double lastXBreak = 0;
            double lastYBreak = 0;
            
            foreach (double yBreak in yBreaks)
            {
                if (firstPassThroughYBreaks)
                {
                    //get first Y point of square.
                    firstPassThroughYBreaks = false;
                    lastYBreak = yBreak;
                }
                else
                {
                    foreach (double xBreak in xBreaks)
                    {
                        if (firstPassThroughXBreaks)
                        {
                            firstPassThroughXBreaks = false;
                            lastXBreak = xBreak;
                        }
                        else
                        {
                            //Generate the square
                            Square sqr = new Square(new Point(lastXBreak, lastYBreak), new Point(xBreak, yBreak));
                            if (!vias.Contains(sqr))
                            {
                                missingSquares.Add(sqr);
                            }
                            lastXBreak = xBreak;
                        }
                    }
                    //Reset this flag for each row.
                    firstPassThroughXBreaks = true;
                    lastYBreak = yBreak;
                }
            }
            //Create squares where there aren't any in via list.
            return missingSquares;
        }
    }
}
