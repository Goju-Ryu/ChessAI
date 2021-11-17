using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ChessAI.DataClasses;
using ChessAI.MoveSelection.MoveGeneration;

namespace UnitTests.MoveSelection.MoveGeneration {
    public class MoveCalculaterTest {
        
        [Test]
        public void DoesDiagonalMovesCalculateCorrect_NoObstructions(){


            List<(byte, Piece)> pieces = new List<(byte, Piece)>();
            pieces.Add( ( 0x00 , (Piece.Bishop) ) );
            Board b = new Board( pieces );
            GameState state = new GameState(b);

            MoveGenerator MG = new MoveGenerator();
            List<Move> moves = MG.CalculatePossibleMoves(state, true);

            Console.WriteLine( moves.Count );

            Assert.IsTrue(false);
        }





    }
}
