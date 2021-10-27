using System;
using System.Collections.Generic;

namespace ChessAI
{
    /**
     * <summary>
     * dummy struct representing a game state in some way
     * </summary>
     */
    public readonly ref struct GameState
    {
        //Implementation goes here...
    }

    /**
     * <summary>
     * A Class that contains all the logic for finding the best move.
     * </summary>
     */
    public class MoveSelector
    {
        private readonly bool _isWhite;
        private string[] _bestMoves;

        public MoveSelector(bool playerIsWhite)
        {
            _isWhite = playerIsWhite;
            _bestMoves = Array.Empty<string>();
        }

        /**
         * <summary>
         * The method to use if you want to perform a fixed depth search for the best move.
         * </summary>
         * <param name="depth">The depth to which the algorithm will search</param>
         * <param name="state">The current state of the game</param>
         */
        public string BestMove(int depth, GameState state)
        {
            //TODO should this be rewritten to act nice with iterative deepening?
            if (_bestMoves.Length ! >= depth)
            {
                var newArray = new string[depth];
                _bestMoves.CopyTo(newArray, 0);
                _bestMoves = newArray;
            }

            MinMax(depth, depth, true, state);

            return _bestMoves[0];
        }
        

        /**
         * <summary>
         * The implementation of the minMax algorithm using alpha pruning that we can use to search for the best move
         * </summary>
         * <param name="searchDepth">The desired depth to which it should search</param>
         * <param name="currentDepth">The depth of the current node</param>
         * <param name="isMaximizer">A value deciding which role the node takes on; maximiser or minimiser</param>
         * <param name="state">The state of the game as it would look all moves leading to this node were taken</param>
         * <param name="alpha">The value storing the maximiser's current best value</param>
         * <param name="beta">The value storing the minimiser's current best value</param>
         */
        private int MinMax(int searchDepth, int currentDepth, bool isMaximizer, GameState state, int alpha = Int32.MinValue, int beta = Int32.MaxValue)
        {
            if (searchDepth <= currentDepth)
            {
                return StaticAnalysis(state: state);
            }
            
            // Generate moves, sort them and remove the previous best move to avoid
            // it being used in other branches than the best
            var moves = CalculatePossibleMoves(state, _isWhite == isMaximizer);
            SortMovesByBest(moves, _bestMoves[currentDepth]);
            _bestMoves[currentDepth] = ""; 

            if (isMaximizer)
            {
                foreach (var move in moves)
                {
                    var child = ApplyMove(move, state);
                    var value = MinMax(searchDepth, currentDepth - 1, false, child, alpha, beta);

                    if (value > alpha)
                    {
                        alpha = value;

                        if (alpha >= beta)
                        {
                            return alpha;
                        }

                        _bestMoves[currentDepth] = move;
                    }
                }

                return alpha;
            }
            else
            {
                foreach (var move in moves)
                {
                    var child = ApplyMove(move, state);
                    var value = MinMax(searchDepth, currentDepth - 1, true, child, alpha, beta);

                    if (value < beta)
                    {
                        beta = value;

                        if (alpha >= beta)
                        {
                            return beta;
                        }
                        
                        _bestMoves[currentDepth] = move;
                    }
                }

                return beta;
            }
        }

        /**
         * <summary>A dummy method representing some logic to calculate all moves from a given state</summary>
         * <param name="state">The state for which possible moves should be calculated</param>
         * <param name="calculateForWhite">
         *  A boolean flag that tells which side's moves should be generated. true -> white and false -> black
         * </param>
         * <returns>A <see cref="List{T}"/> of all legal moves for the given player</returns>
         */
        private static List<string> CalculatePossibleMoves(GameState state, bool calculateForWhite)
        {
            //implementation goes here...
            return new List<string>();
        }

        /**
         * <summary>A dummy method representing some logic to calculate the applying a move to the state</summary>
         * <param name="move">The move that should be applied to a state</param>
         * <param name="state">The state to apply the move to</param>
         * <returns>A new <see cref="GameState"/> with the move applied</returns>
         */
        private static GameState ApplyMove(string move, GameState state)
        {
            //implementation goes here...
            return state;
        }

        /**
         * <summary>A dummy method representing some logic to calculate the value of the state</summary>
         * <param name="state">The state that should be Analyses</param>
         * <returns>
         * A numeric value that represents how good a state is. A positive value is in favor of this engine
         * while a negative one is in favor of its opponent no matter their color
         * </returns>
        */
        private static int StaticAnalysis(GameState state /*may need an argument or static var for who the engine is*/)
        {
            //implementation goes here...
            return 0;
        }

        /**
         * <summary>
         * A dummy method representing some logic to sort the list of moves according to some simple
         * fast logistic to increase the chance of greater cutoffs.
         * Note that it sorts the list in place and therefore won't return anything.
         * </summary>
         * <param name="moves">A list of possible moves</param>
         * <param name="previousBest">
         * The best move at this point in an earlier run.
         * This should always be the first move in the sorted list as it has a very
         * high likelihood of being the best again
         * </param>
         */
        private static void SortMovesByBest(List<string> moves, string previousBest)
        {
           
        }
    }
}