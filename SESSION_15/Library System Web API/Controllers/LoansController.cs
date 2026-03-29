using Azure.Core;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Loan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        public LoansController(LibraryDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDTO>>> GetLoans()
        {
            var loans = await _context.Loans
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
        [HttpGet("{bookId}/{borrowerId}")]
        public async Task<ActionResult<LoanDTO>> GetLoanById(int bookId, int borrowerId)
        {
            var loan = await _context.Loans
                .Where(l => l.BookId == bookId && l.BorrowerId == borrowerId)
                .Select(l => new LoanDTO
                {

                    BorrowerId = l.BorrowerId,
                    BookId = l.BookId,
                    LoanDate = l.LoanDate,
                    ReturnDate = l.ReturnDate,
                    Borrower = new SlimBorrowerDTO
                    {
                        Id = l.Borrower.Id,
                        Name = l.Borrower.Name
                    },
                    Book = new SlimBookDTO
                    {
                        Id = l.Book.Id,
                        Title = l.Book.Title,
                        ISBN = l.Book.ISBN
                    }
                })
                .FirstOrDefaultAsync();
            if (loan == null)
            {
                return NotFound();
            }
            return Ok(loan);
        }

        [HttpPost]
        public async Task<ActionResult<LoanDTO>> CreateLoan(CreateLoanRequest request)
        {
            var book = await _context.Books.FindAsync(request.BookId);
            var borrower = await _context.Borrowers.FindAsync(request.BorrowerId);
            if (book == null || borrower == null)
            {
                return BadRequest("Invalid BookId or BorrowerId.");
            }
            if (request.LoanDate == null || request.LoanDate == default || string.IsNullOrEmpty(request.LoanDate.ToString()) || request.LoanDate.ToString() == "" || request.LoanDate.ToString() == "string" || request.LoanDate.Value <= borrower.MembershipDate)
            {
                return BadRequest("Invalid LoanDate.");
            }
        
            if (request.ReturnDate == null || request.ReturnDate == default || string.IsNullOrEmpty(request.ReturnDate.ToString()) || request.ReturnDate.ToString() == "" || request.ReturnDate.ToString() == "string" || request.ReturnDate.Value <= request.LoanDate.Value)
            {
                return BadRequest("Invalid ReturnDate.");
            }
            var loan = new Entities.Loan
            {
                BookId = request.BookId,
                BorrowerId = request.BorrowerId,
                LoanDate = request.LoanDate.Value,
                ReturnDate = request.ReturnDate.Value
            };
            
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLoanById), new { bookId = loan.BookId, borrowerId = loan.BorrowerId }, loan);
        }
        [HttpPut("{bookId:int}/{borrowerId:int}")]
        public async Task<ActionResult<SlimLoanDTO>> UpdateLoan(int bookId, int borrowerId, UpdateLoanRequest request)
        {
            if (request == null)
                return BadRequest("Request body is required.");

            var loan = await _context.Loans
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.BorrowerId == borrowerId);

            if (loan == null)
                return NotFound();

            var newBookId = (request.BookId == null || request.BookId == default || request.BookId <= 0) ? bookId : request.BookId;

            var bookExists = await _context.Books.AnyAsync(b => b.Id == newBookId);
            if (!bookExists)
                return BadRequest("Invalid BookId.");

            loan.BookId = newBookId;

            var newBorrowerId = (request.BorrowerId == null || request.BorrowerId == default || request.BorrowerId <= 0) ? borrowerId : request.BorrowerId;

            var borrower = await _context.Borrowers
                .FirstOrDefaultAsync(b => b.Id == newBorrowerId);

            if (borrower == null)
                return BadRequest("Invalid BorrowerId.");

            loan.BorrowerId = newBorrowerId;

            if (!request.LoanDate.HasValue)
                return BadRequest("LoanDate is required.");

            if (request.LoanDate.Value < borrower.MembershipDate)
                return BadRequest("LoanDate cannot be before MembershipDate.");

            loan.LoanDate = request.LoanDate.Value;

            if (!request.ReturnDate.HasValue)
                return BadRequest("ReturnDate is required.");

            if (request.ReturnDate.Value <= loan.LoanDate)
                return BadRequest("ReturnDate must be after LoanDate.");

            loan.ReturnDate = request.ReturnDate.Value;

            await _context.SaveChangesAsync();

            var dto = new SlimLoanDTO
            {
                BookId = loan.BookId,
                BorrowerId = loan.BorrowerId,
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            };

            return Ok(dto);
        }

        [HttpDelete("{bookId}/{borrowerId}")]
        public async Task<IActionResult> DeleteLoan(int bookId, int borrowerId)
        {
            var loan = await _context.Loans.FindAsync(bookId, borrowerId);
            if (loan == null)
            {
                return NotFound();
            }
            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
