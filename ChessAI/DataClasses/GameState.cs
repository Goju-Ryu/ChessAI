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
            WhitePieces = tempWhitePieceList.ToArray();
            BlackPieces = tempBlackPieceList.ToArray();
            PreviousMove = new Move();
            CanARankRookCastle = true;
            CanHRankRookCastle = true;
        }

        public GameState(Piece[] whitePieces, Piece[] blackPieces, Move previousMove, bool canARankRookCastle,
            bool canHRankRookCastle)
        {
            var pieces = new List<Piece>(whitePieces.Length + blackPieces.Length);
            pieces.AddRange(whitePieces);
            pieces.AddRange(blackPieces);

            State = new Board(pieces); //TODO implement fast board constructor/factory not using lists
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
        public readonly Piece[] WhitePieces;
        public readonly Piece[] BlackPieces;
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

            Piece[] whitePieces;
            Piece[] blackPieces;
            
            if (move.TargetPiece != Empty)
            {
                if (move.TargetPiece.IsWhite)
                {
                    whitePieces = new Piece[WhitePieces.Length - 1];

                    blackPieces = new Piece[BlackPieces.Length];
                    BlackPieces.CopyTo(blackPieces, 0);
                    
                    int oldI = 0;
                    for (int newI = 0; newI < whitePieces.Length; newI++)
                    {
                        if (WhitePieces[oldI] == move.TargetPiece) oldI++;

                        whitePieces[newI] = WhitePieces[oldI];
                        oldI++;
                    }
                }
                else
                {
                    blackPieces = new Piece[BlackPieces.Length - 1];
                    
                    whitePieces = new Piece[WhitePieces.Length];
                    WhitePieces.CopyTo(whitePieces, 0);
                    
                    int oldI = 0;
                    for (int newI = 0; newI < blackPieces.Length; newI++)
                    {
                        if (BlackPieces[oldI] == move.TargetPiece) oldI++;

                        blackPieces[newI] = BlackPieces[oldI];
                    }
                }
            }
            else
            {
                whitePieces = new Piece[WhitePieces.Length];
                blackPieces = new Piece[BlackPieces.Length];
                WhitePieces.CopyTo(whitePieces, 0);
                BlackPieces.CopyTo(blackPieces, 0);
            }

            var moveList = move.MovePiece.IsWhite ? whitePieces : blackPieces;
            
            
            for (int i = 0; i < moveList.Length; i++)
            {
                if (moveList[i] == move.MovePiece)
                {
                    moveList[i] = new Piece(move.MovePiece.Content, move.EndPos);
                }
            }
            
            var newState = new GameState(whitePieces, blackPieces, move, canARankRookCastle, canHRankRookCastle);
            return newState;
        }
    }
}