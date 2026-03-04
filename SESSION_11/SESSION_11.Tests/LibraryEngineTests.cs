using SESSION_11;

namespace SESSION_11.Tests
{
    [Collection("Console")]
    public class LibraryEngineTests
    {
        private static List<Book> CreateBookList() =>
            new List<Book>
            {
                new Book("111", "Book One", new[] { "Author A" }, new DateTime(2020, 1, 1), 10.00m),
                new Book("222", "Book Two", new[] { "Author B", "Author C" }, new DateTime(2021, 2, 2), 20.00m)
            };

        private static (StringWriter sw, TextWriter original) CaptureConsole()
        {
            var sw = new StringWriter();
            var original = Console.Out;
            Console.SetOut(sw);
            return (sw, original);
        }

        // ── ProcessBooks with user-defined delegate ───────────────────────

        [Fact]
        public void ProcessBooks_UserDefinedDelegate_PrintsEachBookTitle()
        {
            var (sw, original) = CaptureConsole();
            try
            {
                LibraryEngine.ProcessBooks(CreateBookList(), (BookFunctions.BookFunction)BookFunctions.GetTitle);
                var output = sw.ToString();

                Assert.Contains("Book One", output);
                Assert.Contains("Book Two", output);
            }
            finally { Console.SetOut(original); }
        }

        [Fact]
        public void ProcessBooks_UserDefinedDelegate_PrintsAuthors()
        {
            var (sw, original) = CaptureConsole();
            try
            {
                LibraryEngine.ProcessBooks(CreateBookList(), (BookFunctions.BookFunction)BookFunctions.GetAuthors);
                var output = sw.ToString();

                Assert.Contains("Author A", output);
                Assert.Contains("Author B, Author C", output);
            }
            finally { Console.SetOut(original); }
        }

        // ── ProcessBooks with Func<Book, string> delegate ─────────────────

        [Fact]
        public void ProcessBooks_FuncDelegate_PrintsISBNs()
        {
            var (sw, original) = CaptureConsole();
            try
            {
                Func<Book, string> getIsbn = b => b.ISBN;
                LibraryEngine.ProcessBooks(CreateBookList(), getIsbn);
                var output = sw.ToString();

                Assert.Contains("111", output);
                Assert.Contains("222", output);
            }
            finally { Console.SetOut(original); }
        }

        [Fact]
        public void ProcessBooks_FuncDelegate_PrintsFormattedPrices()
        {
            var (sw, original) = CaptureConsole();
            try
            {
                Func<Book, string> getPrice = b => BookFunctions.GetPrice(b);
                LibraryEngine.ProcessBooks(CreateBookList(), getPrice);
                var output = sw.ToString();

                Assert.Contains("10.00", output);
                Assert.Contains("20.00", output);
            }
            finally { Console.SetOut(original); }
        }
    }
}
