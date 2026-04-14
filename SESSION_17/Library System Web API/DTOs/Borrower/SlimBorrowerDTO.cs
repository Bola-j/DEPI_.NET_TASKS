using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System;


namespace Library_System_Web_API.DTOs.Borrower
{
    public class SlimBorrowerDTO
    {
        [JsonPropertyName("Id")]
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyName("Borrower Name")]
        [JsonPropertyOrder(2)]
        public string Name { get; set; }
        [JsonPropertyName("Membership Date")]
        [JsonPropertyOrder(3)]
        public DateOnly MembershipDate { get; set; }
    }
}
