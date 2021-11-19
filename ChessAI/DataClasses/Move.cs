using System;
using static ChessAI.DataClasses.Piece;

namespace ChessAI.DataClasses
{
    public readonly struct Move
    {
        public readonly byte StartPos;
        public readonly byte EndPos;
        public readonly bool IsSpecialMove; //TODO consider using an enum to describe the type of move instead
        public readonly Piece MovePiece;
        public readonly Piece TargetPiece;


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

            IsSpecialMove = false;
            MovePiece = new Piece(Empty);
            TargetPiece = MovePiece;
        }

        /// <summary>
        /// A constructor that takes all the required parameters to define a move
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="endPos"></param>
        /// <param name="isSpecialMove"></param>
        /// <param name="movePiece"></param>
        /// <param name="targetPiece"></param>
        private Move(byte startPos, byte endPos, bool isSpecialMove, Piece movePiece, Piece targetPiece)
        {
            StartPos = startPos;
            EndPos = endPos;
            IsSpecialMove = isSpecialMove;
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
            var move = new Move(startPos, endPos, false, movePiece, targetPiece);
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

            // Dependant on the board. Should this be a static member of board?
            byte whiteKingIndex = 0x04;
            byte blackKingIndex = 0x73;

            var kingIndex = (targetPiece.PieceFlags & White) == White ? whiteKingIndex : blackKingIndex;
            var movePiece = state.State[kingIndex];

            var move = new Move(kingIndex, castlePosition, true, movePiece, targetPiece);
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
            var move = new Move(startPosition, endPos, true, movePiece, promotionPiece);
            return move;
        }
    }
}