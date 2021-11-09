using System.Collections;

namespace ChessAI{
    interface IBoard{
        BitArray Data { get; }
        bool FieldHasPiece(byte fieldIndex);
        bool FieldHasPieceOfColor(byte fieldIndex, bool color);
    }

    interface IPiece{
        byte[] GetMoves();
        bool GetColor();
        byte GetType();
    }
}
