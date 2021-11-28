using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ChessAI.DataClasses;

namespace ChessAI.MoveSelection.StateAnalysis
{
    public class MoveAnalyserFast : IMoveAnalyser
    {
        public int MoveAnalysis(GameState state, Move move)
        {
            var targetValue = move.TargetPiece.PieceType switch
            {
                Piece.Pawn => 100,
                Piece.Knight => 300,
                Piece.Bishop => 300,
                Piece.Rook => 500,
                Piece.Queen => 900,
                Piece.King => 10_000,
                _ => 0
            };

            return targetValue;
        }

        //TODO this move sorting gives too little compared to what it saves
        public void SortMovesByBest(GameState state, List<Move> moves, Move previousBest)
        {
            QuickSort(state, previousBest, CollectionsMarshal.AsSpan(moves));
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