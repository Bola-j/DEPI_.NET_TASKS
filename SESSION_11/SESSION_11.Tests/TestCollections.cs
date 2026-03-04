namespace SESSION_11.Tests
{
    /// <summary>
    /// Serializes test classes that redirect Console.Out so they never run in parallel.
    /// </summary>
    [CollectionDefinition("Console")]
    public class ConsoleCollection { }
}
