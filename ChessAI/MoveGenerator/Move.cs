namespace ChessAI {     
    public readonly struct Move{
        public readonly byte StartPos;
        public readonly byte EndPos;
        public Move(byte startPos , byte endPos){ StartPos = startPos; EndPos = endPos; }
    }

}