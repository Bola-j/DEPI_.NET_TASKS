using System;
using System.Text.Json.Serialization;
using Library_System_Web_API.DTOs.Book;

namespace Library_System_Web_API.DTOs.Author
{
    public class AuthorDTO : SlimAuthorDTO
    {
        [JsonPropertyName("Books")]
        [JsonPropertyOrder(4)]
        public ICollection<SlimBookDTO> Books { get; set; }
    }
}
