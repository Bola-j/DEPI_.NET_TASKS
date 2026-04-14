using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Loan;
using System.Text.Json.Serialization;



namespace Library_System_Web_API.DTOs.Book
{
    public class BookDTO : SlimBookDTO
    {
        [JsonPropertyName("Author")]
        [JsonPropertyOrder(4)]
        public SlimAuthorDTO? Author { get; set; }
        [JsonPropertyName("Loan")]
        [JsonPropertyOrder(5)]
        public LoanWithBorrowerDTO? Loan { get; set; }
    }
}
