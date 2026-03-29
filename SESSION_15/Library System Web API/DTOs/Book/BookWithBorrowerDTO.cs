using Library_System_Web_API.DTOs.Loan;

using System.Text.Json.Serialization;

namespace Library_System_Web_API.DTOs.Book
{
    public class BookWithBorrowerDTO : SlimBookDTO
    {
        [JsonPropertyName("Loan")]
        [JsonPropertyOrder(4)]
        public LoanWithBorrowerDTO? Loan { get; set; }
    }
}
