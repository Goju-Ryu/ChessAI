
using BenchmarkDotNet.Attributes;
using ChessAI;

namespace BenchMarks
{
    [MemoryDiagnoser]
    public class BitBoardExampleBenchMark
    {
        [Params(1, 10, 0b11111111_11111111_00000000_00000000_00000000_00000000_11111111_11111111)]
        public ulong N { get; set; }
        public BitBoardExample BitBoard;
        
       
        [GlobalSetup]
        public void Setup()
        {
            BitBoard = new BitBoardExample(N);
        }

        [Benchmark]
        public string ToString0() => BitBoard.ToString();
        [Benchmark]
        public string ToString1() => BitBoard.ToString1();
        [Benchmark]
        public string ToString2() => BitBoard.ToString2();

     
    }
}