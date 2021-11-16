using ChessAI.DataClasses;
using NUnit.Framework;

namespace UnitTests.DataClasses
{
    public class BoardTests
    {
        [Test]
        public void IsIndexValidTest()
        {
            for (byte i = 0; i < 0x10; i++)
            {
                for (byte j = 0; j < 0x10; j++)
                {
                    
                    var index = (byte) (( i * 0x10 ) + j); // converts i and j to a combined index that should be correct
                    if (i < 8 && j < 8)
                    {
                        Assert.IsTrue(Board.IsIndexValid(index));
                    }
                    else
                    {
                        Assert.IsFalse(Board.IsIndexValid(index));
                    }
                }
            }
        }
    }
}