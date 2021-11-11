using System.Collections.Generic;
using ChessAI.DataClasses;
using NUnit.Framework;
using static ChessAI.DataClasses.FlagPiece;

namespace UnitTests.DataClasses
{
    public class PieceTests
    {
        [Test]
        public void CreatePiece()
        {
            FlagPiece piece = White | Bishop;
            
            Assert.AreEqual((0b0100 | (byte)Bishop), piece);
        }

        [Test]
        public void PieceToString()
        {
            var blackPieces = new List<FlagPiece>();
            for (byte i = 1; i < 6; i++)
            {
                blackPieces.Add(Black | (FlagPiece)i);
            }
            
            
        } 
    }
}