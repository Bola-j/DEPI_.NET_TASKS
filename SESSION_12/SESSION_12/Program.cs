using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SESSION_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The Lists Having more than 20 elements, " +
                "the first 10 rows were only printed to keep console clean and not crowded.");
            Console.WriteLine();

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
            foreach (Product product in productsMoreThan3.Take(10))
            {
                Console.WriteLine(product.ToString());
            }
            #endregion

            Console.WriteLine();

            #region 3
                string[] Arr = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

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
            foreach (var item in res.Take(10))
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

            foreach (var item in categories.Take(10))
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

            Console.WriteLine();
            Console.WriteLine();

            #region LINQ - Ordering Operators

            
            #region 1
            var OrderedProducts = products.OrderBy(p => p.ProductName);
            Console.WriteLine("Sort a list of products by name");
            foreach (var product in OrderedProducts.Take(10))
            {
                Console.WriteLine(product.ToString());
            }
            #endregion
            
            
            Console.WriteLine();

            #region 2
            Console.WriteLine("Uses a custom comparer to do a case-insensitive sort of the words in an array. (missed)");
            string[] caseInSensitive = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };
            caseInSensitive = caseInSensitive.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToArray();
            foreach (var word in caseInSensitive) 
            {
                Console.WriteLine(word);
            }
            #endregion

            Console.WriteLine();

            #region 3
            Console.WriteLine("Sort a list of products by units in stock from highest to lowest.");
            var decreasingStockUnitsProducts = products.OrderByDescending(p => p.UnitsInStock);
            foreach (var product in decreasingStockUnitsProducts.Take(10))
            {
                Console.WriteLine(product.ToString());
            }
            #endregion

            Console.WriteLine();

            
            #region 4
            Console.WriteLine("Sort a list of digits, first by length of their name, and then alphabetically by the name itself.");
            string[] s = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            var sortedDigits = s
                .OrderBy(str => str.Length)
                .ThenBy(str => str).ToArray();

            foreach (var digit in sortedDigits)
                Console.WriteLine(digit);
            #endregion

            Console.WriteLine();

            #region 5
            Console.WriteLine("Sort first by word length and then by a case-insensitive sort of the words in an array.");

            string[] StringArr = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };
            var sortedWords = StringArr
                .OrderBy(str => str.Length)
                .ThenBy(str => str, StringComparer.OrdinalIgnoreCase).ToArray();

            foreach (var word in sortedWords)
                Console.WriteLine(word);
            #endregion

            Console.WriteLine();

            #region 6
            Console.WriteLine("Sort a list of products, first by category, and then by unit price, from highest to lowest.");
            var productsByCategoryThenPrice = products
                .OrderBy(p => p.Category)
                .ThenByDescending(p => p.UnitPrice);

            foreach (var product in productsByCategoryThenPrice.Take(10))
                Console.WriteLine(product.ToString());
            #endregion

            Console.WriteLine();

            #region 7
            Console.WriteLine("Sort first by-word length and then by " +
                "a case-insensitive descending sort of the words in an array.");

            var orderedStringArr = StringArr
                .OrderBy(str => str.Length)
                .ThenByDescending(str => str, StringComparer.OrdinalIgnoreCase).ToArray();

            foreach (var product in orderedStringArr)
                Console.WriteLine(product.ToString());
            #endregion

            Console.WriteLine();

            #region 8
            Console.WriteLine("Create a list of all digits in the array " +
                "whose second letter is 'i' that is reversed from the order in the original array.");
            string[] digitsStrings = {"zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"};

            var reversedDigits = digitsStrings
                .Where(str => str[1] == 'i')
                .Reverse()
                .ToArray();

            foreach (var digit in reversedDigits)
                Console.WriteLine(digit);
            #endregion


            #endregion

            Console.WriteLine();
            Console.WriteLine();


            #region LINQ – Transformation Operators


            #region 1
            Console.WriteLine("Return a sequence of just the names of a list of products.");
            var productNames = products
                .Select(p => p.ProductName)
                .Distinct();

            foreach(var name in productNames.Take(10))
                Console.WriteLine(name);
            #endregion

            Console.WriteLine();

            #region 2
            Console.WriteLine("Produce a sequence of the uppercase and lowercase versions " +
                "of each word in the original array (Anonymous Types).");

            string[] words = { "aPPLE", "BlUeBeRrY", "cHeRry" };
            var upperLowerWords = words
                .Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() });

            foreach (var word in upperLowerWords)
                Console.WriteLine($"Uppercase: {word.Upper}, Lowercase: {word.Lower}");
            #endregion

            Console.WriteLine();

            #region 3
            Console.WriteLine("Produce a sequence containing some properties of Products, " +
                "including UnitPrice which is renamed to Price in the resulting type.");

            var productsList = products
                .Select(p => new { p.ProductName, p.Category, Price = p.UnitPrice });
            foreach (var product in productsList.Take(10))
                Console.WriteLine($"Product: {product.ProductName}, Category: {product.Category}, Price: {product.Price}");
            #endregion

            Console.WriteLine();

            #region 4
            Console.WriteLine("Determine if the value of int in an array match their position in the array.");
            Console.WriteLine("Array is zero-indexed");
            int[] numArr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            numArr.Select((num, index) => new { Number = num, Index = index })
                .ToList()
                .ForEach(item => Console.WriteLine($"Number: {item.Number}, Index: {item.Index}, Match: {item.Number == item.Index}"));
            #endregion


            Console.WriteLine();

            #region 5
            Console.WriteLine("Returns all pairs of numbers from both arrays " +
                "such that the number from numbersA is less than the number from numbersB.");
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };
            numbersA.Join(numbersB, a => true, b => true, (a, b) => new { A = a, B = b })
                .Where(pair => pair.A < pair.B)
                .ToList()
                .ForEach(pair => Console.WriteLine($"A: {pair.A}, B: {pair.B}"));

            #endregion

            Console.WriteLine();

            #region 6

            Console.WriteLine("Select all orders where the order total is less than 50_000");
            var ordersLessThan50k = ListGenerator.CustomersList
                .SelectMany(c => c.Orders, (customer, order) => new { Customer = customer.CustomerName, Order = order })
                .Where(co => co.Order.Total < 50000);

            foreach (var item in ordersLessThan50k.Take(10))
                Console.WriteLine($"Customer: {item.Customer}, Order: {item.Order}");

            #endregion

            Console.WriteLine();

            #region 7
            Console.WriteLine("Select all orders where the order was made in 1998 or later.");

            var ordersFrom1998Onwards = ListGenerator.CustomersList
                .SelectMany(c => c.Orders, (customer, order) => new { Customer = customer.CustomerName, Order = order })
                .Where(co => co.Order.OrderDate.Year >= 1998);

            foreach (var item in ordersFrom1998Onwards.Take(10))
                Console.WriteLine($"Customer: {item.Customer}, Order: {item.Order}");

            #endregion

            #endregion
        }
    }
}
