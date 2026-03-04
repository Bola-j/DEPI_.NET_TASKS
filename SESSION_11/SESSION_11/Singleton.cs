namespace SESSION_11
{
    sealed class Singleton
    {
        private static readonly Lazy<Singleton> _instance =
            new(() => new Singleton());

        // elly bada2 lazem y5las el awel mara y access el instance,
        // w ba3d keda hayb2a 3ndy el instance ready to use
        public int Value { get; set; }

        private Singleton() { }

        public static Singleton Instance => _instance.Value;
        // awel mara hay access el instance,
        // execute el lambda w create el singleton instance
    }
}
