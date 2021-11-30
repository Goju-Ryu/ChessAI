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
            var castlingPositions = new[]
            {
                (new Piece(White | King, 0x04), new Piece(White | Rook, 0x00)),
                (new Piece(White | King, 0x04), new Piece(White | Rook, 0x07)),
                (new Piece(Black | King, 0x74), new Piece(Black | Rook, 0x70)),
                (new Piece(Black | King, 0x74), new Piece(Black | Rook, 0x77))
            };

            foreach ((Piece king, Piece rook) in castlingPositions)
            {
                AssertCastlingParsesCorrectly(king, rook);
            }
        }
        
        
        private void AssertCastlingParsesCorrectly(Piece king, Piece rook)
        {
            var pieces = new List<Piece>(new[]
            {
                king, rook
            });
            
            var board = new Board(pieces);
            var state = new GameState(board, true);
            
            var castlingMove = Move.CreateCastleMove(rook.Position, state);
            var expectedEndPos = (rook.Position & 0x0F) == 0 ? rook.Position + 2 : rook.Position - 1;
            
            Assert.AreEqual(MoveType.Castling, castlingMove.MoveType);
            Assert.AreEqual(king.Position, castlingMove.StartPos);
            Assert.AreEqual(castlingMove.StartPos, castlingMove.MovePiece.Position);
            Assert.AreEqual(king.ColorAndType, castlingMove.MovePiece.ColorAndType);
            Assert.AreEqual(expectedEndPos, castlingMove.EndPos);
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
            Assert.AreEqual(king.Position, parsedMove.StartPos);
            Assert.AreEqual(parsedMove.StartPos, parsedMove.MovePiece.Position);
            Assert.AreEqual(king.ColorAndType, parsedMove.MovePiece.ColorAndType);
            Assert.AreEqual(expectedEndPos, parsedMove.EndPos);
            Assert.AreEqual(Empty, parsedMove.TargetPiece.PieceType);
            
            Assert.AreEqual(castlingMove, parsedMove);
        }
    }
}