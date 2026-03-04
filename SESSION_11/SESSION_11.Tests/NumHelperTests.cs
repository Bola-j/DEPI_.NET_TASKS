using SESSION_11;

namespace SESSION_11.Tests
{
    public class NumHelperTests
    {
        // ── Returns true ──────────────────────────────────────────────────

        [Fact]
        public void IsMultipleOf_WhenExactlyDivisible_ReturnsTrue()
        {
            Assert.True(12.IsMultipleOf(3));
        }

        [Fact]
        public void IsMultipleOf_WhenValueIsZero_ReturnsTrue()
        {
            // 0 % any non-zero number == 0
            Assert.True(0.IsMultipleOf(5));
        }

        [Fact]
        public void IsMultipleOf_WithNegativeDivisor_ReturnsTrue()
        {
            Assert.True(12.IsMultipleOf(-4));
        }

        // ── Returns false ─────────────────────────────────────────────────

        [Fact]
        public void IsMultipleOf_WhenNotDivisible_ReturnsFalse()
        {
            Assert.False(12.IsMultipleOf(5));
        }

        [Fact]
        public void IsMultipleOf_WhenDivisorIsZero_ReturnsFalse()
        {
            // Division by zero guarded — must return false
            Assert.False(12.IsMultipleOf(0));
        }

        [Fact]
        public void IsMultipleOf_WhenRemainderIsNonZero_ReturnsFalse()
        {
            Assert.False(7.IsMultipleOf(3));
        }
    }
}
