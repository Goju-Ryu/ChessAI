using System.Collections.Generic;
using ChessAI.DataClasses;
using static ChessAI.DataClasses.Piece;
using NUnit.Framework;

namespace UnitTests.DataClasses
{
    public class MoveTests
    {
        [Test]
        public void CastlingParseTest()
        {
            var pieces = new List<Piece>(new[]
            {
                new Piece(White | Rook, 0x00), new Piece(White | King, Board.StartPositions[White | King][0])
            });
            var board = new Board(pieces);
            var state = new GameState(board, true);
            
            var castlingMove = Move.CreateCastleMove(0, state);
            
            Assert.AreEqual(MoveType.Castling, castlingMove.MoveType);
            Assert.AreEqual(0x04, castlingMove.StartPos);
            Assert.AreEqual(castlingMove.StartPos, castlingMove.MovePiece.Position);
            Assert.AreEqual(White | King, castlingMove.MovePiece.ColorAndType);
            Assert.AreEqual(0x02, castlingMove.EndPos);
            Assert.AreEqual(Empty, castlingMove.TargetPiece.PieceType);
            
            var castleMoveString = Board.IndexToString(castlingMove.StartPos) + Board.IndexToString(castlingMove.EndPos);

            castleMoveString += castlingMove.MoveType switch
            {
                MoveType.PromotionQueen => "q",
                MoveType.PromotionKnight => "k",
                MoveType.PromotionBishop => "b",
                MoveType.PromotionRook => "r",
                _ => ""
            };

            var parsedMove = Move.Parse(castleMoveString, state);
            
            Assert.AreEqual(MoveType.Castling, parsedMove.MoveType);
            Assert.AreEqual(0x04, parsedMove.StartPos);
            Assert.AreEqual(parsedMove.StartPos, parsedMove.MovePiece.Position);
            Assert.AreEqual(White | King, parsedMove.MovePiece.ColorAndType);
            Assert.AreEqual(0x02, parsedMove.EndPos);
            Assert.AreEqual(Empty, parsedMove.TargetPiece.PieceType);
            
            Assert.AreEqual(castlingMove, parsedMove);
        }
    }
}