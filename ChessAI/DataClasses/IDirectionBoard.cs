namespace ChessAI.DataClasses
{
    public interface IDirectionBoard{
        byte GetFieldsToEdge(int position , int direction);
        bool IsFieldOccupied(byte position);
        bool IsFieldEnemouriouslyConqueredByEvil(byte position);

    }
}