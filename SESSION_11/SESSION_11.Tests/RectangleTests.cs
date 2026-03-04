using SESSION_11;

namespace SESSION_11.Tests
{
    public class RectangleTests
    {
        // ── GetArea ───────────────────────────────────────────────────────

        [Fact]
        public void GetArea_ReturnsWidthTimesHeight()
        {
            var rect = new Rectangle(4, 6);

            Assert.Equal(24, rect.GetArea());
        }

        [Fact]
        public void GetArea_WithDifferentDimensions_ReturnsCorrectArea()
        {
            var rect = new Rectangle(3, 9);

            Assert.Equal(27, rect.GetArea());
        }

        // ── GetPerimeter ──────────────────────────────────────────────────

        [Fact]
        public void GetPerimeter_Returns_TwoTimes_WidthPlusHeight()
        {
            var rect = new Rectangle(4, 6);

            Assert.Equal(20, rect.GetPerimeter());
        }

        [Fact]
        public void GetPerimeter_WithDifferentDimensions_ReturnsCorrectPerimeter()
        {
            var rect = new Rectangle(5, 10);

            Assert.Equal(30, rect.GetPerimeter());
        }

        // ── Color default ─────────────────────────────────────────────────

        [Fact]
        public void Constructor_DefaultColor_IsWhite()
        {
            var rect = new Rectangle(2, 3);

            Assert.Equal("White", rect.Color);
        }

        [Fact]
        public void Constructor_CustomColor_SetsColor()
        {
            var rect = new Rectangle(2, 3, "Blue");

            Assert.Equal("Blue", rect.Color);
        }
    }
}
