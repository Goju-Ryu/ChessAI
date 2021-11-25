using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using ChessAI.MoveSelection.StateAnalysis;
using NUnit.Framework;

namespace UnitTests.MoveSelection.StateAnalysis
{
    
    /// <summary>
    /// This test tests the implementation of QuickSort found in <see cref="MoveAnalyserFast"/>.
    /// This test contains a copy of the method, made to use ints as that is easier to test. Besides the types nothing
    /// is changed.
    /// </summary>
    public class QuickSortTests
    {


        [Test]
        public void MultipleQuickSortTests()
        {
            for (int i = 0; i < 10_000; i++)
            {
                QuickSortTest();
            }
        }
        
        
        
        public void QuickSortTest()
        {
            var values = GenerateRandomValues(5, 10);
            var immutableValues = values.ToImmutableList();

            QuickSort(-1, CollectionsMarshal.AsSpan(values));

            Assert.IsTrue(values.Count == immutableValues.Count);
            Assert.IsTrue(values.TrueForAll(immutableValues.Contains));

            var immutableSortedValues = immutableValues.Sort((a, b) => b - a);
            
            // //To be deleted
            // var newUnsortedValues = immutableValues.ToList();
            // QuickSort(-1, CollectionsMarshal.AsSpan(newUnsortedValues));
            //
            // ///
            
            
            Assert.IsTrue(values.Count == immutableSortedValues.Count);

            for (int i = 0; i < immutableSortedValues.Count; i++)
            {
                Assert.AreEqual(immutableSortedValues[i], values[i]);
            }
        }

        private static List<int> GenerateRandomValues(int minSize, int maxSize, int maxValue = 9)
        {
            var rand = new Random();

            var valArr = new int[rand.Next(minSize, maxSize)];

            for (int i = 0; i < valArr.Length; i++)
            {
                valArr[i] = rand.Next(0, maxValue);
            }

            return valArr.ToList();
        }

        private void QuickSort(in int previousBest, Span<int> moves)
        {
            // Return if there are 1 or fewer elements as they cannot be sorted further
            if (moves.Length < 2) return;

            // if there are only two elements check if they are ordered, else swap before returning
            if (moves.Length == 2)
            {
                if (!moves[0].Equals(previousBest) && moves[0] < moves[1])
                {
                    Swap(moves, 0, 1);
                }
                
                return;
            }

            var pivotIndex = moves.Length - 1;
            // The element we sort the array around 
            var pivot =moves[pivotIndex];
            
            // The index at which we currently think the pivot element should be placed
            var partitionIndex = 0;

            // go through all elements except for the pivot (last element)
            for (int i = 0; i < pivotIndex; i++)
            {
                // if the element has a greater value than pivot, if it not then swap the element with that at partition index
                if (moves[i].Equals(previousBest) || moves[i] > pivot)
                {
                    Swap(moves, partitionIndex, i);
                    partitionIndex++;
                }
            }
            
            // Place pivot at the partition index as that places it with all smaller elements to one side and all greater to the other
            Swap(moves, partitionIndex, pivotIndex);
            
            //Recursively sort the two halves to either side of pivot
            QuickSort(previousBest, moves.Slice(0, partitionIndex));
            QuickSort(previousBest, moves.Slice(partitionIndex + 1));
        }

        private static void Swap(Span<int> moves, int a, int b)
        {
            (moves[a], moves[b]) = (moves[b], moves[a]);
        }
    }
}