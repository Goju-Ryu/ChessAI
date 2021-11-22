using System;

namespace ChessAI.DataClasses
{
    public readonly ref struct PieceList
    {
        private readonly Span<Piece> _pieces;
        public ReadOnlySpan<Piece> Pieces => _pieces;

        public Piece this[int i] => Pieces[i];
        public int Length => Pieces.Length;

        public PieceList(Span<Piece> pieces)
        {
            _pieces = pieces;
        }

        public PieceList(Piece[] pieces)
        {
            _pieces = new Span<Piece>(pieces);
        }

        public PieceList Minus(Piece piece)
        {
            var temp = _pieces[0];

            for (byte i = 0; i < _pieces.Length; i++)
            {
                if (_pieces[i] == piece)
                {
                    _pieces[0] = piece;
                    _pieces[i] = temp;
                }
            }

            return new PieceList(_pieces.Slice(1));
        }

        public PieceList Edit(byte from, Piece to)
        {
            Span<Piece> newSpan = new Piece[_pieces.Length];
            _pieces.CopyTo(newSpan);

            newSpan[from] = to;
            return new PieceList(newSpan);
        }

        public PieceList Edit(Piece from, Piece to)
        {
            for (byte i = 0; i < _pieces.Length; i++)
            {
                if (_pieces[i] == from)
                {
                    return Edit(i, to);
                }
                
            }

            throw new ArgumentException($"Piece {from} doesn't appear in this pieceList {_pieces.ToString()}");
        }
    }
}