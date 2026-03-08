using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;
using static System.Net.WebRequestMethods;

namespace SESSION_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region LINQ - Restriction Operators

            #region 1
            List<Product> products = ListGenerator.ProductsList;
            List<Product> outOfStockProducts = products.Where(p => p.UnitsInStock == 0).ToList();
            
            Console.WriteLine("Find all products that are out of stock.");
            
            foreach (Product product in outOfStockProducts) 
            {
                Console.WriteLine(product.ToString());
            }
            #endregion

            Console.WriteLine();

            #region 2
            List<Product> productsMoreThan3 = products.Where(p => (p.UnitsInStock > 0 && p.UnitPrice > 3)).ToList();
            
            Console.WriteLine("Find all products that are in stock and cost more than 3.00 per unit.");
            foreach (Product product in productsMoreThan3)
            {
                Console.WriteLine(product.ToString());
            }
            #endregion

            Console.WriteLine();

            #region 3
                String[] Arr = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

                var result = Arr.Where((s, i) => s.Length < i);
                Console.WriteLine("Returns digits whose name is shorter than their value.");
                foreach (var word in result)
                {
                    Console.WriteLine($"Word: {word}, Length: {word.Length}");
                }
            #endregion



            #endregion
            Console.WriteLine();
            Console.WriteLine();

            #region LINQ - Element Operators

            #region 1
                Product? firstOutOfStock = outOfStockProducts.FirstOrDefault();
                Console.WriteLine("Get first Product out of Stock");
                Console.WriteLine(firstOutOfStock?.ToString() ?? "No products out of stock.");
            #endregion
            Console.WriteLine();

            #region 2
                Product? firstExpensive = products.FirstOrDefault(p => p.UnitPrice > 1000);
                Console.WriteLine("Get first product whose Price > 1000:");
                Console.WriteLine(firstExpensive?.ToString() ?? "No product found with price > 1000.");
            #endregion
            Console.WriteLine();
            #region 3
            int[] a = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            int b = a.Where(n => n > 5).ElementAt(1);
            //int b = a.Where(n => n > 5).ElementAt(1);
            Console.Write("Return the second number in the array that is greater than 5: ");
            Console.WriteLine(b);
            #endregion

            #endregion
            Console.WriteLine();
            Console.WriteLine();

            #region LINQ - Aggregate Operators

            #region 1
            
            //Uses Count to get the number of odd numbers in the array
            int[] array = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            int oddCount  = array.Count(n => n % 2 == 1);
            foreach (int number in array)
                Console.WriteLine($"Number: {number}, Is Odd: {number % 2 == 1}");
            
            Console.WriteLine("Uses Count to get the number of odd numbers in the array");
            Console.WriteLine($"Odd Numbers Count is: {oddCount}");
            #endregion

            Console.WriteLine();

            #region 2

                var res = ListGenerator.CustomersList
                .Select(c => new {c.CustomerName, OrderCount = c.Orders.Length});

            Console.WriteLine("Customers and their order count:");
            foreach (var item in res)
            {
                Console.WriteLine($"Customer: {item.CustomerName}, Orders: {item.OrderCount}");
            }
            #endregion
            Console.WriteLine();
            
            #region 3

            var categories = products
                .GroupBy(p => p.Category)
                .Select(c => new { Category = c.Key, ProductCount = c.Count() });
            
            Console.WriteLine("Categories and their product count:");

            foreach (var item in categories)
            {
                Console.WriteLine($"Category: {item.Category} , NumOfProducts: {item.ProductCount}");
            }

            #endregion


            Console.WriteLine();

            #region 4
            Console.WriteLine("Get the total of the numbers in an array.");
            int[] aarrr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            //int total = aarrr.Sum();
            //Order using Aggregate

            int total = aarrr.Aggregate((sum, n) => sum + n);
            //sum is sum and n is the current number in the array
            
            Console.WriteLine($"Total of the numbers in the array: {total}");

            #endregion
            Console.WriteLine();

            #region 5

            string[] dictionary_english = System.IO.File.ReadAllLines("dictionary_english.txt");
            
            int totalChars = dictionary_english.Sum(word => word.Length);


            Console.WriteLine("Total number of characters of all words in dictionary_english.txt:");
            Console.WriteLine($"Total Characters: {totalChars}");
            #endregion
            Console.WriteLine();



            #region 6
            var lengthOfWords = dictionary_english
            .GroupBy(word => word)
            .Select(g => new { Word = g.Key, WordLength = g.Key.Length })
            .OrderBy(g => g.WordLength);

            Console.WriteLine($"Longest Length is {lengthOfWords.Last().WordLength}, and it is of the word: {lengthOfWords.Last().Word}");
            #endregion
            Console.WriteLine();


            #region 7
            Console.WriteLine($"Shortest Length is {lengthOfWords.First().WordLength}, and it is of the word: {lengthOfWords.First().Word}");

            #endregion
            Console.WriteLine();

            #region 8
                var avgLength = Math.Round(lengthOfWords.Average(g => g.WordLength));
                Console.WriteLine($"Average Length is {avgLength}");
            #endregion


            #endregion


            # region LINQ - Ordering Operators
            #endregion

            #region LINQ – Transformation Operators
            #endregion
        }
    }
}
