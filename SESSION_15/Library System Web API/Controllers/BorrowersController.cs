using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Loan;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        public BorrowersController(LibraryDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowerDTO>>> GetBorrowers()
        {
            var borrowers = await _context.Borrowers
                .Select(b => new BorrowerDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    MembershipDate = b.MembershipDate,
                    Loans = b.Loans.Select(l => new LoanWithBookDTO
                    {
                        LoanDate = l.LoanDate,
                        ReturnDate = l.ReturnDate,
                        Book = new BookWithAuthorDTO
                        {
                            Id = l.Book.Id,
                            Title = l.Book.Title,
                            ISBN = l.Book.ISBN,
                            Author = new DTOs.Author.SlimAuthorDTO
                            {
                                Id = l.Book.Author.Id,
                                Name = l.Book.Author.Name
                            }
                        }
                    }).ToList()
                })
                .ToListAsync();
            return Ok(borrowers);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BorrowerDTO>> GetBorrowerById(int id)
        {
            var borrower = await _context.Borrowers
                .Where(b => b.Id == id)
                .Select(b => new BorrowerDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    MembershipDate = b.MembershipDate,
                    Loans = b.Loans.Select(l => new LoanWithBookDTO
                    {
                        LoanDate = l.LoanDate,
                        ReturnDate = l.ReturnDate,
                        Book = new BookWithAuthorDTO
                        {
                            Id = l.Book.Id,
                            Title = l.Book.Title,
                            ISBN = l.Book.ISBN,
                            Author = new DTOs.Author.SlimAuthorDTO
                            {
                                Id = l.Book.Author.Id,
                                Name = l.Book.Author.Name
                            }
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if(borrower == null)
            {
                return NotFound();
            }
            return Ok(borrower);

        }
        [HttpPost]
        public async Task<ActionResult<BorrowerDTO>> CreateBorrower(CreateBorrowerRequest createBorrowerRequest)
        {   
            if (string.IsNullOrEmpty(createBorrowerRequest.Name) || createBorrowerRequest.Name == "" || createBorrowerRequest.Name == "string")
            {
                return BadRequest("Name is required.");
            }
            if(createBorrowerRequest.MembershipDate == null || createBorrowerRequest.MembershipDate == default || createBorrowerRequest.MembershipDate.Value <= DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("Membership Date is required and must be a recent or past date.");
            }
            var borrower = new Entities.Borrower
            {
                Name = createBorrowerRequest.Name,
                MembershipDate = createBorrowerRequest.MembershipDate.Value
            };
            
            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();

            var borrowerDTO = new BorrowerDTO
            {
                Id = borrower.Id,
                Name = borrower.Name,
                MembershipDate = borrower.MembershipDate,
                Loans = new List<LoanWithBookDTO>()
            };
            return CreatedAtAction(nameof(GetBorrowerById), new { id = borrower.Id }, borrowerDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrower(int id, UpdateBorrowerRequest updateBorrowerRequest)
        {
            var borrower = await _context.Borrowers.FindAsync(id);
            if (borrower == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(updateBorrowerRequest.Name) || updateBorrowerRequest.Name == "" || updateBorrowerRequest.Name == "string")
            {
                return BadRequest("Name is required.");
            }
            if (updateBorrowerRequest.MembershipDate == null || updateBorrowerRequest.MembershipDate == default || updateBorrowerRequest.MembershipDate.Value <= DateOnly.FromDateTime(DateTime.Now))
            {
                return BadRequest("Membership Date is required and must be a recent or past date.");
            }
            borrower.Name = updateBorrowerRequest.Name;
            borrower.MembershipDate = updateBorrowerRequest.MembershipDate.Value;
            _context.Borrowers.Update(borrower);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrower(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);
            if (borrower == null)
            {
                return NotFound();
            }

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
