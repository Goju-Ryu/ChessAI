using System;
using System.Diagnostics;
using ChessAI.DataClasses;
using static ChessAI.IO.Command;
using static ChessAI.IO.Result;

namespace ChessAI.IO
{
    public class IO
    {
        /// <summary>
        /// Wait for the next command and return its type and arguments if any.
        /// </summary>
        /// <returns></returns>
        public (Command, string[]) ReadCommand()
        {
            var command = Console.In.ReadLine();

            return command switch
            {
                "new" => (NewGame, Array.Empty<string>()),
                "quit" => (Quit, Array.Empty<string>()),
                "force" => (Force, Array.Empty<string>()),
                "go" => (Go, Array.Empty<string>()),
                _ => IsMoveString(command) ? (Command.Move, new[] { command }) : (Unknown, new[] { command })
            };
        }

        /// <summary>
        /// Send a move for the opponent to respond to.
        /// </summary>
        /// <param name="move">The move this engine has chosen</param>
        public void SendMove(Move move)
        {
            var moveString = "move " + Board.IndexToString(move.StartPos) + Board.IndexToString(move.EndPos);

            moveString += move.MoveType switch
            {
                MoveType.PromotionQueen => "q",
                MoveType.PromotionKnight => "k",
                MoveType.PromotionBishop => "b",
                MoveType.PromotionRook => "r",
                _ => ""
            };

            Console.Out.WriteLine(moveString);
            Console.Out.Flush();
        }

        public void SendIsDebugMode(bool isDebug)
        {
            var debugVal = isDebug ? 1 : 0;
            Console.Out.WriteLine("debug=" + debugVal);
            Console.Out.Flush();
        }

        /// <summary>
        /// Writes the state as a debug message.
        /// </summary>
        /// <param name="state">The state that is printed</param>
        public void DebugPrintBoard(GameState state)
        {
            Console.Out.WriteLine(state.State.ToString());
            Console.Out.Flush();
        }

        /// <summary>
        /// Respond to XBoard that signifies that a command was not understood
        /// </summary>
        /// <param name="command">The command received by this engine</param>
        /// <param name="message"></param>
        public void SendError(string command, string message)
        {
            Console.Out.WriteLine("Error (" + message + "): " + command);
            Console.Out.Flush();
        }

        /// <summary>
        /// Declare that the game has ended either due to a stalemate, draw, or check mate.
        /// </summary>
        /// <param name="result">The winner or draw</param>
        /// <param name="message">A message to explain how the game ended</param>
        public void SendGameResult(Result result, string message)
        {
            var winnerString = result switch
            {
                WhiteWin => "1-0",
                BlackWin => "0-1",
                Draw => "1/2-1/2",
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            };

            Console.Out.WriteLine(winnerString + " {" + message + "}");
            Console.Out.Flush();
        }

        /// <summary>
        /// This function takes a string and returns true if and only if it represents a move
        /// as described in the XBoard manual
        /// </summary>
        /// <param name="moveString">The string to be checked</param>
        /// <returns></returns>
        private static bool IsMoveString(string moveString)
        {
            //e7e8q
            var ranks = "12345678";
            var files = "abcdefgh";

            if (moveString.Length < 4 || moveString.Length > 5)
            {
                return false;
            }

            for (int i = 0; i < moveString.Length; i++)
            {
                if ((i % 2) == 0 && !files.Contains(moveString[i], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                if ((i % 2) != 0 && !ranks.Contains(moveString[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Represents the commands that this engine understands
    /// </summary>
    public enum Command
    {
        Unknown,
        NewGame,
        Quit,
        Force,
        Go,
        Move
    }

    /// <summary>
    /// Represents who the winner of a game is or a draw if neither did.
    /// </summary>
    public enum Result
    {
        WhiteWin,
        BlackWin,
        Draw
    }
}