using System.Drawing;

namespace SESSION_11
{
    internal class Program
    {
        static void Main(string[] args)
        {

            #region Ques. 01 - Excercise each mentioned point
            
            #region A. Singleton Design Pattern
            var s1 = Singleton.Instance;
            var s2 = Singleton.Instance;

            s1.Value = 42;

            Console.WriteLine($"Same instance: {ReferenceEquals(s1, s2)}"); // True
            Console.WriteLine($"s2.Value: {s2.Value}");                      // 42
            Console.WriteLine($"s1 HashCode: {s1.GetHashCode()}");
            Console.WriteLine($"s2 HashCode: {s2.GetHashCode()}");
            Console.WriteLine($"Same HashCode: {s1.GetHashCode() == s2.GetHashCode()}");
            #endregion
            
            Console.WriteLine();
            Console.WriteLine();

            #region B. Ananyomous Type
            // Anonymous type with two properties

            var Dog1 = new
            {
                Name = "LOLO",
                Age = 5 
            };
            // Anonymous type with the same properties but different values
            var Dog2 = new
            {
                Name = "MILO",
                Age = 2
            };

            var Dog3 = new
            {
                Name = "NONO",
                Age = 5,
                Color = "Brown"
            };

            Console.WriteLine($"The Dog {Dog1.Name} is wiser than {Dog2.Name}");
            //Console.WriteLine($"{Dog3.Name}'s color is {Dog3.Color} and {Dog1.Name}'s color is {Dog1.Color}");
            // the new added property in Dog3 is not exist in Dog1 and Dog2 so we can't access it from them

            #endregion

            Console.WriteLine();
            Console.WriteLine();

            #region Extension Methods

            int number = 12;
            Console.WriteLine($"Is {number} a multiple of 3? {number.IsMultipleOf(3)}");   // True
            Console.WriteLine($"Is {number} a multiple of 5? {number.IsMultipleOf(5)}");   // False
            Console.WriteLine($"Is {number} a multiple of -4? {number.IsMultipleOf(-4)}"); // True
            Console.WriteLine($"Is 0 a multiple of 3? {0.IsMultipleOf(3)}");               // True
            Console.WriteLine($"Is {number} a multiple of 0? {number.IsMultipleOf(0)}");   // False

            #endregion

            Console.WriteLine();
            Console.WriteLine();

            #region Delegates

            #region Defined Delegates

            Circle    c1 = new Circle(5, "Red");
            Rectangle r1 = new Rectangle(4, 6, "Blue");

            // MeasureOp — same delegate type reused for two different methods
            Shape.MeasureOp getArea      = s => s.GetArea();
            Shape.MeasureOp getPerimeter = s => s.GetPerimeter();
            
            Console.WriteLine("==================================");

            Console.WriteLine($"{c1.Name} Area      : {getArea(c1):F2}");
            Console.WriteLine($"{r1.Name} Area   : {getArea(r1):F2}");
            Console.WriteLine($"{c1.Name} Perimeter : {getPerimeter(c1):F2}");
            Console.WriteLine($"{r1.Name} Perimeter : {getPerimeter(r1):F2}");


            Console.WriteLine("==================================");

            // ShapePrinter — void delegate
            Shape.ShapePrinter print = s => s.PrintInfo();
            print(c1);
            print(r1);

            Console.WriteLine("==================================");

            // ShapeComparator — bool delegate
            Shape.ShapeComparator isLarger = (a, b) => a.IsLargerThan(b);
            Console.WriteLine($"Is {c1.Name} larger than {r1.Name}? {isLarger(c1, r1)}");
            Console.WriteLine("==================================");

            #endregion

            Console.WriteLine();

            #region Built-In Delegates

            Func<Shape, double> getAreaFunc      = s => s.GetArea();
            Func<Shape, double> getPerimeterFunc = s => s.GetPerimeter();

            Console.WriteLine("==================================");
            Console.WriteLine($"{c1.Name} Area      : {getAreaFunc(c1):F2}");
            Console.WriteLine($"{r1.Name} Area      : {getAreaFunc(r1):F2}");
            Console.WriteLine($"{c1.Name} Perimeter : {getPerimeterFunc(c1):F2}");
            Console.WriteLine($"{r1.Name} Perimeter : {getPerimeterFunc(r1):F2}");

            Action<Shape> printAction = s => s.PrintInfo();

            Console.WriteLine("==================================");
            printAction(c1);
            printAction(r1);


            Func<Shape, Shape, bool> isLargerFunc = (a, b) => a.GetArea() > b.GetArea();

            Console.WriteLine("==================================");
            Console.WriteLine($"Is {c1.Name} larger than {r1.Name}? {isLargerFunc(c1, r1)}");
            Console.WriteLine("==================================");

            #endregion

            #endregion

            #endregion

            Console.WriteLine();
            Console.WriteLine("============================================================");
            Console.WriteLine();
            #region Ques. 02  - Book Functions

            var books = new List<Book>
            {
                new Book("111", "Clean Code",         new[] { "Robert Martin" },          new DateTime(2008, 8, 1),  45.99m),
                new Book("222", "The Pragmatic Programmer", new[] { "Hunt", "Thomas" },   new DateTime(1999, 10, 1), 52.00m),
                new Book("333", "Design Patterns",    new[] { "GoF" },                    new DateTime(1994, 11, 1), 60.00m),
            };

            // a) User-defined delegate
            BookFunctions.BookFunction titlePtr   = BookFunctions.GetTitle;
            BookFunctions.BookFunction authorsPtr = BookFunctions.GetAuthors;
            BookFunctions.BookFunction pricePtr   = BookFunctions.GetPrice;

            Console.WriteLine("--- Titles (User-Defined Delegate) ---");
            LibraryEngine.ProcessBooks(books, titlePtr);

            Console.WriteLine("--- Authors (User-Defined Delegate) ---");
            LibraryEngine.ProcessBooks(books, authorsPtr);

            Console.WriteLine("--- Prices (User-Defined Delegate) ---");
            LibraryEngine.ProcessBooks(books, pricePtr);

            Console.WriteLine();

            // b) Built-in delegate (Func<Book, string>)
            Func<Book, string> titleFunc   = BookFunctions.GetTitle;
            Func<Book, string> authorsFunc = BookFunctions.GetAuthors;
            Func<Book, string> priceFunc   = BookFunctions.GetPrice;

            Console.WriteLine("--- Titles (Built-In Delegate) ---");
            LibraryEngine.ProcessBooks(books, titleFunc);

            Console.WriteLine("--- Authors (Built-In Delegate) ---");
            LibraryEngine.ProcessBooks(books, authorsFunc);

            Console.WriteLine("--- Prices (Built-In Delegate) ---");
            LibraryEngine.ProcessBooks(books, priceFunc);

            Console.WriteLine();

            // Anonymous functions 
            Console.WriteLine("--- ISBNs (Anonymous Function) ---");
            LibraryEngine.ProcessBooks(books, (Func<Book, string>)delegate(Book b) { return b.ISBN; });

            // Lambda expressions

            Console.WriteLine("--- Publication Dates (Lambda Expression) ---");
            LibraryEngine.ProcessBooks(books, (Func<Book, string>)(b => b.PublicationDate.ToShortDateString()));
            #endregion
            Console.WriteLine();
            Console.WriteLine("============================================================");
            Console.WriteLine();
        }
    }

    }
