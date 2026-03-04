using SESSION_11;

namespace SESSION_11.Tests
{
    public class CircleTests
    {
        private const int Precision = 10; // decimal places for double comparison

        // ── GetArea ───────────────────────────────────────────────────────

        [Fact]
        public void GetArea_ReturnsPI_Times_RadiusSquared()
        {
            var circle = new Circle(5);

            Assert.Equal(Math.PI * 25, circle.GetArea(), Precision);
        }

        [Fact]
        public void GetArea_WithDifferentRadius_ReturnsCorrectArea()
        {
            var circle = new Circle(3);

            Assert.Equal(Math.PI * 9, circle.GetArea(), Precision);
        }

        // ── GetPerimeter ──────────────────────────────────────────────────

        [Fact]
        public void GetPerimeter_Returns_TwoPI_Times_Radius()
        {
            var circle = new Circle(5);

            Assert.Equal(2 * Math.PI * 5, circle.GetPerimeter(), Precision);
        }

        [Fact]
        public void GetPerimeter_WithDifferentRadius_ReturnsCorrectPerimeter()
        {
            var circle = new Circle(7);

            Assert.Equal(2 * Math.PI * 7, circle.GetPerimeter(), Precision);
        }

        // ── Color default ─────────────────────────────────────────────────

        [Fact]
        public void Constructor_DefaultColor_IsWhite()
        {
            var circle = new Circle(4);

            Assert.Equal("White", circle.Color);
        }

        [Fact]
        public void Constructor_CustomColor_SetsColor()
        {
            var circle = new Circle(4, "Red");

            Assert.Equal("Red", circle.Color);
        }
    }
}
