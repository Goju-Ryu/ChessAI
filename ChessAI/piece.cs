namespace ChessAI {

    public enum Pieces : byte {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }
    public readonly struct Move{
        public readonly byte StartPos;
        public readonly byte EndPos;
        public Move(byte startPos , byte endPos){ StartPos = startPos; EndPos = endPos; }
    }

    // inspiration Source https://www.youtube.com/watch?v=U4ogK0MIzqk
    public class Piece {


    }
}
