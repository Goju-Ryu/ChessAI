using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ChessAI.DataClasses
{
    //todo could this be a ref struct?
    public readonly struct Piece : IEquatable<Piece>
    {
        /// <summary>
        /// _piece stores information describing a single piece possibly on a board.
        /// The byte representing the piece is conceptually split into two parts; one describing the pieces color
        /// and type, and another describing additional information relating to the state it is in.
        /// The piece type is represented by the three least significant bits as consecutive numbers starting at 1.
        /// The color is represented by the next bit where a 1 is white and 0 is black.
        ///
        /// below is a description of the layout of _piece starting from the most significant bit
        /// towards the least significant on
        ///
        ///     unused:         2 bits
        ///     challenges:     1 bit
        ///     is challenged:  1 bit
        ///     is white:       1 bit
        ///     Piece type:     3 bits
        ///
        /// </summary>
        private readonly byte _piece;

        public byte Content => _piece;
        public byte PieceFlags => (byte)(_piece & 0b1111_1000);
        public byte PieceType => (byte)(_piece & PieceMask);
       
        /// <summary>
        /// The position of the piece as an index on an 0x88 board.
        /// If a position doesn't make sense or is unknown for this piece an invalid index is set instead.
        /// </summary>
        public byte Position { get; }

        public Piece(byte flags)
        {
            _piece = flags;
            Position = 0xAA; // Outside valid indexes as the piece has been given no 
        }
        public Piece(byte flags, byte position)
        {
            _piece = flags;
            Position = position;
        }

        public Piece(int flags, byte position)
        {
            Position = position;
            _piece = (byte)flags;
        }

        public Piece(byte flags, int position)
        {
            Position = (byte)position;
            _piece = flags;
        }

        public Piece(int flags, int position)
        {
            Position = (byte)position;
            _piece = (byte)flags;
        }
        //######################################//
        // Constants for easy use of this type  //
        //######################################//

        public const byte Empty = 0;
        public const byte PieceMask = 0b0111;

        // Piece definitions
        public const byte Pawn = 0b0001;
        public const byte Rook = 0b0010;
        public const byte Knight = 0b0011;
        public const byte Bishop = 0b0100;
        public const byte Queen = 0b0101;
        public const byte King = 0b0110;

        public const byte NumOfTypes = 6;

        // Flags
        public const byte White = 0b1000;
        public const byte Black = 0; //TODO check this plays well with other functionality

        public bool isWhite(){
            return ( White & _piece ) != 0;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if( this.PieceType == Piece.Empty)
                return  this.Position.ToString("X4");

            builder.Append( this.isWhite() ? "W" : "B" );
            builder.Append(" ");
            builder.Append(
                (PieceType) switch
                {
                    Piece.Pawn   => "P",
                    Piece.Rook   => "R",
                    Piece.Knight => "K",
                    Piece.Bishop => "B",
                    Piece.Queen  => "Q",
                    Piece.King   => "K",
                    _ => "X"
                }
            );

            return builder.ToString();
        }

        public bool Equals(Piece other)
        {
            return _piece == other._piece;
        }

        public override bool Equals(object obj)
        {
            return obj is Piece other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _piece.GetHashCode();
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