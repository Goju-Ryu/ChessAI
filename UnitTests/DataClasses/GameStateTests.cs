using System;
using System.Linq;
using ChessAI.DataClasses;
using static ChessAI.DataClasses.Piece;
using NUnit.Framework;


namespace UnitTests.DataClasses
{
    public class GameStateTests
    {
        private readonly Piece _rook = new Piece(White | Rook, 0x00);
        private readonly Piece _pawn = new Piece(White | Pawn, 0x02);
        private readonly Piece _queen = new Piece(Black | Queen, 0x20);
        
        [Test]
        public void GameStateConstructorTest()
        {
            var pieces = new[] { _rook, _pawn, _queen}.ToList();

            var fields = new Piece[0x80];
            fields[_rook.Position] = _rook;
            fields[_pawn.Position] = _pawn;
            fields[_queen.Position] = _queen;

            var boardFromPieces = new Board(pieces);
            var boardFromFields = new Board(fields);

            var stateFromPieces = new GameState(boardFromPieces);
            var stateFromFields = new GameState(boardFromFields);
            
           AssertStatesDeepEqual(stateFromFields, stateFromPieces);
           
           Assert.AreEqual(1, stateFromFields.BlackPieces.Length);
           Assert.Contains(_queen, stateFromFields.BlackPieces);
           Assert.Contains(_queen, stateFromPieces.BlackPieces);
           
           Assert.AreEqual(2, stateFromPieces.WhitePieces.Length);
           Assert.Contains(_rook, stateFromFields.WhitePieces);
           Assert.Contains(_rook, stateFromPieces.WhitePieces);
           Assert.Contains(_pawn, stateFromFields.WhitePieces);
           Assert.Contains(_pawn, stateFromPieces.WhitePieces);
        }

        [Test]
        public void ApplyMoveTest()
        {
            var state0 = new GameState(new Board(new[] { _rook, _pawn, _queen }.ToList()));

            var move1 = new Move(0x20, 0x22);
            var state1 = state0.ApplyMove(move1);

            var move2 = new Move(move1.EndPos, move1.StartPos);
            var state2 = state1.ApplyMove(move2);
            
            AssertStatesDeepEqual(state0, state2);

            var move3 = new Move(move2.EndPos, _rook.Position);
            var state3 = state2.ApplyMove(move3);
            
            Assert.AreEqual(1, state3.WhitePieces.Length);
            Assert.Contains(_pawn, state3.WhitePieces);
        }


        private void AssertStatesDeepEqual(GameState state1, GameState state2)
        {
            //Test piece lists
            Assert.AreEqual(state1.BlackPieces.Length, state2.BlackPieces.Length);
            Assert.AreEqual(state1.WhitePieces.Length, state2.WhitePieces.Length);
            
            //Test that the same board was constructed
            Assert.IsTrue(state1.State.Fields.SequenceEqual(state2.State.Fields));
        }
    }
}