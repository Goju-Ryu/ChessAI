using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using static ChessAI.DataClasses.Piece;

namespace ChessAI.DataClasses
{
    public readonly ref struct Board
    {
        private const byte Width = 0x10;
        private const byte Height = 0x8;

        private static readonly ImmutableDictionary<Direction, sbyte> Directions = new Dictionary<Direction, sbyte>()
        {
            { Direction.Up, +0x10 }, // 1 up
            { Direction.Down, -0x10 }, // 1 down
            { Direction.Right, +0x01 }, // 1 right
            { Direction.Left, -0x01 }, // 1 left
        }.ToImmutableDictionary();

        public static sbyte WhiteDirection(Direction direction) => Directions[direction];
        public static sbyte BlackDirection(Direction direction) => (sbyte)-Directions[direction];

        public static readonly ImmutableDictionary<byte, byte[]> StartPositions = new Dictionary<byte, byte[]>()
        {
            { White | Pawn, new byte[] { 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17 } },
            { White | Rook, new byte[] { 0x00, 0x07 } },
            { White | Knight, new byte[] { 0x01, 0x06 } },
            { White | Bishop, new byte[] { 0x02, 0x05 } },
            { White | Queen, new byte[] { 0x03 } },
            { White | King, new byte[] { 0x04 } },

            { Black | Pawn, new byte[] { 0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67 } },
            { Black | Rook, new byte[] { 0x70, 0x77 } },
            { Black | Knight, new byte[] { 0x71, 0x76 } },
            { Black | Bishop, new byte[] { 0x72, 0x75 } },
            { Black | Queen, new byte[] { 0x73 } },
            { Black | King, new byte[] { 0x74 } }
        }.ToImmutableDictionary();

        private readonly Span<Piece> _fields;

        /// <summary>
        /// A constructor taking a <see cref="Span{T}"/> of pieces representing the board. Note that the span length must
        /// be consistent with an 0x88 board.
        /// </summary>
        /// <param name="fields">a span representing the board</param>
        /// <exception cref="ArgumentException">thrown if the given array is not of the right length</exception>
        public Board(Span<Piece> fields)
        {
            var expectedSize = Width * Height;
            if (fields.Length != expectedSize)
            {
                throw new ArgumentException(
                    "fields must be an array of length 0x" + expectedSize.ToString("X") + " / 0d" + expectedSize +
                    " but the provided array had length 0x" + fields.Length.ToString("X") + " / 0d" + fields.Length
                );
            }

            _fields = fields;
        }

        /// <summary>
        /// A constructor taking an array of pieces representing the board. Note that the array length must
        /// be consistent with an 0x88 board.
        /// </summary>
        /// <param name="fields">an array representing the board</param>
        /// <exception cref="ArgumentException">thrown if the given array is not of the right length</exception>
        public Board(Piece[] fields)
        {
            var expectedSize = Width * Height;
            if (fields.Length != expectedSize)
            {
                throw new ArgumentException(
                    "fields must be an array of length 0x" + expectedSize.ToString("X") + " / 0d" + expectedSize +
                    " but the provided array had length 0x" + fields.Length.ToString("X") + " / 0d" + fields.Length
                );
            }

            _fields = fields;
        }

        /// <summary>
        /// A Constructor that takes a list of fields. It is assumed that all pieces have valid positions set.
        /// If one or more pieces has an invalid position it can lead to unexpected behavior.
        /// </summary>
        /// <param name="pieceList">A list of pieces with valid positions</param>
        public Board(List<Piece> pieceList)
        {
            var fields = new Piece[Width * Height];
            for (byte i = 0; i < fields.Length; i++)
            {
                if(!IsIndexValid(i)) continue;
                fields[i] = new Piece(Empty, i);
            }

            foreach (var piece in pieceList)
            {
                fields[piece.Position] = piece;
            }

            _fields = fields;
        }

        public Piece this[int i] => Fields[i];

        public Span<Piece> Fields => _fields; //.AsSpan();

        public bool IsFieldOwnedByWhite(byte position)
        {
            return IsIndexValid(position) && Fields[position].IsWhite;
        }

        public bool IsFieldOccupied(byte position)
        {
            
            return !IsIndexValid(position) || Fields[position].PieceType != Empty;
        }

        public static bool IsIndexValid(byte index)
        {
            //Checks that only bits used for counting from 0-7 is used for each digit in the hex representation of the index
            return (index & 0b1000_1000) == 0;
        }

        public static string IndexToString(byte index)
        {
            if (!IsIndexValid(index)) throw new ArgumentException("Argument must be a valid index");

            int COL = index % 0x10;
            int ROW = index / 0x10;

            string str ="";
            switch(COL){
                case 0: str += "a"; break;
                case 1: str += "b"; break;
                case 2: str += "c"; break;
                case 3: str += "d"; break;
                case 4: str += "e"; break;
                case 5: str += "f"; break;
                case 6: str += "g"; break;
                case 7: str += "h"; break;
            }

            str += (ROW+1) ;
            return str;
        }

        public static byte StringToIndex(string fieldName)
        {
            fieldName = fieldName.ToUpper();
            if (fieldName.Length != 2
                || !"ABCDEFGH".Contains(fieldName[0])
                || !"12345678".Contains(fieldName[1]))
            {
                throw new ArgumentException("Argument must be a valid field name");
            }

            var firstDigit = Byte.Parse(fieldName.Substring(1));
            return (fieldName[0]) switch
            {
                'A' => (byte)(((firstDigit - 1) * 0x10) + 0x00),
                'B' => (byte)(((firstDigit - 1) * 0x10) + 0x01),
                'C' => (byte)(((firstDigit - 1) * 0x10) + 0x02),
                'D' => (byte)(((firstDigit - 1) * 0x10) + 0x03),
                'E' => (byte)(((firstDigit - 1) * 0x10) + 0x04),
                'F' => (byte)(((firstDigit - 1) * 0x10) + 0x05),
                'G' => (byte)(((firstDigit - 1) * 0x10) + 0x06),
                'H' => (byte)(((firstDigit - 1) * 0x10) + 0x07),
                _ => throw new ArgumentException("Argument must be a valid index")
            };
        }

        public static Board CreateNewBoard()
        {
            var fields = new Piece[Width * Height];
            fields.Initialize();

            var pieces = new[]
            {
                //White officers
                new Piece(White | Rook, 0x00),
                new Piece(White | Knight, 0x01),
                new Piece(White | Bishop, 0x02),
                new Piece(White | Queen, 0x03),
                new Piece(White | King, 0x04),
                new Piece(White | Bishop, 0x05),
                new Piece(White | Knight, 0x06),
                new Piece(White | Rook, 0x07),
                //White pawns
                new Piece(White | Pawn, 0x10),
                new Piece(White | Pawn, 0x11),
                new Piece(White | Pawn, 0x12),
                new Piece(White | Pawn, 0x13),
                new Piece(White | Pawn, 0x14),
                new Piece(White | Pawn, 0x15),
                new Piece(White | Pawn, 0x16),
                new Piece(White | Pawn, 0x17),
                //Black pawns
                new Piece(Black | Pawn, 0x60),
                new Piece(Black | Pawn, 0x61),
                new Piece(Black | Pawn, 0x62),
                new Piece(Black | Pawn, 0x63),
                new Piece(Black | Pawn, 0x64),
                new Piece(Black | Pawn, 0x65),
                new Piece(Black | Pawn, 0x66),
                new Piece(Black | Pawn, 0x67),
                //Black officers
                new Piece(Black | Rook, 0x70),
                new Piece(Black | Knight, 0x71),
                new Piece(Black | Bishop, 0x72),
                new Piece(Black | Queen, 0x73),
                new Piece(Black | King, 0x74),
                new Piece(Black | Bishop, 0x75),
                new Piece(Black | Knight, 0x76),
                new Piece(Black | Rook, 0x77),
            };


            foreach (var piece in pieces)
            {
                fields[piece.Position] = piece;
            }

            return new Board(fields);
        }

        // ########################################### //
        //           FOR CALCULATION OF MOVES          //
        // ########################################### //

        public static byte PositionConverter(byte pos)
        {
            int xPos = 0x80 - pos - 0x09;
            return (byte)xPos;
        }

        // TODO DOUBLE CHECK THIS IS CORRECT 
        public static bool IsDIrectionPositive(Piece piece)
        {
            return piece.IsWhite;
        }


        public override string ToString()
        {
            char[] Columns  = new char[]{'A','B','C','D','E','F','G','H'};
            char[] Rows     = new char[]{'1','2','3','4','5','6','7','8'};
            int rowNum = 7;
            StringBuilder strBuilder = new StringBuilder("#\t");

            foreach(char c in Columns){
                 strBuilder.Append( c + "\t");
            }
            strBuilder.Append("\n#\t");

            foreach(char c in Columns){
                 strBuilder.Append( "_\t");
            }
            strBuilder.Append("\n");

            int row, col;
            for (col = 0x70; col >=0 ; col-= 0x10)
            {
                
                strBuilder.Append( "#" + Rows[rowNum--] + ":\t");
                for (row = 0x00; row < 0x08; row+= 0x01)
                {
                   strBuilder.Append( Fields[ (col + row) ].ToPrettyString() + "\t");
                }
                strBuilder.Append( "\n");
            }

            return strBuilder.ToString();
        }
    }

    public enum Direction : byte
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    };
}