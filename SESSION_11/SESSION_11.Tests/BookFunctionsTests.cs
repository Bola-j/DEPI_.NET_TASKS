using SESSION_11;

namespace SESSION_11.Tests
{
    public class BookFunctionsTests
    {
        private static readonly Book _singleAuthorBook =
            new Book("111", "Design Patterns", new[] { "Gang of Four" }, new DateTime(1994, 10, 21), 49.99m);

        private static readonly Book _multiAuthorBook =
            new Book("222", "The Pragmatic Programmer", new[] { "Andrew Hunt", "David Thomas" }, new DateTime(1999, 10, 30), 39.99m);

        // ── GetTitle ─────────────────────────────────────────────────────

        [Fact]
        public void GetTitle_ReturnsCorrectTitle()
        {
            Assert.Equal("Design Patterns", BookFunctions.GetTitle(_singleAuthorBook));
        }

        [Fact]
        public void GetTitle_ReturnsCorrectTitleForDifferentBook()
        {
            Assert.Equal("The Pragmatic Programmer", BookFunctions.GetTitle(_multiAuthorBook));
        }

        // ── GetAuthors ────────────────────────────────────────────────────

        [Fact]
        public void GetAuthors_SingleAuthor_ReturnsAuthorName()
        {
            Assert.Equal("Gang of Four", BookFunctions.GetAuthors(_singleAuthorBook));
        }

        [Fact]
        public void GetAuthors_MultipleAuthors_ReturnsCommaSeparatedNames()
        {
            Assert.Equal("Andrew Hunt, David Thomas", BookFunctions.GetAuthors(_multiAuthorBook));
        }

        // ── GetPrice ──────────────────────────────────────────────────────

        [Fact]
        public void GetPrice_ReturnsStringContainingPriceValue()
        {
            var result = BookFunctions.GetPrice(_singleAuthorBook);

            Assert.Contains("49.99", result);
        }

        [Fact]
        public void GetPrice_DifferentBook_ReturnsStringContainingPriceValue()
        {
            var result = BookFunctions.GetPrice(_multiAuthorBook);

            Assert.Contains("39.99", result);
        }
    }
}
