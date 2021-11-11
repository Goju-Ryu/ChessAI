using System;

namespace ChessAI.DataClasses
{
    public readonly struct Piece //todo could this be a ref struct?
    {
        public readonly byte PieceBitFlags { get; }

        public Piece(byte flags)
        {
            PieceBitFlags = flags;
        }

        //######################################//
        // Constants for easy use of this type  //
        //######################################//
        
        public const byte Empty = 0;
        
        // Piece definitions
        public const byte Pawn = 1;
        public const byte Rook = 2;
        public const byte Knight = 3;
        public const byte Bishop = 4;
        public const byte Queen = 5;
        public const byte King = 6;
        
        // Flags
        public const byte White = 0b1000;
        public const byte Black = 0; //TODO check this plays well with other functionality
        
    }
}

