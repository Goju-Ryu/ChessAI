using System;
using System.Collections.Generic;
using static ChessAI.DataClasses.Piece;

namespace ChessAI.DataClasses
{
    /**
     * <summary>
     * A struct that keeps track of all relevant information about a game
     * </summary>
     */

    public readonly partial struct GameState
    {
        public readonly bool[] hasWhiteTowersMoved;
        public readonly bool[] hasBlackTowersMoved;

        /// <summary>
        /// An easy to use constructor taking only a board.
        /// </summary>
        /// <param name="board">The board that should be represented by this state</param>
        /// <remarks>
        /// This implementation is slow as it has to construct a PieceList itself.
        /// Use other constructors in time critical sections of code.
        /// </remarks>
        public GameState(Board board)
        {
            State = board;
            
            Span<Piece> tempBoardFields = stackalloc Piece[State.Fields.Length];
            State.Fields.CopyTo(tempBoardFields);
            
            List<Piece> tempWhitePieceList = new List<Piece>();
            List<Piece> tempBlackPieceList = new List<Piece>();

            // Fill PieceLists
            for (byte index = 0; index < 0x88; index++)
            {
                // if the index is invalid we have exceeded the boundaries of the board.
                // The body of the loop is skipped until the next valid index is encountered.
                if (!Board.IsIndexValid(index)) continue; 

                var currentPiece = board[index];
                if (currentPiece.PieceType != Empty)
                {
                    
                    // If the found piece doesn't have the right position then set it 
                    if (currentPiece.Position != index)
                    {
                        currentPiece = new Piece(currentPiece.Content, index);
                        tempBoardFields[index] = currentPiece;
                    }
                    
                    if ((currentPiece.PieceFlags & White) == White)
                    {
                        tempWhitePieceList.Add(currentPiece);
                    }
                    else
                    {
                        tempBlackPieceList.Add(currentPiece);
                    }
                }
            }

            State = new Board(tempBoardFields.ToArray());
            WhitePieces = tempWhitePieceList.ToArray();
            BlackPieces = tempBlackPieceList.ToArray();

            // BOOLS
            hasWhiteTowersMoved = new bool[]{false, false};
            hasBlackTowersMoved = new bool[]{false, false};
        }

        public readonly Board State;
        public readonly Piece[] WhitePieces;
        public readonly Piece[] BlackPieces;
        

        /**
         * <summary>A dummy method representing some logic to calculate the applying a move to the state</summary>
         * <param name="move">The move that should be applied to a state</param>
         * <returns>A new <see cref="GameState"/> with the move applied</returns>
         */
        public GameState ApplyMove(Move move)
        {
            Span<Piece> newFields = stackalloc Piece[State.Fields.Length];
            State.Fields.CopyTo(newFields);

            Piece movedPiece = newFields[move.StartPos];
            newFields[move.StartPos] = new Piece(Empty);
            Piece capturedPiece = newFields[move.EndPos];
            newFields[move.EndPos] = new Piece(movedPiece.PieceFlags ^ Piece.beenMovedFlag, move.EndPos) ;

            //TODO use another constructor to make this implementation faster (perhaps make one that takes a span)
            var newBoard = new Board(newFields.ToArray());
            var newState = new GameState(newBoard);

            return newState;
        }
    
    }
}








    
    