using System;
using System.Text;

namespace ChessAI.DataClasses
{
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
        ///<code>
        ///     unused:         1 bit
        ///     has moved       1 bit
        ///     challenges:     1 bit
        ///     is challenged:  1 bit
        ///     is white:       1 bit
        ///     Piece type:     3 bits
        ///</code>
        /// </summary>
        private readonly byte _piece;

        /// <summary>
        /// <inheritdoc cref="_piece"/>
        /// </summary>
        public byte Content => _piece;

        public byte PieceFlags => (byte)(_piece & 0b1111_0000);
        public byte PieceType => (byte)(_piece & PieceMask);
        public byte ColorAndType => (byte)(_piece & 0x0f);
        public bool IsWhite => (_piece & White) == White;
        public bool HasFlag(byte flag) => (PieceFlags & flag) == flag;
        

        /// <summary>
        /// The position of the piece as an index on an 0x88 board.
        /// If a position doesn't make sense or is unknown for this piece an invalid index is set instead.
        /// </summary>
        public byte Position { get; }
        public bool hasMoved => (_piece & BeenMovedFlag) != 0;

        /// <summary>
        /// Base constructor with all arguments given and as the correct type
        /// </summary>
        /// <param name="flags">
        /// piece type and color at minimum but may also contain other flags as described by <see cref="Content"/>.
        /// </param>
        /// <param name="position">An index describing the position on the board</param>
        public Piece(byte flags, byte position)
        {
            _piece = flags;
            Position = position;
        }

        public Piece(byte flags) : this(flags, 0xAA){}

        public Piece(int flags) : this((byte)flags, 0xAA){}

        public Piece(int flags, byte position) : this((byte)flags, position){}

        public Piece(byte flags, int position) : this(flags, (byte)position){}

        public Piece(int flags, int position) : this((byte)flags, (byte)position){}


        //######################################//
        // Constants for easy use of this type  //
        //######################################//

        public const byte Empty = 0;
        public const byte PieceMask = 0b0111;
        public const byte FlagMask = 0b1111_0000;

        // Piece definitions
        public const byte Pawn   = 0b0001;
        public const byte Rook   = 0b0010;
        public const byte Knight = 0b0011;
        public const byte Bishop = 0b0100;
        public const byte Queen  = 0b0101;
        public const byte King   = 0b0110;

        public const byte NumOfTypes = 6;

        // Flags
        public const byte White = 0b1000;
        public const byte Black = 0;

        // movedFlag
        public const byte BeenMovedFlag = 0b0100_0000;
        public const byte ChallengesOtherFlag = 0b0010_0000;
        public const byte ChallengedFlag = 0b0001_0000;
        
        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append(IsWhite ? "White" : "Black");
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

            return builder.ToString();
        }

        public string ToPrettyString()
        {
            string str ="";
            if(PieceType == Empty)
                return ".";

            if(IsWhite){
                switch(PieceType){
                    case Pawn:    str += "|p|";     break;   
                    case Rook:    str += "|r|";     break;
                    case Knight:  str += "|k|";     break;
                    case Bishop:  str += "|b|";     break;
                    case Queen:   str += "|q|";     break;
                    case King:    str += "|K|";     break;
                }
            }else{
                switch(PieceType){
                    case Pawn:    str += "p";     break;   
                    case Rook:    str += "r";     break;
                    case Knight:  str += "k";     break;
                    case Bishop:  str += "b";     break;
                    case Queen:   str += "q";     break;
                    case King:    str += "k";     break;
                }
            }
            return str;
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
        public static bool operator ==(Piece a, Piece b) => a.ColorAndType == b.ColorAndType && a.Position == b.Position;
        public static bool operator ==(byte a, Piece b) => a == b._piece;
        public static bool operator ==(Piece a, byte b) => b == a;
        public static bool operator !=(Piece a, Piece b) => !(a == b);
        public static bool operator !=(byte a, Piece b) => !(a == b);
        public static bool operator !=(Piece a, byte b) => !(a == b);
        public static bool operator ==(int a, Piece b) => (byte)a == b;
        public static bool operator ==(Piece a, int b) => b == a;
        public static bool operator !=(int a, Piece b) => !(a == b);
        public static bool operator !=(Piece a, int b) => !(a == b);
    }
}