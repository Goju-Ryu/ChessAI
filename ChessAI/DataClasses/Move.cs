using System;
using static ChessAI.DataClasses.MoveType;
using static ChessAI.DataClasses.Piece;

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
        /// a simple constructor taking the start and end position.
        /// </summary>
        /// <param name="startPos">starting position</param>
        /// <param name="endPos">destination</param>
        /// <remarks>
        /// This constructor has only been left here for compatibility concerns and should not be used.
        /// When encountered it should be replaced by a factory method call.
        /// The <see cref="CreateSimpleMove"/> Factory is the recommended alternative. It makes sure to set all the
        /// required fields in the correct way only given start and end position and a state.
        /// </remarks>
        [ObsoleteAttribute("When encountered it should be replaced by a factory method call."
                           + "The CreateSimpleMove Factory is the recommended alternative.", false)]
        public Move(byte startPos, byte endPos)
        {
            StartPos = startPos;
            EndPos = endPos;

            MoveType = Ordinary;
            MovePiece = new Piece(Empty);
            TargetPiece = MovePiece;
        }

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
            var enemyDirection = (enemyPiece.PieceFlags & White) == White
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
        /// <param name="castlePosition">The position of the rook that is involved in the castling</param>
        /// <param name="state">The state of the game at the time of the move</param>
        /// <returns>
        /// A new castling move that represents the king taking the castlePosition and the
        /// occupying rook being moved in the appropriate direction
        /// </returns>
        public static Move CreateCastleMove(byte castlePosition, GameState state)
        {
            var targetPiece = state.State[castlePosition];
            
            var kingIndex = Board.StartPositions[targetPiece.ColorAndType][0];
            var movePiece = state.State[kingIndex];

            var move = new Move(kingIndex, castlePosition, Castling, movePiece, new Piece(Empty));
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

            var startPos = Board.StringToIndex(moveString.Substring(0, 2));
            var endPos = Board.StringToIndex(moveString.Substring(2, 2)); //TODO this returns wrong index

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