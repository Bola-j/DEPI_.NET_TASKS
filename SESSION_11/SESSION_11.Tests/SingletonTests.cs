using SESSION_11;

namespace SESSION_11.Tests
{
    public class SingletonTests
    {
        // ── Instance identity ─────────────────────────────────────────────

        [Fact]
        public void Instance_CalledTwice_ReturnsSameReference()
        {
            var s1 = Singleton.Instance;
            var s2 = Singleton.Instance;

            Assert.Same(s1, s2);
        }

        [Fact]
        public void Instance_HashCode_IsIdenticalForBothReferences()
        {
            var s1 = Singleton.Instance;
            var s2 = Singleton.Instance;

            Assert.Equal(s1.GetHashCode(), s2.GetHashCode());
        }

        // ── Value persistence ─────────────────────────────────────────────

        [Fact]
        public void Value_SetOnOneReference_IsVisibleOnOtherReference()
        {
            var s1 = Singleton.Instance;
            var s2 = Singleton.Instance;

            s1.Value = 42;

            Assert.Equal(42, s2.Value);
        }

        [Fact]
        public void Value_CanBeUpdated_ReflectsNewValueOnAllReferences()
        {
            var s1 = Singleton.Instance;
            var s2 = Singleton.Instance;

            s1.Value = 10;
            s2.Value = 99;

            Assert.Equal(99, s1.Value);
        }
    }
}
