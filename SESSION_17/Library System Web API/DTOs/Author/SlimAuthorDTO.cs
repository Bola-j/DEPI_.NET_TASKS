using System;
using System.Text.Json.Serialization;


namespace Library_System_Web_API.DTOs.Author
{
    public class SlimAuthorDTO
    {
        [JsonPropertyName("Id")]
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyName("Author Name")]
        [JsonPropertyOrder(2)]
        public string Name { get; set; }
        [JsonPropertyName("Author BirthDate")]
        [JsonPropertyOrder(3)]
        public DateOnly BirthDate { get; set; }
    }
}
