using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using Visualization;

namespace TestProject1
{
    [TestClass]
    public class VisualizationTests
    {
        private void AssertEqualForSquaresList(List<Square> expected, List<Square> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            IEnumerator<Square> e1 = expected.GetEnumerator();
            IEnumerator<Square> e2 = actual.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext())
            {
                Assert.AreEqual(e1.Current, e2.Current);
            }
        }

        //[TestMethod]
        //public void TurnViasIntoSquares()
        //{
        //    //Refer to attached bitmap for picture of what this is testing.
        //    var input = new List<Via>();
        //    input.Add(new Via(new Point(2.5, 1.5), 1));
        //    input.Add(new Via(new Point(2.0, 3.5), 2));

        //    var expected = new List<Square>();
        //    expected.Add(new Square(new Point(0, 0), new Point(5, 1)));
        //    expected.Add(new Square(new Point(0, 1), new Point(2, 2)));
        //    expected.Add(new Square(new Point(3, 1), new Point(5, 2)));
        //    expected.Add(new Square(new Point(0, 2), new Point(5, 3)));
        //    expected.Add(new Square(new Point(0, 3), new Point(1, 4)));
        //    expected.Add(new Square(new Point(4, 3), new Point(5, 4)));
        //    expected.Add(new Square(new Point(0, 4), new Point(5, 5)));

        //    var actual = BoardMaker.TurnViasIntoSquares(input);

        //    AssertEqualForSquaresList(expected, actual);
        //}

        [TestMethod]
        public void BoardMaker_FillInMissingSquares_Test1()
        {
            //Refer to attached bitmap for picture of what this is testing.
            var vias = new List<Square>();
            var boardOutline = new Square(new Point(0, 0), new Point(5, 5));

            vias.Add(new Square(new Point(2, 1), new Point(3, 2)));
            vias.Add(new Square(new Point(1, 3), new Point(4, 4)));

            var expected = new List<Square>();
            expected.Add(new Square(new Point(0, 0), new Point(1, 1)));
            expected.Add(new Square(new Point(1, 0), new Point(2, 1)));
            expected.Add(new Square(new Point(2, 0), new Point(3, 1)));
            expected.Add(new Square(new Point(3, 0), new Point(4, 1)));
            expected.Add(new Square(new Point(4, 0), new Point(5, 1)));
            expected.Add(new Square(new Point(0, 1), new Point(1, 2)));
            expected.Add(new Square(new Point(1, 1), new Point(2, 2)));
            expected.Add(new Square(new Point(3, 1), new Point(4, 2)));
            expected.Add(new Square(new Point(4, 1), new Point(5, 2)));
            expected.Add(new Square(new Point(0, 2), new Point(1, 3)));
            expected.Add(new Square(new Point(1, 2), new Point(2, 3)));
            expected.Add(new Square(new Point(2, 2), new Point(3, 3)));
            expected.Add(new Square(new Point(3, 2), new Point(4, 3)));
            expected.Add(new Square(new Point(4, 2), new Point(5, 3)));
            expected.Add(new Square(new Point(0, 3), new Point(1, 4)));
            expected.Add(new Square(new Point(4, 3), new Point(5, 4)));
            expected.Add(new Square(new Point(0, 4), new Point(1, 5)));
            expected.Add(new Square(new Point(1, 4), new Point(2, 5)));
            expected.Add(new Square(new Point(2, 4), new Point(3, 5)));
            expected.Add(new Square(new Point(3, 4), new Point(4, 5)));
            expected.Add(new Square(new Point(4, 4), new Point(5, 5)));

            var actual = BoardMaker.GenerateMissingSquares(vias, boardOutline);
                        
            AssertEqualForSquaresList(expected, actual);
        }
    }
}
