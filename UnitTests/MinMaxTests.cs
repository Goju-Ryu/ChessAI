using System;
using System.Collections.Generic;
using System.Linq;
using ChessAI;
using ChessAI.MoveSelection;
using NUnit.Framework;

namespace UnitTests
{
    public class MinMaxTests
    {

        [Test]
        public void BestMoveTest()
        {
            var moveAndState = new MoveCalculatorStateAnalyserStub();
            var moveSelector = 
                new MoveSelector(true, moveAndState, new MoveAnalyserStub(), moveAndState);
            
            Assert.AreEqual("b", moveSelector.BestMove(3, new GameState("")));
            Assert.AreEqual(new[] {"b", "g", "s"}, moveSelector.BestMoves);
        }
    }

    internal class MoveAnalyserStub : IMoveAnalyser
    {
        public int MoveAnalysis(GameState state, string move)
        {
            return 0;
        }

        public void SortMovesByBest(GameState state, List<string> moves, string previousBest)
        {
            moves.Sort((s, s1) =>
                {
                    if (s.Equals(previousBest))
                    {
                        return 1;
                    }
                    else if (s1.Equals(previousBest))
                    {
                        return -1;
                    }

                    return (MoveAnalysis(state, s) - MoveAnalysis(state, s1)) % 1;
                }
            );
        }
    }

    internal class MoveCalculatorStateAnalyserStub : IMoveCalculator, IStateAnalyser
    {
        private Dictionary<string, string[]> _tree;
        private Dictionary<string, int> _nodeValues;

        public MoveCalculatorStateAnalyserStub()
        {
            _tree = new Dictionary<string, string[]>()
            {
                { "", new[] { "a", "b", "c" } },
                { "a", new[] { "d", "e", "f" } },
                { "d", new[] { "l" } },
                { "e", new[] { "m", "n", "o" } },
                { "f", new[] { "p", "q" } },
                { "b", new[] { "g", "h" } },
                { "g", new[] { "s", "t" } },
                { "h", new[] { "u", "v" } },
                { "c", new[] { "i", "j", "k" } },
                { "i", new[] { "w" } },
                { "j", new[] { "x", "y" } },
                { "k", new[] { "z", "aa", "ab" } }
            };
            _nodeValues = new Dictionary<string, int>()
            {
                {"l", 4},
                {"m", 6},
                {"n", 2},
                {"o", 6},
                {"p", 3},
                {"q", 9},
                {"s", 5},
                {"t", 2},
                {"u", 7},
                {"v", 3},
                {"w", 1},
                {"x", 7},
                {"y", 2},
                {"z", 4},
                {"aa", 6},
                {"ab", 3}
            };
        }

        public MoveCalculatorStateAnalyserStub(Dictionary<string, string[]> nodeMap, Dictionary<string, int> leafValues)
        {
            _tree = nodeMap;
            _nodeValues = leafValues;
        }

        public List<string> CalculatePossibleMoves(GameState state, bool calculateForWhite)
        {
            return _tree[state.State].ToList();
        }

        public int StaticAnalysis(GameState state)
        {
            return _nodeValues[state.State];
        }
    }
}