using SESSION_11;

namespace SESSION_11.Tests
{
    public class BookTests
    {
        private static Book CreateSampleBook() =>
            new Book("978-0-13-235088-4", "Clean Code", new[] { "Robert C. Martin" }, new DateTime(2008, 8, 1), 35.99m);

        // ── Constructor ──────────────────────────────────────────────────

        [Fact]
        public void Constructor_SetsAllProperties_Correctly()
        {
            var book = CreateSampleBook();

            Assert.Equal("978-0-13-235088-4", book.ISBN);
            Assert.Equal("Clean Code", book.Title);
            Assert.Equal(new[] { "Robert C. Martin" }, book.Authors);
            Assert.Equal(new DateTime(2008, 8, 1), book.PublicationDate);
            Assert.Equal(35.99m, book.Price);
        }

        [Fact]
        public void Constructor_WithMultipleAuthors_StoresAllAuthors()
        {
            var authors = new[] { "Andrew Hunt", "David Thomas" };
            var book = new Book("978-0-20-161622-4", "The Pragmatic Programmer", authors, new DateTime(1999, 10, 30), 39.99m);

            Assert.Equal(2, book.Authors.Length);
            Assert.Equal("Andrew Hunt", book.Authors[0]);
            Assert.Equal("David Thomas", book.Authors[1]);
        }

        // ── ToString ─────────────────────────────────────────────────────

        [Fact]
        public void ToString_ContainsISBNTitleAuthorDateAndPrice()
        {
            var book = CreateSampleBook();
            var result = book.ToString();

            Assert.Contains("978-0-13-235088-4", result);
            Assert.Contains("Clean Code", result);
            Assert.Contains("Robert C. Martin", result);
            Assert.Contains("2008-08-01", result);
        }

        [Fact]
        public void ToString_MultipleAuthors_JoinsWithCommaAndSpace()
        {
            var book = new Book("123", "Co-Authored Book", new[] { "Alice", "Bob", "Carol" }, DateTime.Today, 20.00m);
            var result = book.ToString();

            Assert.Contains("Alice, Bob, Carol", result);
        }
    }
}
