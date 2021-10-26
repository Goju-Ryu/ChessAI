using System;
using BenchmarkDotNet.Running;

namespace BenchMarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary =
                BenchmarkRunner
                    .Run<BitBoardExampleBenchMark>(); //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}