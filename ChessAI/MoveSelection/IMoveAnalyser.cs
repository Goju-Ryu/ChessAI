using System;
using System.Collections.Generic;

namespace ChessAI.MoveSelection
{
    public interface IMoveAnalyser
    {
        /**
         * <summary>
         * A method representing some logic to calculate the value of a move.
         * This method should not be used to actually calculate the value of a potential state, as that is what the
         * <see cref="IStateAnalyser"/> is for, but should give a rough estimate of how likely it is to be a good
         * move to make.
         * </summary>
         * <param name="state">The current state before the move is applied</param>
         * <param name="move">The move that would be applied to the current state</param>
         * <returns>
         * A numeric value that represents how good the move is likely to be. A positive value is in
         * favor of this engine while a negative one is in favor of its opponent no matter their color
         * </returns>
        */
        int MoveAnalysis(GameState state, string move);

        /**
         * <summary>
         * A dummy method representing some logic to sort the list of moves according to some simple
         * fast logistic to increase the chance of greater cutoffs.
         * Note that it sorts the list in place and therefore won't return anything.
         * </summary>
         * <param name="state">The current state before the move is applied</param>
         * <param name="moves">A list of possible moves</param>
         * <param name="previousBest">
         * The best move at this point in an earlier run.
         * This should always be the first move in the sorted list as it has a very
         * high likelihood of being the best again
         * </param>
         */
        void SortMovesByBest(GameState state, List<string> moves, string previousBest);
    }
}