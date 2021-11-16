namespace ChessAI.DataClasses
{
    public class Board : DirectionBoard , IDirectionBoard {

        // NOTE INHERITED MEMBERS 
        //public static readonly byte maxFields = 64;
        //public static readonly byte fieldsPrSide = 8;
        //public static readonly sbyte[] dirOffsets = { 8, -8, -1, 1, 7, -7, 9, -9 };

        Piece _pieces;
        public Board() : base() {
            _pieces = new Piece();
        }	

        public bool IsFieldOccupied(byte position){
            return false;
        }

        public bool IsFieldEnemouriouslyConqueredByEvil(byte position){
            return false;
        }

    }
}