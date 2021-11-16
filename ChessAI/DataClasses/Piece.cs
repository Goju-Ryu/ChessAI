using System;
using System.Text;

namespace ChessAI.DataClasses
{
    public readonly struct Piece //todo could this be a ref struct?
    {
        private readonly byte _piece;

        public byte Content => _piece;
        public byte PieceFlags => (byte)(_piece & 0b1111_1000);
        public byte PieceType => (byte)(_piece & PieceMask);

        public Piece(byte flags)
        {
            _piece = flags;
        }

        public Piece(int flags)
        {
            _piece = (byte)flags;
        }
        //######################################//
        // Constants for easy use of this type  //
        //######################################//

        public const byte Empty = 0;
        public const byte PieceMask = 0b0111;

        // Piece definitions
        public const byte Pawn      = 0b0001;
        public const byte Rook      = 0b0010;
        public const byte Knight    = 0b0011;
        public const byte Bishop    = 0b0100;
        public const byte Queen     = 0b0101;
        public const byte King      = 0b0110;

        // Flags
        public const byte White = 0b1000;
        public const byte Black = 0; //TODO check this plays well with other functionality


        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append((PieceFlags & White) == White ? "White" : "Black");
            builder.Append(" ");
            builder.Append(
                (PieceType) switch
                {
                    0 => "None",
                    1 => "Pawn",
                    2 => "Rook",
                    3 => "Knight",
                    4 => "Bishop",
                    5 => "Queen",
                    6 => "King",
                    _ => "Invalid"
                }
            );
            
            //Insert appends for other flags here as they are decided on

            return builder.ToString();
        }


        //######################################//
        //          Operator overloads          //
        //######################################//
        
        // bitwise And
        public static Piece operator &(Piece a, Piece b) => new Piece((byte)(a._piece & b._piece));
        public static Piece operator &(Piece a, byte b) => new Piece((byte)(a._piece & b));
        public static Piece operator &(byte a, Piece b) => new Piece((byte)(a & b._piece));
        
        // bitwise Or
        public static Piece operator |(Piece a, Piece b) => new Piece((byte)(a._piece | b._piece));
        public static Piece operator |(Piece a, byte b) => new Piece((byte)(a._piece | b));
        public static Piece operator |(byte a, Piece b) => new Piece((byte)(a | b._piece));
        
        // bitwise XOr
        public static Piece operator ^(Piece a, Piece b) => new Piece((byte)(a._piece ^ b._piece));
        public static Piece operator ^(Piece a, byte b) => new Piece((byte)(a._piece ^ b));
        public static Piece operator ^(byte a, Piece b) => new Piece((byte)(a ^ b._piece));
        
        // Equality
        public static bool operator ==(Piece a, Piece b) => a._piece == b._piece;
        public static bool operator ==(byte a, Piece b) => a == b._piece;
        public static bool operator ==(Piece a, byte b) => a._piece == b;
        public static bool operator !=(Piece a, Piece b) => a._piece != b._piece;
        public static bool operator !=(byte a, Piece b) => a != b._piece;
        public static bool operator !=(Piece a, byte b) => a._piece != b;
        public static bool operator ==(int a, Piece b) => a == b._piece;
        public static bool operator ==(Piece a, int b) => a._piece == b;
        public static bool operator !=(int a, Piece b) => a != b._piece;
        public static bool operator !=(Piece a, int b) => a._piece != b;
    }
}