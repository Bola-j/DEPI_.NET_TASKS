using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Library_System_Web_API.DTOs.Author;

namespace Library_System_Web_API.DTOs.Book
{
    public class BookWithAuthorDTO : SlimBookDTO
    {


        [JsonPropertyName("Author")]
        [JsonPropertyOrder(4)]
        public SlimAuthorDTO? Author { get; set; }
    }
}
