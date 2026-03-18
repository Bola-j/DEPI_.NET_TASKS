using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_System.Entities
{
    internal class Loan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(Book))]
        public int BookId { get; set; }


        [Required]
        [ForeignKey(nameof(Borrower))]
        public int BorrowerId { get; set; }

        // Navigations made nullable to allow EF to materialize without eager loading
        public Book? Book { get; set; }

        public Borrower? Borrower { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly LoanDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? ReturnDate { get; set; }

        public Loan() { }

        public Loan(Book book, Borrower borrower, DateOnly loanDate)
        {
            Book = book ?? throw new ArgumentNullException(nameof(book));
            BookId = book.Id;
            Borrower = borrower ?? throw new ArgumentNullException(nameof(borrower));
            BorrowerId = borrower.Id;
            LoanDate = loanDate;
        }

        public override string ToString()
        {
            return $"BookId: {BookId}, BorrowerId: {BorrowerId}, Loan Date: {LoanDate}, Return Date: {ReturnDate?.ToString() ?? "Not returned yet"}";
        }
    }
}