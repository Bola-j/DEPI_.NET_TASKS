namespace SESSION_11
{
    public class LibraryEngine
    {
        // User-defined delegate
        public static void ProcessBooks(List<Book> bList, BookFunctions.BookFunction fPtr)
        {
            foreach (Book B in bList)
            {
                Console.WriteLine(fPtr(B));
            }
        }

        //Built-in delegate
        public static void ProcessBooks(List<Book> bList, Func<Book, string> fPtr)
        {
            foreach (Book B in bList)
            {
                Console.WriteLine(fPtr(B));
            }
        }

    }
}
