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
        private TimeSpan _moveGenTimeOut;
        private IO.IO _io;
        private bool _isGameOver;
        private bool _isPlayingWhite;

        public GameController(bool isPlayingWhite, TimeSpan moveGenTimeOut)
        {
            IMoveCalculator moveCalculator = new MoveCalculator();
            IMoveAnalyser moveAnalyser = new MoveAnalyserDummy();
            IStateAnalyser stateAnalyser = new StateAnalyserSimple();
            _moveSelector = new MoveSelector(isPlayingWhite, stateAnalyser, moveAnalyser, moveCalculator);

            _moveGenTimeOut = moveGenTimeOut;
            _io = new IO.IO();
            _isGameOver = false;
            _isPlayingWhite = isPlayingWhite;
        }


        public void GameLoop()
        {
            GameState state = GameState.CreateNewGameState(_isPlayingWhite);
            
            while (!_isGameOver)
            {
                var (command, commandText) = _io.ReadCommand();
                switch (command)
                {
                    case Command.Move:
                        PerformMoveActions(commandText[0], ref state);
                        break;
                    case Command.Quit:
                        Environment.Exit(0);
                        break;
                    case Command.NewGame:
                        state = GameState.CreateNewGameState(isWhite: false);
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

        private void PerformMoveActions(string moveString, ref GameState state)
        {
            var enemyMove = Move.Parse(moveString, state); // TODO get move input

            state = state.ApplyMove(enemyMove);

            var (isGameOver, result, message) = IsGameOver(state);
            if (isGameOver)
            {
                _isGameOver = true;
                _io.SendGameResult(result.Value, message);
            }

            // var bestMove = _moveSelector.BestMoveIterative(state, _moveGenTimeOut);
            var bestMove = _moveSelector.BestMove(state, 3);
            
            _io.SendMove(bestMove);
            state = state.ApplyMove(bestMove);
        }

        private static (bool, Result?, string) IsGameOver(GameState state)
        {
            //TODO provide smarter implementation
            var blackHasKing = state.BlackPieces.Pieces.Contains(new Piece(Piece.Black | Piece.King));
            if (!blackHasKing)
            {
                return (true, Result.WhiteWin, "Black lost its king");
            }
            
            var whiteHasKing = state.WhitePieces.Pieces.Contains(new Piece(Piece.White | Piece.King));
            if (!whiteHasKing)
            {
                return (true, Result.BlackWin, "White lost its king");
            }
            
            return (false, null, "");
        }
    }
}