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

            Assert.AreEqual("b", moveSelector.BestMove(new GameState(""), 3 ));
            Assert.AreEqual(new[] { "b", "g", "s" }, moveSelector.BestMoves);
        }

        [Test]
        public void BestMoveIterativeTest()
        {
            var moveAndState = new MoveCalculatorStateAnalyserStub();
            var moveSelector =
                new MoveSelector(true, moveAndState, new MoveAnalyserStub(), moveAndState);

            Assert.AreEqual(
                "b",
                moveSelector.BestMoveIterative(new GameState(""), TimeSpan.FromSeconds(2), 3)
            );

            var expectedPath = new[] { "b", "g", "s" };
            for (int i = 0; i < expectedPath.Length; ++i)
            {
                Assert.AreEqual(expectedPath[i], moveSelector.BestMoves[i]);
            }
        }
    }

    public class MoveAnalyserStub : IMoveAnalyser
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

    public class MoveCalculatorStateAnalyserStub : IMoveCalculator, IStateAnalyser
    {
        private Dictionary<string, (int, string[])> _tree;

        public MoveCalculatorStateAnalyserStub()
        {
            _tree = new Dictionary<string, (int, string[])>()
            {
                { "", (5, new[] { "a", "b", "c" }) },
                { "a", (4, new[] { "d", "e", "f" }) },
                { "d", (4, new[] { "l" }) },
                { "l", (4, Array.Empty<string>()) },
                { "e", (6, new[] { "m", "n", "o" }) },
                { "m", (6, Array.Empty<string>()) },
                { "n", (2, Array.Empty<string>()) },
                { "o", (6, Array.Empty<string>()) },
                { "f", (9, new[] { "p", "q" }) },
                { "p", (3, Array.Empty<string>()) },
                { "q", (9, Array.Empty<string>()) },
                { "b", (5, new[] { "g", "h" }) },
                { "g", (5, new[] { "s", "t" }) },
                { "s", (5, Array.Empty<string>()) },
                { "t", (2, Array.Empty<string>()) },
                { "h", (7, new[] { "u", "v" }) },
                { "u", (7, Array.Empty<string>()) },
                { "v", (3, Array.Empty<string>()) },
                { "c", (1, new[] { "i", "j", "k" }) },
                { "i", (1, new[] { "w" }) },
                { "w", (1, Array.Empty<string>()) },
                { "j", (7, new[] { "x", "y" }) },
                { "x", (7, Array.Empty<string>()) },
                { "y", (2, Array.Empty<string>()) },
                { "k", (6, new[] { "z", "aa", "ab" }) },
                { "z", (4, Array.Empty<string>()) },
                { "aa", (6, Array.Empty<string>()) },
                { "ab", (3, Array.Empty<string>()) }
            };
        }

        public MoveCalculatorStateAnalyserStub(Dictionary<string, (int, string[])> nodeMap)
        {
            _tree = nodeMap;
        }

        public List<string> CalculatePossibleMoves(GameState state, bool calculateForWhite)
        {
            return _tree[state.State].Item2.ToList();
        }

        public int StaticAnalysis(GameState state)
        {
            return _tree[state.State].Item1;
        }
    }
}