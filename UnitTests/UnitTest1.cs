using ChessAI;
using NUnit.Framework;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PassingTest()
        {
            Assert.AreEqual(4, ExampleClass.Add(2,2));
        }

        [Test]
        public void FailingTest()
        {
            Assert.AreEqual(4, ExampleClass.Minus(2, 2));
        }
    }
}