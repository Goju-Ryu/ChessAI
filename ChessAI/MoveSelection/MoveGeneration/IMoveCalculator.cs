using System.Collections.Generic;
using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.MoveGeneration
{
    public interface IMoveCalculator
    {
        /**
         * <summary>A dummy method representing some logic to calculate all moves from a given state</summary>
         * <param name="state">The state for which possible moves should be calculated</param>
         * <param name="calculateForWhite">
         *  A boolean flag that tells which side's moves should be generated. true -> white and false -> black
         * </param>
         * <returns>A <see cref="List{T}"/> of all legal moves for the given player</returns>
         */
        List<string> CalculatePossibleMoves(GameState state, bool calculateForWhite);
    }
}