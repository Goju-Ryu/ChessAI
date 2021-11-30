using System;
using System.Collections.Generic;
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
            var pieces = new[] { _rook, _pawn, _queen }.ToList();

            var fields = new Piece[0x80];
            fields[_rook.Position] = _rook;
            fields[_pawn.Position] = _pawn;
            fields[_queen.Position] = _queen;

            var boardFromPieces = new Board(pieces);
            var boardFromFields = new Board(fields);

            var stateFromPieces = new GameState(boardFromPieces, false);
            var stateFromFields = new GameState(boardFromFields, false);

            AssertStatesDeepEqual(stateFromFields, stateFromPieces);

            Assert.AreEqual(1, stateFromFields.BlackPieces.Length);
            Assert.Contains(_queen, stateFromFields.BlackPieces.Pieces.ToArray());
            Assert.Contains(_queen, stateFromPieces.BlackPieces.Pieces.ToArray());

            Assert.AreEqual(2, stateFromPieces.WhitePieces.Length);
            Assert.Contains(_rook, stateFromFields.WhitePieces.Pieces.ToArray());
            Assert.Contains(_rook, stateFromPieces.WhitePieces.Pieces.ToArray());
            Assert.Contains(_pawn, stateFromFields.WhitePieces.Pieces.ToArray());
            Assert.Contains(_pawn, stateFromPieces.WhitePieces.Pieces.ToArray());
        }

        [Test]
        public void CreateNewGameTest()
        {
            var state = GameState.CreateNewGameState(false);

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(White | Pawn, state.State[0x10 + i].ColorAndType);
                Assert.AreEqual(Black | Pawn, state.State[0x60 + i].ColorAndType);
            }

            var whitePieceOrder = new[]
            {
                new Piece(White | Rook, 0x00),
                new Piece(White | Knight, 0x01),
                new Piece(White | Bishop, 0x02),
                new Piece(White | Queen, 0x03),
                new Piece(White | King, 0x04),
                new Piece(White | Bishop, 0x05),
                new Piece(White | Knight, 0x06),
                new Piece(White | Rook, 0x07),
            };
            var blackPieceOrder = new[]
            {
                new Piece(Black | Rook, 0x70),
                new Piece(Black | Knight, 0x71),
                new Piece(Black | Bishop, 0x72),
                new Piece(Black | Queen, 0x73),
                new Piece(Black | King, 0x74),
                new Piece(Black | Bishop, 0x75),
                new Piece(Black | Knight, 0x76),
                new Piece(Black | Rook, 0x77),
            };

            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(whitePieceOrder[i].ColorAndType, state.State[0x00 + i].ColorAndType);
                Assert.AreEqual(blackPieceOrder[i].ColorAndType, state.State[0x70 + i].ColorAndType);
            }
        }

        [Test]
        public void ApplyOrdinaryMoveTest()
        {
            var state0 = new GameState(new Board(new[] { _rook, _pawn, _queen }.ToList()), false);

            var move1 = Move.CreateSimpleMove(0x20, 0x22, state0);
            var state1 = state0.ApplyMove(move1);

            var move2 = Move.CreateSimpleMove(move1.EndPos, move1.StartPos, state1);
            var state2 = state1.ApplyMove(move2);

            AssertStatesDeepEqual(state0, state2);

            var move3 = Move.CreateSimpleMove(move2.EndPos, _rook.Position, state2);
            var state3 = state2.ApplyMove(move3);

            Assert.AreEqual(1, state3.WhitePieces.Length);
            Assert.Contains(_pawn, state3.WhitePieces.Pieces.ToArray());
        }

        [Test]
        public void ApplyCastlingMoveTest()
        {
            var pieces = new List<Piece>[]
            {
                new(new[]
                    {
                        new Piece(White | Rook, 0x00), new Piece(White | King, Board.StartPositions[White | King][0])
                    }
                ),
                new(new[]
                {
                    new Piece(White | Rook, 0x07), new Piece(White | King, Board.StartPositions[White | King][0])
                }),
                new(new[]
                    {
                        new Piece(Black | Rook, 0x70), new Piece(Black | King, Board.StartPositions[Black | King][0])
                    }
                ),
                new(new[]
                {
                    new Piece(Black | Rook, 0x77), new Piece(Black | King, Board.StartPositions[Black | King][0])
                })
            };


            AssertCastlingCorrectly(pieces[0],
                new GameState(new Board(new List<Piece>(new[]
                {
                    new Piece(White | King, 0x02),
                    new Piece(White | Rook, 0x03)
                })), false)
            );
            AssertCastlingCorrectly(pieces[1],
                new GameState(new Board(new List<Piece>(new[]
                {
                    new Piece(White | King, 0x06),
                    new Piece(White | Rook, 0x05)
                })), false)
            );
            AssertCastlingCorrectly(pieces[2],
                new GameState(new Board(new List<Piece>(new[]
                {
                    new Piece(Black | King, 0x72),
                    new Piece(Black | Rook, 0x73)
                })), false)
            );
            AssertCastlingCorrectly(pieces[3],
                new GameState(new Board(new List<Piece>(new[]
                {
                    new Piece(Black | King, 0x76),
                    new Piece(Black | Rook, 0x75)
                })), false)
            );
        }

        private void AssertCastlingCorrectly(List<Piece> pieces, in GameState expectedResult)
        {
            var board = new Board(pieces);
            var stateWhite = new GameState(board, true);
            var castlingAsWhite = Move.CreateCastleMove(pieces.Find((piece => piece.PieceType == Rook)).Position, stateWhite);
            var newStateWhite = stateWhite.ApplyMove(castlingAsWhite);
            if (pieces[0].IsWhite)
            {
                 Assert.IsFalse(newStateWhite.CanARankRookCastle || newStateWhite.CanHRankRookCastle);
            }
            else
            {
                Assert.IsTrue(newStateWhite.CanARankRookCastle || newStateWhite.CanHRankRookCastle);
            }
            AssertStatesDeepEqual(newStateWhite, expectedResult);


            var stateBlack = new GameState(board, false);
            var castlingAsBlack = Move.CreateCastleMove(pieces.Find((piece => piece.PieceType == Rook)).Position, stateBlack);
            var newStateBlack = stateBlack.ApplyMove(castlingAsBlack);
            if (pieces[0].IsWhite)
            {
                Assert.IsTrue(newStateBlack.CanARankRookCastle || newStateBlack.CanHRankRookCastle);
            }
            else
            {
                Assert.IsFalse(newStateBlack.CanARankRookCastle || newStateBlack.CanHRankRookCastle);
            }
            AssertStatesDeepEqual(newStateBlack, expectedResult);
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