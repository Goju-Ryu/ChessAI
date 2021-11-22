using System;
using ChessAI.DataClasses;

namespace ChessAI{
	class Program {
		static void Main(string[ ] args)
		{
			var gameController = new GameController(false, TimeSpan.FromSeconds(15));
			
			gameController.GameLoop();
		}
	}
}
