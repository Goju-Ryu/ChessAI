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

        /// <summary>
        /// Return a new PieceList that does not contain a given element.
        /// This function may reorder the elements of this PieceList
        /// </summary>
        /// <param name="piece">The element to be removed</param>
        /// <returns>A new PieceList with the desired change applied</returns>
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

        /// <summary>
        /// Return a new PieceList where one element has been changed in any way.
        /// </summary>
        /// <param name="from">The index of the element to be changed</param>
        /// <param name="to">The desired piece that should replace the current piece</param>
        /// <returns>A new PieceList with the desired change applied</returns>
        public PieceList Edit(byte from, Piece to)
        {
            Span<Piece> newSpan = new Piece[_pieces.Length];
            _pieces.CopyTo(newSpan);

            newSpan[from] = to;
            return new PieceList(newSpan);
        }
        
        /// <summary>
        /// Return a new PieceList where one element has been changed in any way.
        /// </summary>
        /// <param name="from">
        /// The element to be changed.
        /// It is important that the pieces are equal using the == operator
        /// </param>
        /// <param name="to">The desired piece that should replace the current piece</param>
        /// <returns>A new PieceList with the desired change applied</returns>
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

        /// <summary>
        /// Edit a Piece of this PieceList in place. Some limitations apply to ensure the state stays consistent.
        /// Only flags can be changed, not the position. The piece type and color can not be changed however.
        /// </summary>
        /// <param name="index">The index of the element to be changed</param>
        /// <param name="flags">
        /// The flags that should be set, note that a flag is assumed to be false if not provided,
        /// so make sure that previous flags that should be preserved are passed as well
        /// </param>
        public void EditInPlace(byte index, byte flags)
        {
            var oldPiece = _pieces[index];
            _pieces[index] = new Piece((flags & Piece.FlagMask) | oldPiece.ColorAndType, oldPiece.Position);
        }


        /// <summary>
        /// Edit a Piece of this PieceList in place. Some limitations apply to ensure the state stays consistent.
        /// Only flags can be changed, not the position. The piece type and color can not be changed however.
        /// </summary>
        /// <param name="from">
        /// The element to be changed.
        /// It is important that the pieces are equal using the == operator
        /// </param>
        /// <param name="flags">
        /// The flags that should be set, note that a flag is assumed to be false if not provided,
        /// so make sure that previous flags that should be preserved are passed as well
        /// </param>
        public void EditInPlace(Piece from, byte flags)
        {
            for (byte i = 0; i < _pieces.Length; i++)
            {
                if (_pieces[i] == from)
                {
                    EditInPlace(i, flags);
                    return;
                }
                
            }

            throw new ArgumentException($"Piece {from} doesn't appear in this pieceList {_pieces.ToString()}");
        }
    }
}