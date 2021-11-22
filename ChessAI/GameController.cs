using System;
using System.Linq;
using ChessAI.DataClasses;
using ChessAI.IO;
using ChessAI.MoveSelection;
using ChessAI.MoveSelection.MoveGeneration;
using ChessAI.MoveSelection.StateAnalysis;

namespace ChessAI
{
    public class GameController
    {
        private MoveSelector _moveSelector;
        private GameState _state;
        private TimeSpan _moveGenTimeOut;
        private IO.IO _io;
        private bool _isGameOver;

        public GameController(bool isPlayerWhite, TimeSpan moveGenTimeOut)
        {
            IMoveCalculator moveCalculator = new MoveGenerator();
            IMoveAnalyser moveAnalyser = new MoveAnalyserDummy();
            IStateAnalyser stateAnalyser = new StateAnalyserSimple();
            _moveSelector = new MoveSelector(isPlayerWhite, stateAnalyser, moveAnalyser, moveCalculator);

            _state = GameState.CreateNewGameState(isPlayerWhite);

            _moveGenTimeOut = moveGenTimeOut;
            _io = new IO.IO();
            _isGameOver = false;
        }


        public void GameLoop()
        {
            while (!_isGameOver)
            {
                var (command, commandText) = _io.ReadCommand();
                switch (command)
                {
                    case Command.Move:
                        PerformMoveActions(commandText[0]);
                        break;
                    case Command.Quit:
                        Environment.Exit(0);
                        break;
                    case Command.NewGame:
                        _state = GameState.CreateNewGameState(isWhite: false);
                        _isGameOver = false;
                        break;
                    case Command.Unknown:
                        _io.SendError(
                            commandText.Aggregate((s1, s2) => s1 + s2),
                            "Command not implemented"
                        );
                        break;
                }
            }
        }

        private void PerformMoveActions(string moveString)
        {
            var enemyMove = Move.Parse(moveString, _state); // TODO get move input

            _state = _state.ApplyMove(enemyMove);

            var (isGameOver, result, message) = IsGameOver(_state);
            if (isGameOver)
            {
                _isGameOver = true;
                _io.SendGameResult(result.Value, message);
            }

            var bestMove = _moveSelector.BestMoveIterative(_state, _moveGenTimeOut);
            
            _io.SendMove(bestMove);
            _state = _state.ApplyMove(bestMove);

            
        }

        private static (bool, Result?, string) IsGameOver(GameState state)
        {
            //TODO provide smarter implementation
            var blackHasKing = state.BlackPieces.Contains(new Piece(Piece.Black | Piece.King));
            if (!blackHasKing)
            {
                return (true, Result.WhiteWin, "Black lost its king");
            }
            
            var whiteHasKing = state.WhitePieces.Contains(new Piece(Piece.White | Piece.King));
            if (!whiteHasKing)
            {
                return (true, Result.BlackWin, "White lost its king");
            }
            
            return (false, null, "");
        }
    }
}