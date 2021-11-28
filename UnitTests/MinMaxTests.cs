using System;
using ChessAI.DataClasses;
using ChessAI.MoveSelection;
using ChessAI.MoveSelection.MoveGeneration;
using ChessAI.MoveSelection.StateAnalysis;
using NUnit.Framework;

namespace UnitTests
{
    public class MinMaxTests
    {
        [Test]
        public void NodeCutoffTest()
        {
            var state = GameState.CreateNewGameState(false);
            var searchDepth = 5;
            
            var moveSelector = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );

            moveSelector.TestMinMax(searchDepth, 0, true, state);
            moveSelector.TestMinMaxNoCutoff(searchDepth, 0, true, state);

            Assert.Greater(moveSelector.NodesVisited, 0);
            Assert.Greater(moveSelector.NodesVisitedMax, 0);
            Assert.Greater(moveSelector.NodesVisitedMax, moveSelector.NodesVisited);


            var moveSelectorNoSort = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserDummy(),
                new MoveCalculator()
            );

            moveSelectorNoSort.TestMinMax(searchDepth, 0, true, state);
            moveSelectorNoSort.TestMinMaxNoCutoff(searchDepth, 0, true, state);

            Assert.Greater(moveSelectorNoSort.NodesVisited, 0);
            Assert.Greater(moveSelectorNoSort.NodesVisitedMax, 0);
            Assert.Greater(moveSelectorNoSort.NodesVisitedMax, moveSelectorNoSort.NodesVisited);
            
            Assert.AreEqual(moveSelectorNoSort.NodesVisitedMax, moveSelector.NodesVisitedMax);

            Console.WriteLine(
                $@"
Results:
    Max Nodes in search:            {moveSelector.NodesVisitedMax}
    Nodes visited without sorting:  {moveSelectorNoSort.NodesVisited}
    Nodes visited with sorting:     {moveSelector.NodesVisited}

    cutoff without sorting:         {moveSelector.NodesVisitedMax - moveSelectorNoSort.NodesVisited}
    cutoff with sorting:            {moveSelector.NodesVisitedMax - moveSelector.NodesVisited}
                "
            );
        }
    }

    class TestMoveSelector : MoveSelector
    {
        public TestMoveSelector(bool playerIsWhite, IStateAnalyser stateAnalyser, IMoveAnalyser moveAnalyser,
            IMoveCalculator moveCalculator, int initialMoveArraySize = 6) : base(playerIsWhite, stateAnalyser,
            moveAnalyser, moveCalculator, initialMoveArraySize)
        {
        }

        public int TestMinMax(int searchDepth, int currentDepth, bool isMaximizer, in GameState state,
            int alpha = int.MinValue, int beta = int.MaxValue)
        {
            return MinMax(searchDepth, currentDepth, isMaximizer, state, alpha, beta);
        }

        public int TestMinMaxNoCutoff(int searchDepth, int currentDepth, bool isMaximizer, in GameState state)
        {
            return MinMaxNoCutoff(searchDepth, currentDepth, isMaximizer, state);
        }
    }
}