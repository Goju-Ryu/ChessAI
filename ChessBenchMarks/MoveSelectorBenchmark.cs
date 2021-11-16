using System;
using BenchmarkDotNet.Attributes;
using ChessAI.DataClasses;
using ChessAI.MoveSelection;
using ChessAI.MoveSelection.MoveGeneration;
using ChessAI.MoveSelection.StateAnalysis;
using NUnit.Framework;
using UnitTests;

namespace BenchMarks
{
    [MemoryDiagnoser]
    public class MoveSelectorBenchmark
    {
        [Params(1, 2, 3, 4, 6)] //todo try deeper depths when they can be auto generated 
        public int Depth { get; set; }
        [Params(0, 6, 12)]
        public int InitialPathArraySize { get; set; }

        public MoveSelector MoveSelector;
        
        private IMoveAnalyser _moveAnalyser;
        private IMoveCalculator _moveCalculator;
        private IStateAnalyser _stateAnalyser;

        public MoveSelectorBenchmark()
        {
            //TODO switch out the interfaces with actual implementations once they are ready
            var moveAndStateProvider = new MoveCalculatorStateAnalyserStub();
            _moveAnalyser = new MoveAnalyserStub();
            _moveCalculator = moveAndStateProvider;
            _stateAnalyser = moveAndStateProvider;
        }

        [GlobalSetup]
        public void Setup()
        {
            MoveSelector = 
                new MoveSelector(true, _stateAnalyser, _moveAnalyser, _moveCalculator, InitialPathArraySize);
        }

        [Benchmark]
        public Move BestMove() => MoveSelector.BestMove(new GameState(), Depth);

        [Benchmark]
        public Move BestMoveIterative() =>
            MoveSelector.BestMoveIterative(new GameState(), TimeSpan.FromSeconds(30), Depth);

    }
}