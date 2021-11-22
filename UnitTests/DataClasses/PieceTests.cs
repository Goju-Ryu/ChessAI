using ChessAI.DataClasses;
using NUnit.Framework;
using System;
using static ChessAI.DataClasses.Piece;

namespace UnitTests.DataClasses
{
    public class PieceTests
    {
        [Test]
        public void CreatePiece()
        {
            Piece piece = new Piece( White | Bishop );

            var expected = 0b1000 | Bishop;
            Assert.IsTrue(expected == piece);
        }

        [Test]
        public void PieceToString()
        {
            var black = "Black";
            var white = "White";
            var pieces = new[]
            {
                "None",
                "Pawn",
                "Rook",
                "Knight",
                "Bishop",
                "Queen",
                "King",
            };
            
            
            for (byte i = 1; i < 6; i++)
            {
                Assert.AreEqual(white + " " + pieces[i], new Piece(White | i).ToString());
                Assert.AreEqual(black + " " + pieces[i], new Piece(Black | i).ToString());
            }
        }
    }
}