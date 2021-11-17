using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static ChessAI.DataClasses.Piece;

namespace ChessAI.DataClasses
{
    /**
     * <summary>
     * A struct that keeps track of all relevant information about a game
     * </summary>
     */
    public readonly struct GameState
    {
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
            var tempBoardFields = board.Fields; 
            
            List<Piece> tempWhitePieceList = new List<Piece>();
            List<Piece> tempBlackPieceList = new List<Piece>();

            // Fill PieceLists
            for (byte index = 0; index < 0x88; index++)
            {
                // if the index is invalid we have exceeded the boundaries of the board.
                // The body of the loop is skipped until the next valid index is encountered.
                if (!Board.IsIndexValid(index)) continue; 

                var currentPiece = board[index];
                if (currentPiece != Empty)
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

            State = new Board(tempBoardFields);
            WhitePieces = tempWhitePieceList.ToArray();
            BlackPieces = tempBlackPieceList.ToArray();
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
            //implementation goes here...

            var newFields = new Piece[State.Fields.Length];
            State.Fields.CopyTo(newFields, 0);

            var movedPiece = newFields[move.StartPos];
            newFields[move.StartPos] = new Piece(Empty);
            var capturedPiece = newFields[move.EndPos];
            newFields[move.EndPos] = movedPiece;

            var newPieceList = new Piece[6][];
            throw new NotImplementedException();
        }
    }
}