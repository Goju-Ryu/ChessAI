using System;
using System.Collections.Generic;
using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.StateAnalysis
{
    public class MoveAnalyserSimple : IMoveAnalyser
    {

        private static int PieceValue(Piece piece)
        {
            return piece.PieceType switch
            {
                Piece.Pawn => 100,
                Piece.Knight => 300,
                Piece.Bishop => 300,
                Piece.Rook => 500,
                Piece.Queen => 900,
                Piece.King => 10_000,
                _ => 0
            };
        }
        public int MoveAnalysis(GameState state, Move move)
        {
            // Gives a static value for the piece type, but if the piece challenges an opponent then a static
            // modifier is added. 100 was chosen as this makes a piece more important than others of its type but not
            // more important than the next tier. I intuit that this will more often be the case though the assumption 
            // has not yet been tested.
            var targetValue = 
                PieceValue(move.TargetPiece) + (move.TargetPiece.HasFlag(Piece.ChallengesOtherFlag) ? 100 : 0);
            
            var moveValue = move.MovePiece.HasFlag(Piece.ChallengedFlag) 
                ? PieceValue(move.MovePiece) + targetValue 
                : targetValue;
           

            return moveValue;
        }
        
        public void SortMovesByBest(GameState state, List<Move> moves, Move previousBest)
        {
            var prevBestIndex = moves.FindIndex((move) => move.Equals(previousBest));
            if(prevBestIndex == -1) return; // If previous best isn't in moves then return
            
            //Put previous best in front of the list
            var temp = moves[0];
            moves[0] = previousBest;
            moves[prevBestIndex] = temp;
        }
        
    }
}