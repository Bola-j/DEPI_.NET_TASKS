using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Loan;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Borrower;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Library_System_Web_API.Results;
namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly ILogger<BooksController> _logger;
        private readonly IMapper _mapper;
        public BooksController(LibraryDBContext context, ILogger<BooksController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<BookDTO>>>> GetBooks(string? name, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(Result.Failure("Page and PageSize must be greater than 0."));
            }

            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Loans)
                .ThenInclude(l => l.Borrower)
                .AsNoTracking();

            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(b => EF.Functions.Like(b.Title, $"%{searchName}%") || EF.Functions.Like(b.Author.Name, $"%{searchName}%"));

            var totalCount = await query.CountAsync();

            var books = await query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => _mapper.Map<BookDTO>(b))
                .ToListAsync();

            _logger.LogInformation($"Retrieved {books.Count} books (Page: {page}, PageSize: {pageSize}, TotalCount: {totalCount}).");
            return Ok(GenericResult<PagedResult<BookDTO>>.Success(new PagedResult<BookDTO>(books, page, pageSize, totalCount)));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<BookDTO>>> GetBook(int id)
        {   
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Loans)
                .ThenInclude(l => l.Borrower)
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            
            if (book == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(Result.Failure($"Book with ID {id} not found."));
            }
            return Ok(GenericResult<BookDTO>.Success(_mapper.Map<BookDTO>(book)));

        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimBookDTO>>> CreateBook(CreateBookRequest request)
        {
            var author = await _context.Authors.FindAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogWarning($"Author with ID {request.AuthorId} not found.");  
                return BadRequest(Result.Failure($"Author with ID {request.AuthorId} not found."));
            }
            if (string.IsNullOrEmpty(request.Title) || request.Title == "" || request.Title == "string")
            {
                _logger.LogWarning("Title is required."); 
                return BadRequest(Result.Failure("Title is required."));
            }
            if ((string.IsNullOrEmpty(request.ISBN) || request.ISBN == "" || request.ISBN == "string") && !System.Text.RegularExpressions.Regex.IsMatch(request.ISBN, @"^(?:\d{10}|\d{13})$"))
            {
                _logger.LogWarning("ISBN is required and must be 10 or 13 digits.");
                return BadRequest(Result.Failure("ISBN is required and must be 10 or 13 digits."));
            }
            var book = _mapper.Map<Entities.Book>(request);
            _context.Books.Add(book);

            _logger.LogInformation($"Creating new book: Title='{request.Title}', ISBN='{request.ISBN}', AuthorId={request.AuthorId}.");
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Book created with ID {book.Id}.");

            var slimBookDTO = _mapper.Map<SlimBookDTO>(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, GenericResult<SlimBookDTO>.Success(slimBookDTO));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<SlimBookDTO>>> UpdateBook(int id, UpdateBookRequest request)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(GenericResult<SlimBookDTO>.Failure($"Book with ID {id} not found."));
            }
            var author = await _context.Authors.FindAsync(request.AuthorId);
            if (author == null)
            {
                _logger.LogWarning($"Author with ID {request.AuthorId} not found.");
                return BadRequest(GenericResult<SlimBookDTO>.Failure($"Author with ID {request.AuthorId} not found."));
            }
            if (string.IsNullOrEmpty(request.Title) || request.Title == "" || request.Title == "string")
            {
                _logger.LogWarning("Title is required.");
                return BadRequest(GenericResult<SlimBookDTO>.Failure("Title is required."));
            }
            if ((string.IsNullOrEmpty(request.ISBN) || request.ISBN == "" || request.ISBN == "string") && !System.Text.RegularExpressions.Regex.IsMatch(request.ISBN, @"^(?:\d{10}|\d{13})$"))
            {
                _logger.LogWarning("ISBN is required and must be 10 or 13 digits.");
                return BadRequest(GenericResult<SlimBookDTO>.Failure("ISBN is required and must be 10 or 13 digits."));
            }

            _mapper.Map(request, book);

            _logger.LogInformation($"Updating book with ID {id}: Title='{request.Title}', ISBN='{request.ISBN}', AuthorId={request.AuthorId}.");
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Book with ID {id} updated successfully.");

            return Ok(GenericResult<SlimBookDTO>.Success(_mapper.Map<SlimBookDTO>(book)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimBookDTO>>> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(GenericResult<SlimBookDTO>.Failure($"Book with ID {id} not found."));
            }
            _context.Books.Remove(book);
            _logger.LogInformation($"Deleting book with ID {id}.");
            await _context.SaveChangesAsync();

            return Ok(GenericResult<SlimBookDTO>.Success(_mapper.Map<SlimBookDTO>(book)));
        }
    }
}
