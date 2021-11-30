using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChessAI.DataClasses;
using ChessAI.MoveSelection.MoveGeneration;
using ChessAI.MoveSelection.StateAnalysis;

namespace ChessAI.MoveSelection
{
    /**
     * <summary>
     * A Class that contains all the logic for finding the best move.
     * </summary>
     */
    public class MoveSelector
    {
        private readonly bool _isWhite;
        private Move[] _tempBestMoves;

        private readonly IStateAnalyser _stateAnalyser;
        private readonly IMoveAnalyser _moveAnalyser;
        private readonly IMoveCalculator _moveCalculator;

        public Move[] BestMoves { get; private set; }
        public int NodesVisited { get; private set; }
        public int NodesVisitedMax { get; private set; } 

        public MoveSelector(bool playerIsWhite, IStateAnalyser stateAnalyser, IMoveAnalyser moveAnalyser,
            IMoveCalculator moveCalculator, int initialMoveArraySize = 6)
        {
            _isWhite = playerIsWhite;
            BestMoves = new Move[initialMoveArraySize];
            _tempBestMoves = new Move[initialMoveArraySize];
            _stateAnalyser = stateAnalyser;
            _moveAnalyser = moveAnalyser;
            _moveCalculator = moveCalculator;

            ResetNodeCounts();
        }

        public void ResetNodeCounts()
        {
            NodesVisited = 0;
            NodesVisitedMax = 0;
        }

        /**
         * <summary>
         * The method to use if you want to perform a fixed depth search for the best move.
         * </summary>
         * <param name="depth">The depth to which the algorithm will search</param>
         * <param name="state">The current state of the game</param>
         */
        public Move BestMove(GameState state, int depth)
        {
            //TODO reduce amount of allocations and copies of arrays
            if (BestMoves.Length < depth)
            {
                var newArray = new Move[depth];
                BestMoves.CopyTo(newArray, 0);
                BestMoves = newArray;
            }

            // To avoid _bestMoves and _tempBestMoves referring to the same array,
            // _tempBestMoves have to be reassigned every call.
            _tempBestMoves = new Move[depth];


            MinMax(depth, 0, true, state);
            // BestMoves = _tempBestMoves;

            return BestMoves[0];
        }

        public Move BestMoveImproved(GameState state, int depth)
        {
            //TODO reduce amount of allocations and copies of arrays
            if (BestMoves.Length < depth)
            {
                var newArray = new Move[depth];
                BestMoves.CopyTo(newArray, 0);
                BestMoves = newArray;
            }


            MinMaxImproved(depth, 0, true, state);

            return BestMoves[0];
        }

        public Move BestMoveIterativeImproved(GameState state, TimeSpan timeLimit, int maxDepth = int.MaxValue)
        {
            var now = DateTime.Now;
            var maxTime = now.Add(timeLimit);

            for (int depth = 1; depth <= maxDepth; depth++)
            {
                if (maxTime < DateTime.Now)
                {
                    return BestMoves[0];
                }

                if (BestMoves.Length < depth)
                {
                    var newArray = new Move[Math.Min(depth * 2, maxDepth)];
                    BestMoves.CopyTo(newArray, 0);
                    BestMoves = newArray;
                }
                MinMaxImproved(depth, 0, true, state);
            }


            return BestMoves[0];
        }

        /**
         * <summary>
         * The method to use if you want to perform a search for the best move with a time constraint.
         * </summary>
         * <param name="state">The current state of the game</param>
         * <param name="timeLimit">
         * The time constraint of the search.
         * When this limit is hit the function will return and drop any potential searches it is attempting.
         * </param>
         * <param name="maxDepth">
         * The depth to which the algorithm will search if it can do so within the allotted time span.
         * </param>
         */
        public Move BestMoveIterative(GameState state, TimeSpan timeLimit, int maxDepth = int.MaxValue)
        {
            var now = DateTime.Now;
            var maxTime = now.Add(timeLimit);
            var previousRunTime = TimeSpan.Zero;

            for (int depth = 1; depth <= maxDepth; depth++)
            {
                if (maxTime.Subtract(DateTime.Now) < previousRunTime)
                {
                    return BestMoves[0];
                }

                BestMove(state, depth);
            }


            return BestMoves[0];
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
        protected int MinMax(int searchDepth, int currentDepth, bool isMaximizer, in GameState state,
            int alpha = int.MinValue, int beta = int.MaxValue)
        {
            if (searchDepth <= currentDepth)
            {
                return _stateAnalyser.StaticAnalysis(state, _isWhite);
            }

            // Generate moves, sort them and remove the previous best move to avoid
            // it being used in other branches than the best
            var moves = _moveCalculator.CalculatePossibleMoves(state, _isWhite == isMaximizer);
            _moveAnalyser.SortMovesByBest(state, moves, BestMoves[currentDepth]);

            if (isMaximizer)
            {
                for (var index = 0; index < moves.Count; index++)
                {
                    ++NodesVisited;
                    var move = moves[index];
                    var child = state.ApplyMove(move);
                    var value = MinMax(searchDepth, currentDepth + 1, false, child, alpha, beta);

                    if (value > alpha)
                    {
                        alpha = value;

                        if (alpha >= beta)
                        {
                            return alpha;
                        }

                        BestMoves[currentDepth] = move;
                    }
                }

                return alpha;
            }
            else
            {
                for (var index = 0; index < moves.Count; index++)
                {
                    ++NodesVisited;
                    var move = moves[index];
                    var child = state.ApplyMove(move);
                    var value = MinMax(searchDepth, currentDepth + 1, true, child, alpha, beta);

                    if (value < beta)
                    {
                        beta = value;

                        if (alpha >= beta)
                        {
                            return beta;
                        }

                        BestMoves[currentDepth] = move;
                    }
                }

                return beta;
            }
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
        protected int MinMaxImproved(int searchDepth, int currentDepth, bool isMaximizer, in GameState state,
            int alpha = int.MinValue, int beta = int.MaxValue)
        {
            if (searchDepth <= currentDepth)
            {
                return _stateAnalyser.StaticAnalysis(state, _isWhite);
            }

            // Generate moves, sort them and remove the previous best move to avoid
            // it being used in other branches than the best
            var moves = _moveCalculator.CalculatePossibleMoves(state, _isWhite == isMaximizer);
            _moveAnalyser.SortMovesByBest(state, moves, BestMoves[currentDepth]);

            if (isMaximizer)
            {
                Span<Piece> childBoardSpan = stackalloc Piece[0x80];
                for (var index = 0; index < moves.Count; index++)
                {
                    ++NodesVisited;
                    var move = moves[index];
                    var child = state.ApplyMove(move, childBoardSpan);
                    var value = MinMaxImproved(searchDepth, currentDepth + 1, false, child, alpha, beta);

                    if (value > alpha)
                    {
                        alpha = value;

                        if (alpha >= beta)
                        {
                            return alpha;
                        }

                        BestMoves[currentDepth] = move;
                    }
                }

                return alpha;
            }
            else
            {
                Span<Piece> childBoardSpan = stackalloc Piece[0x80];
                for (var index = 0; index < moves.Count; index++)
                {
                    ++NodesVisited;
                    var move = moves[index];
                    var child = state.ApplyMove(move, childBoardSpan);
                    var value = MinMaxImproved(searchDepth, currentDepth + 1, true, child, alpha, beta);

                    if (value < beta)
                    {
                        beta = value;

                        if (alpha >= beta)
                        {
                            return beta;
                        }

                        BestMoves[currentDepth] = move;
                    }
                }

                return beta;
            }
        }
        
        /**
         * <summary>
         * An implementation of the minMax algorithm not using alpha/beta pruning or any other tricks.
         * This should only be used for testing purposes and if it is required to visit every node.
         * </summary>
         * <param name="searchDepth">The desired depth to which it should search</param>
         * <param name="currentDepth">The depth of the current node</param>
         * <param name="isMaximizer">A value deciding which role the node takes on; maximiser or minimiser</param>
         * <param name="state">The state of the game as it would look all moves leading to this node were taken</param>
         * <param name="alpha">The value storing the maximiser's current best value</param>
         * <param name="beta">The value storing the minimiser's current best value</param>
         * <returns>The evaluation value of the best outcome</returns>
         */
        protected int MinMaxNoCutoff(int searchDepth, int currentDepth, bool isMaximizer, in GameState state)
        {
            if (searchDepth <= currentDepth)
            {
                return _stateAnalyser.StaticAnalysis(state, _isWhite);
            }

            // Generate moves, sort them and remove the previous best move to avoid
            // it being used in other branches than the best
            var moves = _moveCalculator.CalculatePossibleMoves(state, _isWhite == isMaximizer);
            
            
            if (isMaximizer)
            {
                var currentBest = int.MinValue;
                for (var index = 0; index < moves.Count; index++)
                {
                    ++NodesVisitedMax;
                    var move = moves[index];
                    var child = state.ApplyMove(move);
                    var value = MinMaxNoCutoff(searchDepth, currentDepth + 1, false, child);

                    if (value > currentBest)
                    {
                        currentBest = value;

                        BestMoves[currentDepth] = move;
                    }
                }

                return currentBest;
            }
            else
            {
                var currentBest = int.MaxValue;
                for (var index = 0; index < moves.Count; index++)
                {
                    ++NodesVisitedMax;
                    var move = moves[index];
                    var child = state.ApplyMove(move);
                    var value = MinMaxNoCutoff(searchDepth, currentDepth + 1, true, child);

                    if (value < currentBest)
                    {
                        currentBest = value;

                        BestMoves[currentDepth] = move;
                    }
                }

                return currentBest;
            }
        }
    }
}