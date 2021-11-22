using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static ChessAI.DataClasses.Piece;

namespace ChessAI.DataClasses
{
    /**
     * <summary>
     * A struct that keeps track of all relevant information about a game
     * </summary>
     */
    public readonly ref struct GameState
    {
        /// <summary>
        /// An easy to use constructor taking only a board.
        /// </summary>
        /// <param name="board">The board that should be represented by this state</param>
        /// <param name="isWhite">a boolean deciding if the Ai is playing as white</param>
        /// <param name="lastMove">The move performed last</param>
        /// <param name="canARankRookCastle">A boolean representing the possibility of castling in the A rank</param>
        /// <param name="canHRankRookCastle">A boolean representing the possibility of castling in the H rank</param>
        /// <remarks>
        /// This implementation is slow as it has to construct a PieceList itself.
        /// Use other constructors in time critical sections of code.
        /// </remarks>
        public GameState(Board board, bool isWhite, Move lastMove = new Move(), bool canARankRookCastle = true,
            bool canHRankRookCastle = true)
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
            WhitePieces = new PieceList(tempWhitePieceList.ToArray());
            BlackPieces = new PieceList(tempBlackPieceList.ToArray());
            PreviousMove = lastMove;
            CanARankRookCastle = canARankRookCastle;
            CanHRankRookCastle = canHRankRookCastle;
            IsWhite = isWhite;
        }

        public GameState(Board board, PieceList whitePieces, PieceList blackPieces, Move previousMove, bool canARankRookCastle,
            bool canHRankRookCastle)
        {
            State = board;
            WhitePieces = whitePieces;
            BlackPieces = blackPieces;
            PreviousMove = previousMove;
            CanARankRookCastle = canARankRookCastle;
            CanHRankRookCastle = canHRankRookCastle;
        }

        public static GameState CreateNewGameState(bool isWhite)
        {
            var board = Board.CreateNewBoard();
            return new GameState(board, isWhite);
        }

        public readonly Board State;
        public readonly PieceList WhitePieces;
        public readonly PieceList BlackPieces;
        public readonly Move PreviousMove;
        public readonly bool CanARankRookCastle;
        public readonly bool CanHRankRookCastle;

        public static bool IsWhite = true;

        /**
         * <summary>A dummy method representing some logic to calculate the applying a move to the state</summary>
         * <param name="move">The move that should be applied to a state</param>
         * <returns>A new <see cref="GameState"/> with the move applied</returns>
         */
        public GameState ApplyMove(Move move)
        {
            var canARankRookCastle = CanARankRookCastle;
            var canHRankRookCastle = CanHRankRookCastle;

            //If this is the AI' move check if castling is affected
            if ((canARankRookCastle || canHRankRookCastle) && IsWhite == move.MovePiece.IsWhite)
            {
                if (move.MovePiece == King)
                {
                    canARankRookCastle = false;
                    canHRankRookCastle = false;
                }
                else if (move.MovePiece.PieceType == Rook)
                {
                    switch (move.StartPos & 0xf0)
                    {
                        case 0x00: //Is in rank A
                            canARankRookCastle = false;
                            break;
                        case 0x70: //Is in rank H
                            canHRankRookCastle = false;
                            break;
                    }
                }
            }

            PieceList whitePieces = WhitePieces;
            PieceList blackPieces = BlackPieces;
            
            if (move.TargetPiece != Empty)
            {
                if (move.TargetPiece.IsWhite)
                {
                    whitePieces = WhitePieces.Minus(move.TargetPiece);
                }
                else
                {
                    blackPieces = BlackPieces.Minus(move.TargetPiece);
                }
            }

            var modifiedMovePiece = new Piece(move.MovePiece.Content, move.EndPos);
            if (move.MovePiece.IsWhite)
            {
                for (byte i = 0; i < whitePieces.Length; i++)
                {
                    if (whitePieces[i].Position == move.StartPos)
                    {
                        whitePieces = whitePieces.Edit(i, modifiedMovePiece);
                    }
                }
            }
            else
            {
                for (byte i = 0; i < blackPieces.Length; i++)
                {
                    if (blackPieces[i].Position == move.StartPos)
                    {
                        blackPieces = blackPieces.Edit(i, modifiedMovePiece);
                    }
                }
            }

            var fields = new Piece[State.Fields.Length];
            State.Fields.CopyTo(fields);
            fields[move.StartPos] = new Piece(Empty, move.StartPos);
            fields[move.EndPos] = modifiedMovePiece;

            var newBoard = new Board(fields);
            
            var newState = new GameState(newBoard, whitePieces, blackPieces, move, canARankRookCastle, canHRankRookCastle);
            return newState;
        }
    }
}