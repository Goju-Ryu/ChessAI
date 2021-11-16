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
            State = board;
            List<byte>[] tempPieceList = new List<byte>[6];
            for (var i = 0; i < tempPieceList.Length; i++)
            {
                tempPieceList[i] = new List<byte>();
            }
            
            for (byte i = 0; i < 0x88; i++)
            {
                if (!Board.IsIndexValid(i))
                {
                    i += 0x8;
                }

                var currentPiece = board[i];
                if (currentPiece != Empty)
                {
                    tempPieceList[currentPiece.PieceType - 1].Add(i);
                }
            }

            PieceList = new byte[6][];

            for (int i = 0; i < PieceList.Length; i++)
            {
                PieceList[i] = tempPieceList[i].ToArray();
            }
        }


        public readonly Board State;
        public readonly byte[][] PieceList; 

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