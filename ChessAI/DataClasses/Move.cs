using System;
using static ChessAI.DataClasses.MoveType;
using static ChessAI.DataClasses.Piece;
using System.Collections.Generic;
using ChessAI.DataClasses;

namespace ChessAI.DataClasses
{
    public readonly struct Move
    {
        public readonly Piece MovePiece;
        public readonly Piece TargetPiece;
        public readonly byte StartPos;
        public readonly byte EndPos;
        public readonly MoveType MoveType;

        /// <summary>
        /// A constructor that takes all the required parameters to define a move
        /// </summary>
        /// <param name="startPos">The starting position of the moving piece</param>
        /// <param name="endPos">The destination of the moving piece</param>
        /// <param name="moveType">The type of move, designating which, if any, special rules are used</param>
        /// <param name="movePiece">The moving piece</param>
        /// <param name="targetPiece">The piece to be captured if any</param>
        private Move(byte startPos, byte endPos, MoveType moveType, Piece movePiece, Piece targetPiece)
        {
            StartPos = startPos;
            EndPos = endPos;
            MoveType = moveType;
            MovePiece = movePiece;
            TargetPiece = targetPiece;
        }

        public override string ToString()
        {
            var move = Board.IndexToString(StartPos) + Board.IndexToString(EndPos);

            move = MoveType switch
            {
                PromotionQueen => "q",
                PromotionRook => "r",
                PromotionBishop => "b",
                PromotionKnight => "k",
                _ => ""
            };

            return move;
        }

        private sealed class MoveEqualityComparer : IEqualityComparer<Move>
        {
            public bool Equals(Move x, Move y)
            {
                return x.MoveType == y.MoveType &&
                       x.StartPos == y.StartPos &&
                       x.EndPos == y.EndPos &&
                       x.MovePiece.Equals(y.MovePiece) &&
                       x.TargetPiece.Equals(y.TargetPiece);
            }

            public int GetHashCode(Move obj)
            {
                return HashCode.Combine(obj.MovePiece, obj.TargetPiece, obj.StartPos, obj.EndPos, (int)obj.MoveType);
            }
        }

        public static IEqualityComparer<Move> MoveComparer { get; } = new MoveEqualityComparer();

        /// <summary>
        /// A factory method that creates a simple move where no special rules are involved.
        /// </summary>
        /// <param name="startPos">The position from which the movePiece originates</param>
        /// <param name="endPos">The destination of the movePiece</param>
        /// <param name="state">The state of the game at the time of the move</param>
        /// <returns>A new move from startPos to endPos</returns>
        public static Move CreateSimpleMove(byte startPos, byte endPos, GameState state)
        {
            var movePiece = state.State[startPos];
            var targetPiece = state.State[endPos];
            var move = new Move(startPos, endPos, Ordinary, movePiece, targetPiece);
            return move;
        }

        /// <summary>
        /// A factory method that creates an en peasant move.
        /// This method performs no checks on the legality of the move, not even if the pieces required are
        /// actually there. This method should therefore only be called after the legality has been determined.
        /// </summary>
        /// <param name="startPos">The position from which the movePiece originates</param>
        /// <param name="enemyPos">The position of the pawn to be captured en peasant</param>
        /// <param name="state">The state of the game at the time of the move</param>
        /// <returns>A new move from startPos to endPos, that also captures the enemy pawn it passes</returns>
        public static Move CreateEnPeasantMove(byte startPos, byte enemyPos, GameState state)
        {
            var movePiece = state.State[startPos];
            var enemyPiece = state.State[enemyPos];
            var enemyDirection = enemyPiece.IsWhite
                ? Board.WhiteDirection(Direction.Up)
                : Board.BlackDirection(Direction.Up);
            var endPos = (byte)(enemyPos - enemyDirection);

            var move = new Move(startPos, endPos, EnPeasant, movePiece, enemyPiece);
            return move;
        }

        /// <summary>
        /// A factory method that creates a castling move.
        /// This method performs no checks on the legality of the move, not even if the pieces required are
        /// actually there. This method should therefore only be called after the legality has been determined.
        /// </summary>
        /// <param name="rookPosition">The position of the rook that is involved in the castling</param>
        /// <param name="state">The state of the game at the time of the move</param>
        /// <returns>
        /// A new castling move that represents the king taking the rookPosition and the
        /// occupying rook being moved in the appropriate direction
        /// </returns>
        public static Move CreateCastleMove(byte rookPosition, GameState state)
        {
            //TODO fix this wrong implementation
            var castleRook = state.State[rookPosition];
            var isWhite = castleRook.IsWhite;

            var kingIndex = Board.StartPositions[isWhite ? (byte)(White | King) : (byte)(Black | King)][0];
            var movePiece = state.State[kingIndex];

            var endPos = (byte)((rookPosition & 0x0f) == 0 ? kingIndex - 2 : kingIndex + 2);

            var move = new Move(kingIndex, endPos, Castling, movePiece, new Piece(Empty));
            return move;
        }

        /// <summary>
        /// A factory method that creates a pwn promotion move.
        /// This method performs no checks on the legality of the move, not even if the pieces required are
        /// actually there. This method should therefore only be called after the legality has been determined. 
        /// </summary>
        /// <param name="startPosition">The starting position of the pawn</param>
        /// <param name="endPos">
        /// The destination of the pawn. note that this may also occur through an attack
        /// which opens the possibility of three different end positions
        /// </param>
        /// <param name="promotionPiece">The piece the pawn should be promoted to</param>
        /// <param name="state">The state of the game at the time of the move</param>
        /// <returns>
        /// A new pawn promotion move that represents a pawn moving to the 8th rank, being promote to the promotionPiece
        /// </returns>
        public static Move CreatePawnPromotionMove(byte startPosition, byte endPos, Piece promotionPiece,
            GameState state)
        {
            var movePiece = state.State[startPosition];
            var promotionType = promotionPiece.PieceType switch
            {
                Queen => PromotionQueen,
                Knight => PromotionKnight,
                Bishop => PromotionBishop,
                Rook => PromotionRook,
                _ => throw new ArgumentOutOfRangeException(
                    $"The piece type {promotionPiece.PieceType} is not an option for promotion"
                )
            };

            var move = new Move(startPosition, endPos, promotionType, movePiece, promotionPiece);
            return move;
        }

        public static Move Parse(string moveString, GameState state)
        {
            //TODO implement more exhaustive analysis of moveString
            moveString = moveString.ToLower();

            var startPos = Board.StringToIndex(moveString.Substring(0, 2));
            var endPos = Board.StringToIndex(moveString.Substring(2, 2));
            var movePiece = state.State[startPos];

            if (moveString.Length > 4)
            {
                var color = movePiece.IsWhite ? White : Black;
                var promotionPiece = moveString[4] switch
                {
                    'q' => new Piece(color | Queen, endPos),
                    'k' => new Piece(color | Knight, endPos),
                    'r' => new Piece(color | Rook, endPos),
                    'b' => new Piece(color | Bishop, endPos),
                    _ => throw new ArgumentException(
                        $"Move: ({moveString}) could not be parsed due to letter '{moveString[4]}'")
                };
                return CreatePawnPromotionMove(startPos, endPos, promotionPiece, state);
            }

            if (movePiece.PieceType == King)
            {
                if (Math.Abs(startPos - endPos) == 2)
                {
                    byte castlePos;
                    if ((startPos - endPos) > 0)
                    {
                        castlePos = (byte)(endPos - 2);
                    }
                    else
                    {
                        castlePos = (byte)(endPos + 1);
                    } //TODO Validate that this is correct calculation

                    return CreateCastleMove(castlePos, state);
                }
            }

            var previousMove = state.PreviousMove;
            if (previousMove.MovePiece.PieceType == Pawn)
            {
                if (((previousMove.StartPos - previousMove.EndPos) / 0x10) == 2)
                {
                    if (movePiece.PieceType == Pawn)
                    {
                        if (Math.Abs(startPos - previousMove.EndPos) == 1)
                        {
                            if (Math.Abs(endPos - previousMove.EndPos) == 0x10)
                            {
                                //TODO validate that this is always an en peasant move and cannot be any other
                                return CreateEnPeasantMove(startPos, previousMove.EndPos, state);
                            }
                        }
                    }
                }
            }

            return CreateSimpleMove(startPos, endPos, state);
        }
    }

    public enum MoveType : byte
    {
        Ordinary,
        EnPeasant,
        Castling,
        PromotionQueen,
        PromotionRook,
        PromotionBishop,
        PromotionKnight
    }
}