using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ChessAI.DataClasses;
using ChessAI.MoveSelection.StateAnalysis;
using NUnit.Framework;

namespace UnitTests.MoveSelection.StateAnalysis
{
    public class StateAnalyserSimpleTest : StateAnalyserSimple
    {
        

        [Test]
        public void KingPointConsistencyTest()
        {
            PositionPointConsistencyTest(KingPositionPoints);
            IndexTranslationTest(KingPositionPoints);
        }
        
        [Test]
        public void QueenPointConsistencyTest()
        {
            PositionPointConsistencyTest(QueenPositionPoints);
            IndexTranslationTest(QueenPositionPoints);
        }
        
        [Test]
        public void RookPointConsistencyTest()
        {
            PositionPointConsistencyTest(RookPositionPoints);
            IndexTranslationTest(RookPositionPoints);
        }
        
        [Test]
        public void BishopPointConsistencyTest()
        {
            PositionPointConsistencyTest(BishopPositionPoints);
            IndexTranslationTest(BishopPositionPoints);
        }
        
        [Test]
        public void KnightPointConsistencyTest()
        {
            PositionPointConsistencyTest(KnightPositionPoints);
            IndexTranslationTest(KnightPositionPoints);
        }
        
        [Test]
        public void PawnPointConsistencyTest()
        {
            PositionPointConsistencyTest(PawnPositionPoints);
            IndexTranslationTest(PawnPositionPoints);
        }
        
        
        
        private static void PositionPointConsistencyTest(int[] positionPoints)
        {
            var whitePoints = new List<int>();
            var blackPoints = new List<int>();

            for (byte i = 0; i < positionPoints.Length; i++)
            {
                if (!Board.IsIndexValid(i)) continue;
                
                whitePoints.Add(positionPoints[i]);
                blackPoints.Add(positionPoints[^(i + 1)]); // Indexed from the end towards the start. Essentially ^(i + 1) == Length - (i + 1)
            }
            
            Assert.AreEqual(whitePoints.Count, blackPoints.Count);

            for (int i = 0; i < whitePoints.Count; i++)
            {
                Assert.AreEqual(whitePoints[i], blackPoints[i]);
            }
            
        }
        
        private static void IndexTranslationTest(int[] positionPoints)
        {
            for (byte i = 0; i < positionPoints.Length; i++)
            {
                if (!Board.IsIndexValid(i)) continue;
               
                Assert.DoesNotThrow(() => positionPoints.GetValue(i), "Invalid index ({0:X}) for white", i);
                Assert.DoesNotThrow(() => positionPoints.GetValue(i + 8), "Invalid index ({0:X}) for black", i);
            }
        }
    }
}