using System;
using System.Linq;
using ChessAI.DataClasses;

namespace ChessAI
{
    class Program
    {
        static void Main(string[] args)
        {
            var arg = args.Length > 0 ? args[0] : "10";
            
            var gameController = new GameController(false, TimeSpan.FromSeconds(int.Parse(arg)));

            gameController.GameLoop();
        }
    }
}