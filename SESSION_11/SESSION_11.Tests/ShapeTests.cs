using SESSION_11;

namespace SESSION_11.Tests
{
    [Collection("Console")]
    public class ShapeTests
    {
        // ── IsLargerThan ──────────────────────────────────────────────────

        [Fact]
        public void IsLargerThan_WhenShapeHasLargerArea_ReturnsTrue()
        {
            var large = new Circle(10);   // area ≈ 314.16
            var small = new Circle(3);    // area ≈ 28.27

            Assert.True(large.IsLargerThan(small));
        }

        [Fact]
        public void IsLargerThan_WhenShapeHasSmallerArea_ReturnsFalse()
        {
            var small = new Rectangle(2, 3);  // area = 6
            var large = new Circle(10);       // area ≈ 314.16

            Assert.False(small.IsLargerThan(large));
        }

        [Fact]
        public void IsLargerThan_CrossShapeComparison_ReturnsTrue()
        {
            var rect  = new Rectangle(20, 20); // area = 400
            var circle = new Circle(5);        // area ≈ 78.54

            Assert.True(rect.IsLargerThan(circle));
        }

        [Fact]
        public void IsLargerThan_WhenAreasAreEqual_ReturnsFalse()
        {
            var r1 = new Rectangle(4, 6);  // area = 24
            var r2 = new Rectangle(3, 8);  // area = 24

            Assert.False(r1.IsLargerThan(r2));
        }

        // ── PrintInfo ─────────────────────────────────────────────────────

        [Fact]
        public void PrintInfo_Circle_WritesNameAndColorToConsole()
        {
            var circle   = new Circle(5, "Red");
            var sw       = new StringWriter();
            var original = Console.Out;
            Console.SetOut(sw);
            try
            {
                circle.PrintInfo();
                var output = sw.ToString();

                Assert.Contains("Circle", output);
                Assert.Contains("Red", output);
            }
            finally { Console.SetOut(original); }
        }

        [Fact]
        public void PrintInfo_Rectangle_WritesNameAndColorToConsole()
        {
            var rect     = new Rectangle(4, 6, "Blue");
            var sw       = new StringWriter();
            var original = Console.Out;
            Console.SetOut(sw);
            try
            {
                rect.PrintInfo();
                var output = sw.ToString();

                Assert.Contains("Rectangle", output);
                Assert.Contains("Blue", output);
            }
            finally { Console.SetOut(original); }
        }
    }
}
