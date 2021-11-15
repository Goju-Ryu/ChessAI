using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessAI.DataClasses
{
    public readonly struct Board
    {
        private const byte Width = 0x10;
        private const byte Height = 0x8;
        public static readonly Board EmptyBoard = new Board(new Piece[Width * Height]);

        public readonly Piece[] Fields; //TODO consider using a Span<T> instead

        private Board(Piece[] fields)
        {
            Fields = fields;
        }

        public Board(List<(byte, Piece)> pieceIndexList)
        {
            var fields = new Piece[Width * Height];
            foreach (var pieceIndexPair in pieceIndexList)
            {
                fields[pieceIndexPair.Item1] = pieceIndexPair.Item2;
            }

            Fields = fields;
        }

        public bool IsFieldOccupied(byte position)
        {
            return Fields[position] == Piece.Empty;
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
                || !"ABCDEFGHabcdefgh".Contains(fieldName.First())
                || !"12345678".Contains(fieldName[1]))
            {
                throw new ArgumentException("Argument must be a valid field name");
            }

            var lastDigit = Byte.Parse(fieldName.Substring(1));
            return (fieldName.First()) switch
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
    }
}