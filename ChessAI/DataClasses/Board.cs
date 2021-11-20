using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ChessAI.DataClasses
{
    public readonly struct Board
    {
        private const byte Width = 0x10;
        private const byte Height = 0x8;

        private static readonly  ImmutableDictionary<Direction, sbyte> Directions =  new Dictionary<Direction, sbyte>()
        {
            { Direction.Up, +0x10 }, // 1 up
            { Direction.Down, -0x10 }, // 1 down
            { Direction.Right, +0x01 }, // 1 right
            { Direction.Left, -0x01 }, // 1 left
        }.ToImmutableDictionary();

        public static sbyte WhiteDirection(Direction direction) => Directions[direction];
        public static sbyte BlackDirection(Direction direction) => (sbyte) -Directions[direction];

        private readonly Piece[] _fields; //TODO consider replacing this array with a stack allocated ReadOnlySpan<T>

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
            foreach (var piece in pieceList)
            {
                fields[piece.Position] = piece;
            }

            _fields = fields;
        }

        public Piece this[int i] => Fields[i];

        public ReadOnlySpan<Piece> Fields => _fields.AsSpan();

        public bool IsFieldOwnedByWhite(byte position)
        {
            return Fields[position].IsWhite;
        }

        public bool IsFieldOccupied(byte position)
        {
            return Fields[position].PieceType != Piece.Empty;
        }

        public static bool IsIndexValid(byte index)
        {
            //Checks that only bits used for counting from 0-7 is used for each digit in the hex representation of the index
            return (index & 0b1000_1000) == 0;
        }

        public static string IndexToString(byte index)
        {
            if (!IsIndexValid(index)) throw new ArgumentException("Argument must be a valid index");

            return (index & 0xF0) switch
            {
                0x00 => "A" + (index & 0xF),
                0x10 => "B" + (index & 0xF),
                0x20 => "C" + (index & 0xF),
                0x30 => "D" + (index & 0xF),
                0x40 => "E" + (index & 0xF),
                0x50 => "F" + (index & 0xF),
                0x60 => "G" + (index & 0xF),
                0x70 => "H" + (index & 0xF),
                _ => throw new ArgumentException("Argument must be a valid index")
            };
        }

        public static byte StringToIndex(string fieldName)
        {
            if (fieldName.Length != 2
                || !"ABCDEFGHabcdefgh".Contains(fieldName[0])
                || !"12345678".Contains(fieldName[1]))
            {
                throw new ArgumentException("Argument must be a valid field name");
            }

            var lastDigit = Byte.Parse(fieldName.Substring(1));
            return (fieldName[0]) switch
            {
                'A' => (byte)(0x00 + lastDigit),
                'B' => (byte)(0x10 + lastDigit),
                'C' => (byte)(0x20 + lastDigit),
                'D' => (byte)(0x30 + lastDigit),
                'E' => (byte)(0x40 + lastDigit),
                'F' => (byte)(0x50 + lastDigit),
                'G' => (byte)(0x60 + lastDigit),
                'H' => (byte)(0x70 + lastDigit),
                _ => throw new ArgumentException("Argument must be a valid index")
            };
        }

        public override string ToString()
        {
            String str = "{";
            for (int i = 0; i < Fields.Length; i++)
            {
                if (i % 0x10 == 0)
                {
                    str += "\n";
                }
                else if (i % 0x08 == 0)
                {
                    str += "\t";
                }

                str += "\t" + Fields[i].ToString();
            }

            return str;
        }
    
        public static byte PositionConverter(byte pos){
            int xPos = 0x80 - pos - 0x09;
            return (byte) xPos;
        }

        // TODO DOUBLE CHECK THIS IS CORRECT 
        public static bool isDIrectionPositive(Piece piece){
            return piece.IsWhite;
        }
    }

    public enum Direction : byte
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    };

    public enum BT : int
    {
        ColA = 0x00,
        ColB = 0x10,
        ColC = 0x20,
        ColD = 0x30,
        ColE = 0x40,
        ColF = 0x50,
        ColG = 0x60,
        ColH = 0x70,

        Row1 = 0x00,
        Row2 = 0x01,
        Row3 = 0x02,
        Row4 = 0x03,
        Row5 = 0x04,
        Row6 = 0x05,
        Row7 = 0x06,
        Row8 = 0x07
    };
}