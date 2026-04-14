namespace Library_System_Web_API.DTOs.Author
{
    public class CreateAuthorRequest
    {
        public string? Name { get; set; }
        public DateOnly? Birthdate { get; set; }
    }
}
