public struct Move{
    public byte from, to , pieceIndex;
    public bool hasCaptured;

    public Move(byte from, byte to, byte pieceIndex, bool hasCaptured = false){
        this.from = from;
        this.to = to;
        this.pieceIndex = pieceIndex;
        this.hasCaptured = hasCaptured;
    }

    public Move(){}
}