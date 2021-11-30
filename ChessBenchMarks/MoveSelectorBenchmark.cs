using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using ChessAI.DataClasses;
using ChessAI.MoveSelection;
using ChessAI.MoveSelection.MoveGeneration;
using ChessAI.MoveSelection.StateAnalysis;

namespace BenchMarks
{
    [MemoryDiagnoser]
    public class MoveSelectorBenchmark
    {
        [Params( 5, 6)] public int Depth { get; set; }

        // [Params(0, 3, 6)] public int InitialPathArraySize { get; set; }

        [ParamsSource(nameof(MoveAnalyserSource))]
        public IMoveAnalyser MoveAnalyser { get; set; }

        private IMoveCalculator _moveCalculator;
        private IStateAnalyser _stateAnalyser;
        private MoveSelector _moveSelector;


        public IEnumerable<IMoveAnalyser> MoveAnalyserSource()
        {
            yield return new MoveAnalyserDummy();
            // yield return new MoveAnalyserFast();
        }


        public MoveSelectorBenchmark()
        {
            _moveCalculator = new MoveCalculator();
            _stateAnalyser = new StateAnalyserSimple();
        }

        [GlobalSetup]
        public void SetUp()
        {
            _moveSelector = new MoveSelector(false, _stateAnalyser, MoveAnalyser, _moveCalculator,
                0);
        }


        [Benchmark]
        public Move BestMove()
        {
            return _moveSelector.BestMove(GameState.CreateNewGameState(false), Depth);
        }

        [Benchmark]
        public Move BestMoveImproved()
        {
            return _moveSelector.BestMoveImproved(GameState.CreateNewGameState(false), Depth);
        }

        // [Benchmark]
        // public Move BestMoveIterative()
        // {
        //     return _moveSelector.BestMoveIterative(
        //         GameState.CreateNewGameState(false), TimeSpan.FromSeconds(30), Depth);
        // }
        //
        // [Benchmark]
        // public Move BestMoveIterativeImproved()
        // {
        //     return _moveSelector.BestMoveIterativeImproved(GameState.CreateNewGameState(false),
        //         TimeSpan.FromSeconds(30), Depth);
        // }
    }
}