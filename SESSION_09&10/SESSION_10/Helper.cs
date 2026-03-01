using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SESSION_10
{
    internal class Helper<T> where T : IComparable<T>   
    {
        /// <summary>
        /// Optimized Bubble Sort with:
        /// 1. Early termination flag  → Best case: O(n)
        /// 2. Reduced inner boundary  → Fewer comparisons per pass
        /// Worst / Average case remains O(n²)
        /// </summary>
        public static void OptimizedBubbleSort(T[] arr)
        {
            int n = arr.Length;

            for (int i = 0; i < n - 1; i++)
            {
                bool swapped = false; // Optimization 1: track if any swap happened

                // Optimization 2: last i elements are already sorted, skip them
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (arr[j].CompareTo(arr[j + 1]) > 0)
                    {
                        (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]); // C# tuple swap
                        swapped = true;
                    }
                }

                // No swaps means the array is already sorted → stop early
                if (!swapped)
                {
                    Console.WriteLine($"\nEarly exit at pass {i + 1} — array already sorted!");
                    break;
                }
            }
        }

        public static void PrintArray(T[] arr) =>
            Console.WriteLine(string.Join(", ", arr));

        public static void InPlaceReverse(ArrayList arrayList)
        {
            int left = 0;
            int right = arrayList.Count - 1;

            while (left < right)
            {
                object? temp = arrayList[left];
                arrayList[left] = arrayList[right];
                arrayList[right] = temp;

                left++;
                right--;
            }
        }
        public static List<int> EvenList(List<int> list)
        {

            List<int> result = new List<int>();
            foreach (var i in list)
            {
                if (i % 2 == 0)
                {
                    result.Add(i);
                }
            }
            return result;
        }
    }
}
