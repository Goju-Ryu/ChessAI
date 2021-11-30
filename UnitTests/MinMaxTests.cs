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
        [Explicit]
        public void NodeCutoffDevelopmentMinMax()
        {
            var state1 = GameState.CreateNewGameState(false);
            var state2 = GameState.CreateNewGameState(false);
            var state3 = GameState.CreateNewGameState(false);

            var moveSelector = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );

            var moveSelectorNoSort = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserDummy(),
                new MoveCalculator()
            );

            var moveSelectorImproved = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserSimple(),
                new MoveCalculator()
            );

            for (int searchDepth = 2; searchDepth < 6; searchDepth++)
            {
                moveSelector.TestMinMax(searchDepth, 0, true, state1);
                moveSelector.TestMinMaxNoCutoff(searchDepth, 0, true, state1);

                moveSelectorNoSort.TestMinMax(searchDepth, 0, true, state2);
                moveSelectorNoSort.TestMinMaxNoCutoff(searchDepth, 0, true, state2);

                moveSelectorImproved.TestMinMax(searchDepth, 0, true, state3);

                Assert.AreEqual(moveSelectorNoSort.NodesVisitedMax, moveSelector.NodesVisitedMax);

                var maxNodes = moveSelector.NodesVisitedMax;

                var noSortNodes = moveSelectorNoSort.NodesVisited;
                var sortNodes = moveSelector.NodesVisited;
                var impSortNodes = moveSelectorImproved.NodesVisited;

                var noSortCutoff = maxNodes - noSortNodes;
                var sortCutoff = maxNodes - sortNodes;
                var impSortCutoff = maxNodes - impSortNodes;

                var noSortTotalCutPercent = (float)noSortCutoff / maxNodes * 100;
                var sortTotalCutPercent = (float)sortCutoff / maxNodes * 100;
                var impSortTotalCutPercent = (float)impSortCutoff / maxNodes * 100;

                var sortRelativeCutPercent = (float) (noSortNodes - sortNodes) / noSortNodes * 100;
                var impSortRelativeCutPercent = (float)(noSortNodes - impSortNodes) / noSortNodes * 100;


                Console.WriteLine(
                    $@"
Results with depth {searchDepth}:
    Max Nodes in search:            {maxNodes}
    Nodes visited without sorting:  {noSortNodes}
    Nodes visited with sorting:     {sortNodes}
    Nodes visited with new sorting: {impSortNodes}

    cutoff without sorting:         {noSortCutoff}({noSortTotalCutPercent}%)  
    cutoff with sorting:            {sortCutoff}({sortTotalCutPercent}%)  compared to no sort: {sortRelativeCutPercent}
    cutoff with new sorting:        {impSortCutoff}({impSortTotalCutPercent}%)  compared to no sort: {impSortRelativeCutPercent}
 
"
                );
            }
        }

        [Test]
        [Explicit]
        public void NodeCutoffTest()
        {
            var searchDepth = 5;

            var state = GameState.CreateNewGameState(false);
            var moveSelector = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );
            
            var state2 = GameState.CreateNewGameState(false);
            var moveSelector2 = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );

            moveSelector.TestMinMax(searchDepth, 0, true, state);
            moveSelector2.TestMinMaxImproved(searchDepth, 0, true, state2);
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
    cutoff with minMaxImproved:     {moveSelector.NodesVisitedMax - moveSelector2.NodesVisited}
                "
            );
        }

        [Test]
        [Explicit]
        public void NodeCutoffDevelopmentBestMoveIterative()
        {
            var state1 = GameState.CreateNewGameState(false);
            var state2 = GameState.CreateNewGameState(false);
            var state3 = GameState.CreateNewGameState(false);

            var moveSelector = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );

            var moveSelectorNoSort = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserDummy(),
                new MoveCalculator()
            );

            var moveSelectorImproved = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserSimple(),
                new MoveCalculator()
            );

            int searchDepth = 6;
            moveSelector.BestMoveIterative(state1, TimeSpan.FromSeconds(20), searchDepth);
            moveSelector.TestMinMaxNoCutoff(searchDepth, 0, true, state1);

            moveSelectorNoSort.BestMoveIterative(state2, TimeSpan.FromSeconds(20), searchDepth);
            moveSelectorNoSort.TestMinMaxNoCutoff(searchDepth, 0, true, state2);

            moveSelectorImproved.BestMoveIterative(state3, TimeSpan.FromSeconds(20), searchDepth);

            Assert.AreEqual(moveSelectorNoSort.NodesVisitedMax, moveSelector.NodesVisitedMax);

            var maxNodes = moveSelector.NodesVisitedMax;

            var noSortNodes = moveSelectorNoSort.NodesVisited;
            var sortNodes = moveSelector.NodesVisited;
            var impSortNodes = moveSelectorImproved.NodesVisited;

            var noSortCutoff = maxNodes - noSortNodes;
            var sortCutoff = maxNodes - sortNodes;
            var impSortCutoff = maxNodes - impSortNodes;

            var noSortTotalCutPercent = (float)noSortCutoff / maxNodes * 100;
            var sortTotalCutPercent = (float)sortCutoff / maxNodes * 100;
            var impSortTotalCutPercent = (float)impSortCutoff / maxNodes * 100;

            var sortRelativeCutPercent = (float)sortCutoff / noSortCutoff * 100;
            var impSortRelativeCutPercent = (float)impSortCutoff / noSortCutoff * 100;


            Console.WriteLine(
                $@"
Results with depth {searchDepth}:
    Max Nodes in search:            {maxNodes}
    Nodes visited without sorting:  {noSortNodes}
    Nodes visited with sorting:     {sortNodes}
    Nodes visited with new sorting: {impSortNodes}

    cutoff without sorting:         {noSortCutoff}({noSortTotalCutPercent}%)  
    cutoff with sorting:            {sortCutoff}({sortTotalCutPercent}%)  compared to no sort: {sortRelativeCutPercent}
    cutoff with new sorting:        {impSortCutoff}({impSortTotalCutPercent}%)  compared to no sort: {impSortRelativeCutPercent}
 
"
            );
        }

        [Test]
        public void MinMaxImprovedTest()
        {
            var state = GameState.CreateNewGameState(false);
            var searchDepth = 5;

            var moveSelector1 = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );


            var moveSelector2 = new TestMoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );

            moveSelector1.TestMinMax(searchDepth, 0, true, state);
            moveSelector2.TestMinMaxImproved(searchDepth, 0, true, state);

            Assert.Greater(moveSelector1.NodesVisited, 0);
            Assert.Greater(moveSelector2.NodesVisited, 0);
            Assert.AreEqual(moveSelector2.NodesVisitedMax, moveSelector1.NodesVisitedMax);
            Assert.AreEqual(moveSelector1.BestMoves.Length, moveSelector2.BestMoves.Length);

            for (int i = 0; i < moveSelector1.BestMoves.Length; i++)
            {
                Assert.AreEqual(moveSelector1.BestMoves[i], moveSelector2.BestMoves[i]);
            }
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

        public int TestMinMaxImproved(int searchDepth, int currentDepth, bool isMaximizer, in GameState state,
            int alpha = int.MinValue, int beta = int.MaxValue)
        {
            return MinMaxImproved(searchDepth, currentDepth, isMaximizer, state, alpha, beta);
        }

        public int TestMinMaxNoCutoff(int searchDepth, int currentDepth, bool isMaximizer, in GameState state)
        {
            return MinMaxNoCutoff(searchDepth, currentDepth, isMaximizer, state);
        }
    }
}