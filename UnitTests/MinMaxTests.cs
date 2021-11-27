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
            var moveSelector = new MoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserFast(),
                new MoveCalculator()
            );

            moveSelector.BestMove(state, 6);
            
            Assert.Greater(moveSelector.MaxNodes, 0);
            Assert.Greater(moveSelector.NodesCutoff, 0);
            Assert.Greater(moveSelector.MaxNodes, moveSelector.NodesCutoff);
            
            var moveSelectorNoSort = new MoveSelector
            (
                false,
                new StateAnalyserSimple(),
                new MoveAnalyserDummy(),
                new MoveCalculator()
            );
            
            moveSelectorNoSort.BestMove(state, 6);
            
            Assert.GreaterOrEqual(moveSelectorNoSort.MaxNodes, moveSelector.MaxNodes);
            Assert.Greater(moveSelector.NodesCutoff, moveSelectorNoSort.NodesCutoff);
            Assert.Greater(moveSelector.MaxNodes, moveSelector.NodesCutoff);
        }
    }
}