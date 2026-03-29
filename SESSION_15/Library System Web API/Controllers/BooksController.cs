using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Loan;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Borrower;
using Microsoft.EntityFrameworkCore;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        public BooksController(LibraryDBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var books = await _context.Books
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Author = new SlimAuthorDTO
                    {
                        Id = b.Author.Id,
                        Name = b.Author.Name,
                        BirthDate = b.Author.BirthDate
                    },
                    Loan = b.Loans
                        .Select(l => new LoanWithBorrowerDTO
                        {
                            Borrower = new SlimBorrowerDTO
                            {
                                Id = l.Borrower.Id,
                                Name = l.Borrower.Name,
                                MembershipDate = l.Borrower.MembershipDate
                            },
                            LoanDate = l.LoanDate,
                            ReturnDate = l.ReturnDate
                        })
                        .FirstOrDefault()
                }

                )
                .ToListAsync();

            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBook(int id)
        {
            var book = await _context.Books
                .Where(b => b.Id == id)
                .Select(b => new BookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Author = new SlimAuthorDTO
                    {
                        Id = b.Author.Id,
                        Name = b.Author.Name,
                        BirthDate = b.Author.BirthDate
                    },
                    Loan = b.Loans
                        .Select(l => new LoanWithBorrowerDTO
                        {
                            Borrower = new SlimBorrowerDTO
                            {
                                Id = l.Borrower.Id,
                                Name = l.Borrower.Name,
                                MembershipDate = l.Borrower.MembershipDate
                            },
                            LoanDate = l.LoanDate,
                            ReturnDate = l.ReturnDate
                        })
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);

        }

        [HttpPost]
        public async Task<ActionResult<SlimBookDTO>> CreateBook(CreateBookRequest request)
        {
            var author = await _context.Authors.FindAsync(request.AuthorId);
            if (author == null)
            {
                return BadRequest($"Author with ID {request.AuthorId} not found.");
            }
            if (string.IsNullOrEmpty(request.Title) || request.Title == "" || request.Title == "string")
            {
                return BadRequest("Title is required.");
            }
            if ((string.IsNullOrEmpty(request.ISBN) || request.ISBN == "" || request.ISBN == "string") && !System.Text.RegularExpressions.Regex.IsMatch(request.ISBN, @"^(?:\d{10}|\d{13})$"))
            {
                return BadRequest("ISBN is required and must be 10 or 13 digits.");
            }
            var book = new Entities.Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                AuthorId = author.Id,
            };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            var slimBookDTO = new SlimBookDTO
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN
            };
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, slimBookDTO);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookRequest request)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(request.AuthorId);
            if (author == null)
            {
                return BadRequest($"Author with ID {request.AuthorId} not found.");
            }
            if (string.IsNullOrEmpty(request.Title) || request.Title == "" || request.Title == "string")
            {
                return BadRequest("Title is required.");
            }
            if ((string.IsNullOrEmpty(request.ISBN) || request.ISBN == "" || request.ISBN == "string") && !System.Text.RegularExpressions.Regex.IsMatch(request.ISBN, @"^(?:\d{10}|\d{13})$"))
            {
                return BadRequest("ISBN is required and must be 10 or 13 digits.");
            }
            book.Title = request.Title;
            book.ISBN = request.ISBN;
            book.AuthorId = author.Id;
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
