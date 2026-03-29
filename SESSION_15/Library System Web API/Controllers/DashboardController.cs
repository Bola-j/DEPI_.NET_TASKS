using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Data;
using Microsoft.EntityFrameworkCore;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Loan;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        public DashboardController(LibraryDBContext context)
        {
            _context = context;
        }

        [HttpGet("Loans/DueDate")]
        public async Task<ActionResult<IEnumerable<LoanDTO>>> GetLoansDue(DateOnly date)
        {
            var loans = await _context.Loans
                .Where(l => l.ReturnDate == date)
                .Select(l => new LoanDTO
                {
                    BorrowerId = l.BorrowerId,
                    BookId = l.BookId,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate,
                    Borrower = new SlimBorrowerDTO
                    {
                        Id = l.Borrower.Id,
                        Name = l.Borrower.Name,
                        MembershipDate = l.Borrower.MembershipDate
                    },
                    Book = new SlimBookDTO
                    {
                        Id = l.Book.Id,
                        Title = l.Book.Title,
                        ISBN = l.Book.ISBN
                    }
                })
                .ToListAsync();
            return Ok(loans);
        }
        
    }
}