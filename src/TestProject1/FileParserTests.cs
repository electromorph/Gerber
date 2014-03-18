using GerberLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Moq;
using System.Collections.Generic;

namespace UnitTests
{
        
    /// <summary>
    ///This is a test class for FileParserNewTest and is intended
    ///to contain all FileParserNewTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FileParserTests
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private void AssertEqualForQueue(Queue<string> expected, Queue<string> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            IEnumerator<string> e1 = expected.GetEnumerator();
            IEnumerator<string> e2 = actual.GetEnumerator();
            while (e1.MoveNext() && e2.MoveNext())
            {
                Assert.AreEqual(e1.Current, e2.Current);
            }
        }

        [TestMethod]
        public void AddOneTest()
        {
            int startInt = 1;
            var fileParser = new FileParser();
            var actual = fileParser.AddOne(startInt);
            Assert.AreEqual(startInt+1, actual);
        }


        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_CommandsButNoParameters()
        {
            var input = new Queue<string>();
            input.Enqueue("11111*22222*33333");
            input.Enqueue("AAAAA*BBBBB*CCCCC");

            var expected = new Queue<string>();
            expected.Enqueue("11111*22222*33333");
            expected.Enqueue("AAAAA*BBBBB*CCCCC");

            var fileParser = new FileParser();
            var actual = fileParser.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_EmptyFile()
        {
            // EmptyFile ==> No output
            var input = new Queue<string>();
            var expected = new Queue<string>();

            var fileParser = new FileParser();
            var actual = fileParser.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_MultipleParametersOnOneLine()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA%%BBBBB%%CCCCC%");
            
            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA%");
            expected.Enqueue("%BBBBB%");
            expected.Enqueue("%CCCCC%");
            
            var fileParser = new FileParser();
            var actual = fileParser.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }


        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_DifferentNumbersOfParametersOnDifferentLines()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA*%");
            input.Enqueue("%BBBBB*CCCCC%");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA*%");
            expected.Enqueue("%BBBBB*CCCCC%");
            
            var fileParser = new FileParser();
            var actual = fileParser.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_PercentBracesSpanMoreThanOneLine()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA*");
            input.Enqueue("BBBBB*CCCCC%");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA*BBBBB*CCCCC%");
            
            var fileParser = new FileParser();
            var actual = fileParser.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_OrphanPercentAtEOF()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA%");
            input.Enqueue("BBBBB%");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA%");
            expected.Enqueue("BBBBB");

            var fileParser = new FileParser();
            var actual = fileParser.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_ParameterSplitAcrossLines()
        {
            var input = new Queue<string>();
            input.Enqueue("AAAAA%BBB");
            input.Enqueue("BB%");

            var expected = new Queue<string>();
            expected.Enqueue("AAAAA");
            expected.Enqueue("%BBBBB%");
            
            var fileParser = new FileParser();
            var actual = fileParser.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void GroupContentsOfPercentBracesOnIndividualLinesTest_OrphanPercentInMiddleOfFile()
        {
            var input = new Queue<string>();
            input.Enqueue("AAAAA%BBB*");
            input.Enqueue("CCCCC");

            var expected = new Queue<string>();
            expected.Enqueue("AAAAA");
            expected.Enqueue("BBB*CCCCC");
            
            var target = new FileParser();
            var actual = target.GroupContentsOfPercentBracesOnIndividualLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitMultiLinesInPercentBracesIntoSingleLinesTest_OneParameterOnOneLine()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA%");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA%");

            var fileParser = new FileParser();
            var actual = fileParser.SplitMultiLinesInPercentBracesIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitMultiLinesInPercentBracesIntoSingleLinesTest_OneParameterOnOneLineWithAsteriskAtEnd()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA*%");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA%");
            
            var fileParser = new FileParser();
            var actual = fileParser.SplitMultiLinesInPercentBracesIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitMultiLinesInPercentBracesIntoSingleLinesTest_ThreeAlphaParametersOnOneLine()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA*BBBBB*CCCCC%");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA%");
            expected.Enqueue("%BBBBB%");
            expected.Enqueue("%CCCCC%");
            
            var fileParser = new FileParser();
            var actual = fileParser.SplitMultiLinesInPercentBracesIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitMultiLinesInPercentBracesIntoSingleLinesTest_ParameterWithoutPercentAtEnd()
        {
            var input = new Queue<string>();
            input.Enqueue("%AAAAA");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA%");

            var fileParser = new FileParser();
            var actual = fileParser.SplitMultiLinesInPercentBracesIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitMultiLinesInPercentBracesIntoSingleLinesTest_MixOfCommandsAndParameters()
        {
            Queue<string> input = new Queue<string>();
            input.Enqueue("AAAAA*");
            input.Enqueue("%BBBBB*CCCCC%");
            input.Enqueue("DDDDD*");

            Queue<string> expected = new Queue<string>();
            expected.Enqueue("AAAAA*");
            expected.Enqueue("%BBBBB%");
            expected.Enqueue("%CCCCC%");
            expected.Enqueue("DDDDD*");

            var fileParser = new FileParser();
            var actual = fileParser.SplitMultiLinesInPercentBracesIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitMultiLinesInPercentBracesIntoSingleLinesTest_ParametersFollowedByNumbersAreTreatedAsOneParameter()
        {
            // Testing that lines followed by numbers are treated as one command. (for AM - aperture macros).
            var input = new Queue<string>();

            input.Enqueue("%AAAAA%");
            input.Enqueue("%BBBBB*11,111,11%");
            input.Enqueue("%CCCCC*");
            input.Enqueue("22%");
            input.Enqueue("%DDDDD%");

            var expected = new Queue<string>();
            expected.Enqueue("%AAAAA%");
            expected.Enqueue("%BBBBB*11,111,11%");
            expected.Enqueue("%CCCCC*22%");
            expected.Enqueue("%DDDDD%");

            var fileParser = new FileParser();
            var actual = fileParser.SplitMultiLinesInPercentBracesIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitCommandsIntoSingleLinesTest_OneCommandWithAsterisk()
        {
            // AAAAA* ==> AAAAA
            var input = new Queue<string>();
            input.Enqueue("AAAAA*");

            var expected = new Queue<string>();
            expected.Enqueue("AAAAA");
            
            var fileParser = new FileParser();
            var actual = fileParser.SplitCommandsIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitCommandsIntoSingleLinesTest_OneCommandWithoutAsterisk()
        {
            // AAAAA ==> AAAAA
            var input = new Queue<string>();
            input.Enqueue("AAAAA");

            var expected = new Queue<string>();
            expected.Enqueue("AAAAA");

            var fileParser = new FileParser();
            var actual = fileParser.SplitCommandsIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitCommandsIntoSingleLinesTest_SeveralCommandsWithAsterisksOnDifferentLines()
        {
            // AAAAA*<nl>BBBBB*<nl>CCCCC* ==> AAAAA<nl>BBBBB<nl>CCCCC
            Queue<string> input = new Queue<string>();
            input.Enqueue("AAAAA*");
            input.Enqueue("BBBBB*");
            input.Enqueue("CCCCC*");
            input.Enqueue("DDDDD*");

            Queue<string> expected = new Queue<string>();
            expected.Enqueue("AAAAA");
            expected.Enqueue("BBBBB");
            expected.Enqueue("CCCCC");
            expected.Enqueue("DDDDD");
         
            var fileParser = new FileParser();
            var actual = fileParser.SplitCommandsIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitCommandsIntoSingleLinesTest_MixOfCommandsAndParameters()
        {
            // Mix of commands and parameters.
            var input = new Queue<string>();
            input.Enqueue("AAAAA*BBBBB");
            input.Enqueue("%CCCCC%");
            input.Enqueue("DDDDD*EEEEE*");

            var expected = new Queue<string>();
            expected.Enqueue("AAAAA");
            expected.Enqueue("BBBBB");
            expected.Enqueue("%CCCCC%");
            expected.Enqueue("DDDDD");
            expected.Enqueue("EEEEE");
            
            var fileParser = new FileParser();
            var actual = fileParser.SplitCommandsIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }

        [TestMethod]
        public void SplitCommandsIntoSingleLinesTest_MultilineCommands()
        {
            // One with an asterisk at end, and the second without.
            
            var input = new Queue<string>();
            input.Enqueue("AAAAA");
            input.Enqueue("BBBBB");
            input.Enqueue("CCCCC*");
            input.Enqueue("DDDDD");
            input.Enqueue("EEEEE");

            var expected = new Queue<string>();
            expected.Enqueue("AAAAABBBBBCCCCC");
            expected.Enqueue("DDDDDEEEEE");

            var fileParser = new FileParser();
            var actual = fileParser.SplitCommandsIntoSingleLines(input);

            AssertEqualForQueue(expected, actual);
        }
    }
}
