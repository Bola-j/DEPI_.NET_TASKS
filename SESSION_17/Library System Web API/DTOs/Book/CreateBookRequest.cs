namespace Library_System_Web_API.DTOs.Book
{
    public class CreateBookRequest
    {
    public string? Title { get; set; }
    public string? ISBN { get; set; }
    public int? AuthorId { get; set; }
    }
}
