using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Library_System_Web_API.DTOs.Book
{
    public class SlimBookDTO
    {
        [JsonPropertyName("Id")]
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyName("Book Title")]
        [JsonPropertyOrder(2)]
        public string Title { get; set; }
        [JsonPropertyName("Book ISBN")]
        [StringLength(13, MinimumLength = 10)]
        [RegularExpression(@"^(?:\d{10}|\d{13})$", ErrorMessage = "ISBN must be 10 or 13 digits.")]
        [JsonPropertyOrder(3)]
        public string? ISBN { get; set; }
    }
}
