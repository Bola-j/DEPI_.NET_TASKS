using SESSION_12;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SESSION_13
{
    internal class Program
    {
            
        static int[] Arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
        static List<Product> products = ListGenerator.ProductsList;  
        static List<Customer> customers = ListGenerator.CustomersList;
        static string[] dicitonary_english = File.ReadAllLines("dictionary_english.txt");
        static void Main(string[] args)
        {

            #region LINQ - Element Operators


            #region Get first Product out of Stock
            var product = products
            .Select(p => p)
            .First(p => p.UnitsInStock == 0);

            Console.WriteLine("Get first Product out of Stock");
            Console.WriteLine(product);
            #endregion

            Console.WriteLine();
            Console.WriteLine();

            #region Return the first product whose Price > 1000, unless there is no match, in which case null is returned.
            var morethan1000Product = products
            .Select(p => p)
            .FirstOrDefault(p => p.UnitPrice > 1000);

            Console.WriteLine("Return the first product whose Price > 1000, unless there is no match, in which case null is returned.");
            Console.WriteLine(morethan1000Product);
            #endregion

            Console.WriteLine();
            Console.WriteLine();


            #region Retrieve the second number greater than 5 

            var secondGT5 = Arr
                .Where(n => n > 5)
                .ElementAt(1);

            Console.WriteLine("Retrieve the second number greater than 5 ");
            Console.WriteLine(secondGT5);
            #endregion


            #endregion

            Console.WriteLine();
            Console.WriteLine("===========================================================");
            Console.WriteLine();

            #region LINQ - Aggregate Operators


            #region Uses Count to get the number of odd numbers in the array
            Console.WriteLine("Uses Count to get the number of odd numbers in the array");
            int oddNumbersCount = Arr
                .Where(n => n % 2 != 0)
                .Count();

            Console.WriteLine(oddNumbersCount);
            #endregion

            Console.WriteLine();

            #region Return a list of customers and how many orders each has.
            Console.WriteLine("Return a list of customers and how many orders each has.");

            var customersOrdersCount = customers
                .Select(
                    c => new
                    {
                        CustomerID = c.CustomerID,
                        OrdersCount = c.Orders.Count()
                    }
                );
            foreach (var c in customersOrdersCount.Take(10))
            {
                Console.WriteLine(c);
            }
            #endregion


            Console.WriteLine();

            #region Return a list of categories and how many products each has
            Console.WriteLine("Return a list of categories and how many products each has");

            var categoriesProductsCount = products
                .Select(
                    p => new
                    {
                        Category = p.Category,
                        ProductsCount = p.Category.Count()
                    }
                );


            foreach (var cat in categoriesProductsCount)
            {
                Console.WriteLine(cat);
            }
            #endregion


            Console.WriteLine();

            #region Get the total of the numbers in an array.
            Console.WriteLine("Get the total of the numbers in an array.");
            int total = Arr.Sum();
            Console.WriteLine(total);
            #endregion


            Console.WriteLine();

            #region Get the total number of characters of all words in dictionary_english.txt 
            
            Console.WriteLine("Get the total number of characters of all words in dictionary_english.txt ");
            int totalCharacters = dicitonary_english.Sum(w => w.Length);
            Console.WriteLine(totalCharacters);
            #endregion


            Console.WriteLine();

            #region Get the length of the shortest word in dictionary_english.txt
            Console.WriteLine("Get the length of the shortest word in dictionary_english.txt");
            int shortestWordLength = dicitonary_english
                .Min(w => w.Length);
            Console.WriteLine(shortestWordLength);
            #endregion


            Console.WriteLine();

            #region Get the length of the longest word in dictionary_english.txt
            Console.WriteLine("Get the length of the longest word in dictionary_english.txt");
            int longestWordLength = dicitonary_english
                .Max(w => w.Length);
            Console.WriteLine(longestWordLength);
            #endregion


            Console.WriteLine();

            #region Get the length of the average word in dictionary_english.txt
            Console.WriteLine("Get the length of the average word in dictionary_english.txt");
            double averageWordLength = dicitonary_english
                .Average(w => w.Length);
            Console.WriteLine(averageWordLength);
            #endregion


            Console.WriteLine();

            #region Get the total units in stock for each product category. 
            Console.WriteLine("Get the total units in stock for each product category. ");

                var totalUnitsInStockByCategory = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalUnitsInStock = g.Sum(p => p.UnitsInStock)
                });

            foreach (var cat in totalUnitsInStockByCategory)
            {
                Console.WriteLine($"Category: {cat.Category}, Total Units in Stock: {cat.TotalUnitsInStock}");
            }
            #endregion


            Console.WriteLine();

            #region Get the cheapest price among each category's products  
            Console.WriteLine("Get the cheapest price among each category's products  ");
            var cheapestPriceByCategory = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    CheapestPrice = g.Min(p => p.UnitPrice)
                });
            foreach (var prod in cheapestPriceByCategory)
            {
                Console.WriteLine($"Category: {prod.Category}, Cheapest Price: {prod.CheapestPrice}");
            }
            #endregion


            Console.WriteLine();

            #region Get the products with the cheapest price in each category (Use Let)
            Console.WriteLine("Get the products with the cheapest price in each category (Use Let)");

            var cheapestProductsByCategory =
                from p in products
                group p by p.Category into g
                let cheapestPrice = g.Min(x => x.UnitPrice)
                from p in g
                where p.UnitPrice == cheapestPrice
                select new
                {
                    Category = g.Key,
                    ProductName = p.ProductName,
                    Price = p.UnitPrice
                };

            foreach (var p in cheapestProductsByCategory)
            {
                Console.WriteLine($"Category: {p.Category}, Product: {p.ProductName}, Price: {p.Price}");
            }
            #endregion


            Console.WriteLine();

            #region Get the most expensive price among each category's products.
            Console.WriteLine("Get the most expensive price among each category's products.");

            var mostExpensivePriceByCategory = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    MostExpensivePrice = g.Max(p => p.UnitPrice)
                });

            foreach (var prod in mostExpensivePriceByCategory)
            {
                Console.WriteLine($"Category: {prod.Category}, Most Expensive Price: {prod.MostExpensivePrice}");
            }
            #endregion


            Console.WriteLine();

            #region Get the products with the most expensive price in each category.
            Console.WriteLine("Get the products with the most expensive price in each category.");
            
            
            var mostExpensiveProductsByCategory =

                from p in products
                group p by p.Category into g
                let mostExpensivePrice = g.Max(x => x.UnitPrice)
                from p in g
                where p.UnitPrice == mostExpensivePrice
                select new
                {
                    Category = g.Key,
                    Product = p
                };

            foreach (var p in mostExpensiveProductsByCategory)
            {
                Console.WriteLine($"Category: {p.Category}, Product: {p.Product.ToString()}");

            }


            #endregion


            Console.WriteLine();

            #region Get the average price of each category's products.
            Console.WriteLine("Get the average price of each category's products.");

                var averagePriceByCategory = products
                .GroupBy(p => p.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    AveragePrice = g.Average(p => p.UnitPrice),
                });

            foreach (var prod in averagePriceByCategory)
            {
                Console.WriteLine($"Category: {prod.Category}, Average Price: {prod.AveragePrice}");
            }
            #endregion


            #endregion

                Console.WriteLine();
            Console.WriteLine("===========================================================");
            Console.WriteLine();


            #region LINQ - Set Operators

            #region Find the unique Category names from Product List
            Console.WriteLine("Find the unique Category names from Product List");

            var uniqueCategories = products
                .Select(p => p.Category)
                .Distinct();
            foreach (var category in uniqueCategories)
            {
                Console.WriteLine(category);
            }
            #endregion

            Console.WriteLine();

            #region  Produce a Sequence containing the unique first letter from both product and customer names.
            Console.WriteLine("Produce a Sequence containing the unique first letter from both product and customer names.");
            
            var uniqueFirstLetters = products
                .Select(p => p.ProductName[0])
                .Union(customers.Select(c => c.CustomerName[0]))
                .Distinct();

            foreach (var letter in uniqueFirstLetters)
            {
                Console.WriteLine(letter);

            }

            #endregion

            Console.WriteLine();

            #region Create one sequence that contains the common first letter from both product and customer names.
            var commonFirstLetters = products
            .Select(p => p.ProductName[0])
            .Intersect(customers.Select(c => c.CustomerName[0]));

            Console.WriteLine("Common first letters from both product and customer names:");
            foreach (var letter in commonFirstLetters)
            {
                Console.WriteLine(letter);
            }
            #endregion


            Console.WriteLine();

            #region Create one sequence that contains the first letters of product names that are not also first letters of customer names.
            Console.WriteLine("Create one sequence that contains the first letters of product names that are not also first letters of customer names.");

            var firstLettersNotInCustomers = products
                .Select(p => p.ProductName[0])
                .Except(customers.Select(c => c.CustomerName[0]));

            foreach (var letter in firstLettersNotInCustomers)
            {
                Console.WriteLine(letter);
            }

            #endregion


            Console.WriteLine();

            #region Create one sequence that contains the last Three Characters in each name of all customers and products, including any duplicates
            Console.WriteLine("Create one sequence that contains the last Three Characters in each name of all customers and products, including any duplicates");
            
            var lastThreeCharacters = products
                .Select(p => p.ProductName.Length >= 3 ? p.ProductName.Substring(p.ProductName.Length - 3) : null)
                .Union(customers.Select(c => c.CustomerName.Length >= 3 ? c.CustomerName.Substring(c.CustomerName.Length - 3) : null));
            
            foreach (var str in lastThreeCharacters)
            {
                Console.WriteLine(str);
            }

            #endregion
            #endregion

            Console.WriteLine();
            Console.WriteLine("===========================================================");
            Console.WriteLine();

            #region LINQ - Partitioning Operators
            #region Get the first 3 orders from customers in Washington
            Console.WriteLine(". Get the first 3 orders from customers in Washington");

            var first3OrdersFromWashingtonCustomers = customers
                .Where(c => c.Region == "WA")
                .SelectMany(c => c.Orders)
                .Take(3);

            foreach (var order in first3OrdersFromWashingtonCustomers)
                Console.WriteLine(order);
            #endregion

            Console.WriteLine();

            #region Get all but the first 2 orders from customers in Washington.
            Console.WriteLine("Get all but the first 2 orders from customers in Washington.");
            var allButFirst2OrdersFromWashingtonCustomers = customers
                .Where(c => c.Region == "WA")
                .SelectMany(c => c.Orders)
                .Skip(2);

            foreach (var item in allButFirst2OrdersFromWashingtonCustomers)
            {
                Console.WriteLine(item);
            }
            #endregion


            Console.WriteLine();

            #region Return elements starting from the beginning of the array until a number is hit that is less than its position in the array.
            Console.WriteLine("Return elements starting from the beginning of the array until a number is hit that is less than its position in the array.");
            
            var elementsUntilCondition = Arr
                .TakeWhile((n, index) => n >= index);

            foreach (var element in elementsUntilCondition) {
                Console.WriteLine(element);
            }
            #endregion

            Console.WriteLine();

            #region Get the elements of the array starting from the first element divisible by 3
            Console.WriteLine("Get the elements of the array starting from the first element divisible by 3.");

            var elementsFromFirstDivisibleBy3 = Arr
                .SkipWhile(n => n % 3 != 0);

            foreach (var element in elementsFromFirstDivisibleBy3) {
                Console.WriteLine(element);
            }
            #endregion

            Console.WriteLine();

            #region Get the elements of the array starting from the first element less than its position.
            Console.WriteLine("Get the elements of the array starting from the first element less than its position.");
            var elementsFromFirstLessThanPosition = Arr
                .SkipWhile((n, index) => n >= index);

            foreach (var element in elementsFromFirstLessThanPosition) {
                Console.WriteLine(element);
            }
            #endregion
            
            
            #endregion



            Console.WriteLine();
            Console.WriteLine("===========================================================");
            Console.WriteLine();

            #region LINQ - Quantifiers

            #region Determine if any of the words contain the substring 'ei'
            Console.WriteLine("Determine if any of the words contain the substring 'ei'");
            bool containsSubstring = dicitonary_english
                .Any(w => w.Contains("ei"));

            Console.WriteLine();
            #endregion
            
            Console.WriteLine();

            #region Return a grouped a list of products only for categories that have at least one product that is out of stock.
            Console.WriteLine("Return a grouped a list of products only for categories that have at least one product that is out of stock.");

            var outOfStockProductsByCategory = products
                .GroupBy(p => p.Category)
                .Where(g => g.Any(p => p.UnitsInStock == 0));


            foreach (var item in outOfStockProductsByCategory)
            {
                Console.WriteLine($"Category: {item.Key}");
                foreach (var p in item)
                {
                    Console.WriteLine($"  Product: {p.ProductName}, UnitsInStock: {p.UnitsInStock}");
                }
            }



            #endregion
            Console.WriteLine();

            #region Return a grouped a list of products only for categories that have all of their products in stock.
            Console.WriteLine("Return a grouped a list of products only for categories that have all of their products in stock.");

            var inStockProductsByCategory = products
                .GroupBy(p => p.Category)
                .Where(g => g.All(p => p.UnitsInStock > 0));

            foreach (var item in inStockProductsByCategory)
                {
                Console.WriteLine($"Category: {item.Key}");
                foreach (var p in item)
                {
                    Console.WriteLine($"  Product: {p.ProductName}, UnitsInStock: {p.UnitsInStock}");
                }
            }
            #endregion
            #endregion




            Console.WriteLine();
            Console.WriteLine("===========================================================");
            Console.WriteLine();

            #region LINQ - Grouping Operators

            #region 1.	Use group by to partition a list of numbers by their remainder when divided by 5
            Console.WriteLine("1.\tUse group by to partition a list of numbers by their remainder when divided by 5");
            List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

           var groupedByRemainder = numbers
                .GroupBy(n => n % 5)
                .Select(g => new
                {
                    Remainder = g.Key,
                    Numbers = g.ToList()
                });

            foreach (var group in groupedByRemainder)
            {
                Console.WriteLine($"Remainder: {group.Remainder}");
                Console.WriteLine($"Numbers: {string.Join(", ", group.Numbers)}");
            }
            #endregion

            Console.WriteLine();

            #region 2.	Uses group by to partition a list of words by their first letter.
            Console.WriteLine("2.\tUses group by to partition a list of words by their first letter.");
            
            var groupedByFirstLetter = dicitonary_english
                .GroupBy(w => w[0])
                .Select(g => new
                {
                    FirstLetter = g.Key,
                    Words = g.ToList()
                });

            foreach (var group in groupedByFirstLetter)
            {
                Console.WriteLine($"First Letter: {group.FirstLetter}");
                Console.WriteLine($"Words: {string.Join(", ", group.Words.Take(10))}"); // Print only first 10 words for brevity
            }
            #endregion


            Console.WriteLine();

            #region Use Group By with a custom comparer that matches words that are consists of the same Characters Together
            Console.WriteLine("Use Group By with a custom comparer that matches words that are consists of the same Characters Together");
            string[] stringArr = { "from", "salt", "earn", " last", "near", "form" };


            var groupedBySameCharacters = stringArr
                .GroupBy(w => new string(w.OrderBy(c => c).ToArray()))
                .Select(g => new
                {
                    SortedCharacters = g.Key,
                    Words = g.ToList()
                });

            foreach (var group in groupedBySameCharacters)
                {
                Console.WriteLine($"Sorted Characters: {group.SortedCharacters}");
                Console.WriteLine($"Words: {string.Join(", ", group.Words)}");
                }
            #endregion

            #endregion

        }
    }
}
