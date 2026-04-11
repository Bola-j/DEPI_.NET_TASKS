using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_System_Web_API.Entities
{
    public class Borrower
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly MembershipDate { get; set; }

        public ICollection<Loan> Loans { get; set; }

        public Borrower()
        {
            Loans = new List<Loan>();
        }

        public Borrower(string name, DateOnly membershipDate)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MembershipDate = membershipDate;
            Loans = new List<Loan>();
        }

        public override string ToString()
        {
            return $"Id: {Id}, Borrower: {Name}, Membership Date: {MembershipDate}";
        }
    }
}