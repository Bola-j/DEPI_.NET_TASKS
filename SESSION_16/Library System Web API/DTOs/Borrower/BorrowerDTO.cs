using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Library_System_Web_API.DTOs.Loan;

namespace Library_System_Web_API.DTOs.Borrower
{
    public class BorrowerDTO : SlimBorrowerDTO
    {
        [JsonPropertyName("loans")]
        [JsonPropertyOrder(4)]
        public ICollection<LoanWithBookDTO>? Loans { get; set; }
    }
}
