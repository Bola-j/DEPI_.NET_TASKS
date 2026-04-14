using System;
using System.Text.Json.Serialization;


namespace Library_System_Web_API.DTOs.Loan
{
    public class SlimLoanDTO
    {
        [JsonPropertyName("BookId")]
        [JsonPropertyOrder(1)]
        public int BookId { get; set; }
        [JsonPropertyName("BorrowerId")]
        [JsonPropertyOrder(2)]
        public int BorrowerId { get; set; }
        [JsonPropertyName("LoanDate")]
        [JsonPropertyOrder(3)]
        public DateOnly? LoanDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        [JsonPropertyName("ReturnDate")]
        [JsonPropertyOrder(4)]
        public DateOnly? ReturnDate { get; set; }
    }
}
