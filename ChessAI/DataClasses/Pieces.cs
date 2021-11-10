namespace ChessAI.DataClasses
{
    public enum Pieces : byte {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }
    
    // EXAMPLE FOR MADS 
    public static class PiecesExtensions {
        public static int ExampleExtensionMethod(this Pieces piece){
            return (int) piece; // THIS METHOD IS NOW AVAILABLE IN Pieces enum OBJECTS // HURRAH
        }
    }
}