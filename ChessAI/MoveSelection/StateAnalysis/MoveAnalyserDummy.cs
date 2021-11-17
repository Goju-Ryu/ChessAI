using System.Collections.Generic;
using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.StateAnalysis
{
    /// <summary>
    /// An <see cref="IMoveAnalyser"/> that does not analyse but simply returns static values to enable
    /// testing with no sorting of moves.
    /// </summary>
    public class MoveAnalyserDummy : IMoveAnalyser
    {
        public int MoveAnalysis(GameState state, Move move)
        {
            return 0; //Simply evaluate any move in any context as being completely neutral
        }

        public void SortMovesByBest(GameState state, List<Move> moves, Move previousBest)
        {
            // Intentionally left empty
            // Do nothing to ensure no sorting happens
        }
    }
}