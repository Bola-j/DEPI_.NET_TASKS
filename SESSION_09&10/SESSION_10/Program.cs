using System.Collections;

namespace SESSION_10
{
    internal class Program
    {
        
        static void Main(string[] args)
        {

            #region Ques. 01
            int[] arr = { 64, 34, 25, 12, 22, 11, 90 };

            Console.WriteLine("Original Array:");
            Helper<int>.PrintArray(arr);

            Helper<int>.OptimizedBubbleSort(arr);

            Console.WriteLine("\nSorted Array:");
            Helper<int>.PrintArray(arr);
            #endregion

            Console.WriteLine();
            Console.WriteLine();


            #region Ques. 02

            Console.WriteLine("\n===== Range Validation Tests =====\n");

            // Test 1: Valid construction
            Console.WriteLine("-- Test 1: Valid Range(5, 20) --");
            Range<int> range = new Range<int>(5, 20);
            Console.WriteLine($"Low: {range.Low}, High: {range.High}");

            // Test 2: Valid Low update
            Console.WriteLine("\n-- Test 2: Set Low = 10 (valid, 10 <= 20) --");
            range.Low = 10;
            Console.WriteLine($"Low: {range.Low}, High: {range.High}");

            // Test 3: Invalid Low update
            Console.WriteLine("\n-- Test 3: Set Low = 25 (invalid, 25 > 20) --");
            range.Low = 25;
            Console.WriteLine($"Low: {range.Low}, High: {range.High}");

            // Test 4: Valid High update
            Console.WriteLine("\n-- Test 4: Set High = 30 (valid, 30 >= 10) --");
            range.High = 30;
            Console.WriteLine($"Low: {range.Low}, High: {range.High}");

            // Test 5: Invalid High update
            Console.WriteLine("\n-- Test 5: Set High = 5 (invalid, 5 < 10) --");
            range.High = 5;
            Console.WriteLine($"Low: {range.Low}, High: {range.High}");

            // Test 6: Invalid construction — should throw
            Console.WriteLine("\n-- Test 6: Invalid Range(50, 10) — expects exception --");
            try
            {
                Range<int> invalidRange = new Range<int>(50, 10);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }


            #endregion

            Console.WriteLine();
            Console.WriteLine();
            #region Ques. 03
                ArrayList arrayList = new ArrayList() { 'c', 2, 2.98, true, "BolBol" };
                Console.WriteLine("Original ArrayList:");
                Console.WriteLine();

                foreach (var item in arrayList)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                Helper<int>.InPlaceReverse(arrayList);

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("Reversed ArrayList:");
                Console.WriteLine();

                foreach (var item in arrayList)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();


            #endregion


            Console.WriteLine();
            Console.WriteLine();

            #region Ques. 04
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> evenList = Helper<int>.EvenList(list);

            Console.WriteLine("Original List:");
            Console.WriteLine(string.Join(", ", list)); 
            
            Console.WriteLine("Even List:");
            Console.WriteLine(string.Join(", ", evenList));

            #endregion
            
            Console.WriteLine();
            Console.WriteLine();

            #region Ques. 05

            Console.WriteLine("===== FixedSizeList Tests =====\n");

            // Test 1: Valid construction
            Console.WriteLine("-- Test 1: Create FixedSizeList with maxSize = 3 --");
            FixedSizeList<int> fixedList = new FixedSizeList<int>(3);
            Console.WriteLine($"Max allowed size: {fixedList.MaxSize}"); // Output: Max allowed size: 3

            // Test 2: Invalid construction — maxSize <= 0
            Console.WriteLine("\n-- Test 2: Create FixedSizeList with maxSize = 0 (expects exception) --");
            try
            {
                FixedSizeList<int> invalidList = new FixedSizeList<int>(0);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Test 3: Adding items within the limit
            Console.WriteLine("\n-- Test 3: Add 3 items (all within limit) --");
            fixedList.Add(10);
            fixedList.Add(20);
            fixedList.Add(30);
            Console.WriteLine($"Items added: {string.Join(", ", fixedList)}");
            Console.WriteLine($"Count: {fixedList.Count}");

            // Test 4: Adding beyond the limit — should throw
            Console.WriteLine("\n-- Test 4: Add a 4th item (expects exception) --");
            try
            {
                fixedList.Add(40);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Test 5: Get item at valid index
            Console.WriteLine("\n-- Test 5: Get item at index 1 (expects 20) --");
            int value = fixedList.Get(1);
            Console.WriteLine($"Item at index 1: {value}");

            // Test 6: Get item at invalid index — should throw
            Console.WriteLine("\n-- Test 6: Get item at index 5 (expects exception) --");
            try
            {
                int outOfRange = fixedList.Get(5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Test 7: Method hiding — bypass via List<T> reference ⚠️
            Console.WriteLine("\n-- Test 7: Add via List<T> reference (bypasses size check!) --");
            FixedSizeList<string> strList = new FixedSizeList<string>(1);
            strList.Add("Only Item");
            List<string> baseRef = strList;   // typed as List<T>
            baseRef.Add("Sneaky Item");       // ⚠️ NO exception — size check bypassed!
            Console.WriteLine($"Count after bypass: {strList.Count}"); // prints 2!

            #endregion
            
            
            Console.WriteLine();
            Console.WriteLine();

            #region Ques. 06
            Console.Write("Enter a string: ");
            string? s = Console.ReadLine();
            Dictionary<char, int> charCount = new Dictionary<char, int>();
            char nonRepeatedChar = new char();
            foreach (char c in s)
            {
                if (charCount.ContainsKey(c))
                {
                    charCount[c]++;
                }
                else
                {
                    charCount[c] = 1;
                }
            }
            foreach (char c in charCount.Keys)
            {
                if (charCount[c] == 1)
                {
                    nonRepeatedChar = c;
                    break;
                }
            }

            Console.WriteLine($"The non-repeated character is : {nonRepeatedChar}");

            #endregion
        }
    }
}
