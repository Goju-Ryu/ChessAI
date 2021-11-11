using System;
using System.Diagnostics.CodeAnalysis;

namespace ChessAI.DataClasses
{
    [Flags]
    public enum FlagPiece : byte
    {
        // First three bits are used to represent which piece this is
        None    = 0,
        Pawn    = 1,
        Rook    = 2,
        Knight  = 3,
        Bishop  = 4,
        Queen   = 5,
        King    = 6,
        
        // Helpers
        PieceMask = 0b0111,
        
        // The next 7 bits can contain actual flags
        White = 1 << 3,
        Black = 0,
        
    }

    public static class FlagPieceExtensions
    {

        public static bool IsWhite(this FlagPiece piece)
        {
            return (piece & FlagPiece.White) == FlagPiece.White;
        }

        public static FlagPiece[] GetPieceTypes()
        {
            return new[]
            {
                FlagPiece.Pawn,
                FlagPiece.Rook,
                FlagPiece.Knight,
                FlagPiece.Bishop,
                FlagPiece.Queen,
                FlagPiece.King
            };
        }
    }
}