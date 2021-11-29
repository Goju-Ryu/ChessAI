using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.StateAnalysis
{
    public class MoveAnalyserFastImproved : IMoveAnalyser
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

        private static int SimpleMoveAnalysis(Move move)
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
            moves.Sort((m1, m2) => 
                m1.Equals(previousBest) 
                    ? 1 
                    : m2.Equals(previousBest) 
                        ? -1 
                        : SimpleMoveAnalysis(m1) - SimpleMoveAnalysis(m2));
            
            //QuickSort(state, previousBest, CollectionsMarshal.AsSpan(moves));
        }

        private void QuickSort(in GameState state, Move previousBest, Span<Move> moves)
        {
            // Return if there are 1 or fewer elements as they cannot be sorted further
            if (moves.Length < 2) return;

            // if there are only two elements check if they are ordered, else swap before returning
            if (moves.Length == 2)
            {
                if (!moves[0].Equals(previousBest) && MoveAnalysis(state, moves[0]) < MoveAnalysis(state, moves[1]))
                {
                    Swap(moves, 0, 1);
                }
                
                return;
            }

            var pivotIndex = moves.Length - 1;
            // The element we sort the array around 
            var pivot = MoveAnalysis(state, moves[pivotIndex]);
            
            // The index at which we currently think the pivot element should be placed
            var partitionIndex = 0;

            // go through all elements except for the pivot (last element)
            for (int i = 0; i < pivotIndex; i++)
            {
                // if the element has a greater value than pivot, if it not then swap the element with that at partition index
                if (moves[i].Equals(previousBest) || MoveAnalysis(state, moves[i]) > pivot)
                {
                    Swap(moves, partitionIndex, i);
                    partitionIndex++;
                }
            }
            
            // Place pivot at the partition index as that places it with all smaller elements to one side and all greater to the other
            Swap(moves, partitionIndex, pivotIndex);
            
            //Recursively sort the two halves to either side of pivot
            QuickSort(state, previousBest, moves.Slice(0, partitionIndex));
            QuickSort(state, previousBest, moves.Slice(partitionIndex + 1));
        }

        private static void Swap(Span<Move> moves, int a, int b)
        {
            (moves[a], moves[b]) = (moves[b], moves[a]);
        }
    }
}