using BenchmarkDotNet.Attributes;
using ChessAI.MoveSelection;
using NUnit.Framework;
using UnitTests;

namespace BenchMarks
{
    [MemoryDiagnoser]
    public class MoveSelectorBenchmark
    {
        [Params(1, 2, 4, 6)] //todo try deeper depths when they can be auto generated 
        public int Depth { get; set; }
        [Params(0, 1, 3, 6, 12)]
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
        public string BestMove() => MoveSelector.BestMove(Depth, new GameState(""));

    }
}