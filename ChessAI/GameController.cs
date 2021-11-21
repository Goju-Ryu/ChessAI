using System;
using ChessAI.DataClasses;
using  ChessAI.IO;
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

        public GameController(bool isPlayerWhite, TimeSpan moveGenTimeOut)
        {
            IMoveCalculator moveCalculator = new MoveGenerator();
            IMoveAnalyser moveAnalyser = new MoveAnalyserDummy();
            IStateAnalyser stateAnalyser = new StateAnalyserSimple();
            _moveSelector = new MoveSelector(isPlayerWhite, stateAnalyser, moveAnalyser, moveCalculator);

            _state = GameState.CreateNewGameState();
            
            _moveGenTimeOut = moveGenTimeOut;
        }


        public void GameLoop()
        {
            bool hasEnded = false;

            while (!hasEnded)
            {
                var move = Move.Parse("", _state);  // TODO get move input

                _state = _state.ApplyMove(move);

                var bestMove = _moveSelector.BestMoveIterative(_state, _moveGenTimeOut);

                _state = _state.ApplyMove(bestMove);

                if (!IsGameOver(_state))
                {
                    //TODO send move to opponent
                }
                else
                {
                    hasEnded = true;
                    //TODO send a message about game over
                }
            }

        }

        private static bool IsGameOver(GameState state)
        {
            return false; //TODO implement this 
        }
    }
}