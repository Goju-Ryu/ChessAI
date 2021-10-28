using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChessAI.MoveSelection
{
    /**
     * <summary>
     * dummy struct representing a game state in some way
     * </summary>
     */
    public readonly struct GameState
    {
        public GameState(string state)
        {
            State = state;
        }

        //Implementation goes here...
        //TODO replace string state with actual implementation
        public string State { get; }
        
        /**
         * <summary>A dummy method representing some logic to calculate the applying a move to the state</summary>
         * <param name="move">The move that should be applied to a state</param>
         * <returns>A new <see cref="GameState"/> with the move applied</returns>
         */
        public GameState ApplyMove(string move)
        {
            //implementation goes here...
            return new GameState(move);
        }
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
        private IStateAnalyser _stateAnalyser;
        private IMoveAnalyser _moveAnalyser;
        private IMoveCalculator _moveCalculator;

        public string[] BestMoves => _bestMoves;

        public MoveSelector(bool playerIsWhite, IStateAnalyser stateAnalyser, IMoveAnalyser moveAnalyser, IMoveCalculator moveCalculator )
        {
            _isWhite = playerIsWhite;
            _bestMoves = Array.Empty<string>();
            _stateAnalyser = stateAnalyser;
            _moveAnalyser = moveAnalyser;
            _moveCalculator = moveCalculator;
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
            if (_bestMoves.Length < depth)
            {
                var newArray = new string[depth];
                _bestMoves.CopyTo(newArray, 0);
                _bestMoves = newArray;
            }

            MinMax(depth, 0, true, state);

            return _bestMoves[0];
        }

        public string BestMoveIterative(ulong timeLimit, GameState state)
        {
            //TODO implement this

            ulong elapsedTime;
            int depth = 1;
            do
            {
                var startTime = DateTime.Now;
                var task = new Task<int>(() => MinMax(depth++, 0, true, state));
                
                elapsedTime = (ulong) (DateTime.Now - startTime).TotalMilliseconds;
            } while ( (timeLimit -= elapsedTime) > 0);
            
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
         * <returns>The evaluation value of the best outcome</returns>
         */
        private int MinMax(int searchDepth, int currentDepth, bool isMaximizer, in GameState state, int alpha = Int32.MinValue, int beta = Int32.MaxValue)
        {
            if (searchDepth <= currentDepth)
            {
                return _stateAnalyser.StaticAnalysis(state: state);
            }
            
            // Generate moves, sort them and remove the previous best move to avoid
            // it being used in other branches than the best
            var moves = _moveCalculator.CalculatePossibleMoves(state, _isWhite == isMaximizer);
            _moveAnalyser.SortMovesByBest(state, moves, _bestMoves[currentDepth]);

            if (isMaximizer)
            {
                foreach (var move in moves)
                {
                    var child = state.ApplyMove(move);
                    var value = MinMax(searchDepth, currentDepth + 1, false, child, alpha, beta);

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
                    var child = state.ApplyMove(move);
                    var value = MinMax(searchDepth, currentDepth + 1, true, child, alpha, beta);

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
    }
}