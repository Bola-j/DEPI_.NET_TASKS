using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.DTOs.Book;
using AutoMapper;
using Library_System_Web_API.Results;
using Library_System_Web_API.Services;
namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BooksController> _logger;
        private readonly IMapper _mapper;
        public BooksController(IBookService bookService, ILogger<BooksController> logger, IMapper mapper)
        {
            _bookService = bookService;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<BookDTO>>>> GetBooks(string? name, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<BookDTO>>.Failure("Page and PageSize must be greater than 0."));
            }

            var result = await _bookService.GetAllAsync(name, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<BookDTO>>.Failure(result.Error));
            }

            var books = result.Value.Data.Select(_mapper.Map<BookDTO>).ToList();

            _logger.LogInformation($"Retrieved {books.Count} books (Page: {page}, PageSize: {pageSize}, TotalCount: {result.Value.TotalCount}).");
            return Ok(GenericResult<PagedResult<BookDTO>>.Success(new PagedResult<BookDTO>(books, page, pageSize, result.Value.TotalCount)));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<BookDTO>>> GetBook(int id)
        {   
            var result = await _bookService.GetByIdAsync(id);

            
            if (result.IsFailure)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(GenericResult<BookDTO>.Failure($"Book with ID {id} not found."));
            }
            _logger.LogInformation("Book with ID {BookId} retrieved successfully.", id);
            return Ok(GenericResult<BookDTO>.Success(_mapper.Map<BookDTO>(result.Value)));

        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimBookDTO>>> CreateBook(CreateBookRequest request)
        {
            var authorExists = request.AuthorId.HasValue && await _bookService.AuthorExistsAsync(request.AuthorId.Value);
            if (!authorExists)
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
            var book = _mapper.Map<Entities.Book>(request);

            _logger.LogInformation($"Creating new book: Title='{request.Title}', ISBN='{request.ISBN}', AuthorId={request.AuthorId}.");
            var createResult = await _bookService.CreateAsync(book);
            if (createResult.IsFailure)
            {
                _logger.LogError("Failed to create book. Error: {Error}", createResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<SlimBookDTO>.Failure(createResult.Error));
            }
            _logger.LogInformation($"Book created with ID {book.Id}.");

            var slimBookDTO = _mapper.Map<SlimBookDTO>(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, GenericResult<SlimBookDTO>.Success(slimBookDTO));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<SlimBookDTO>>> UpdateBook(int id, UpdateBookRequest request)
        {
            var getResult = await _bookService.GetByIdAsync(id);
            if (getResult.IsFailure)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(GenericResult<SlimBookDTO>.Failure($"Book with ID {id} not found."));
            }

            var book = getResult.Value;

            var authorExists = request.AuthorId.HasValue && await _bookService.AuthorExistsAsync(request.AuthorId.Value);
            if (!authorExists)
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
            var updateResult = await _bookService.UpdateAsync(book);
            if (updateResult.IsFailure)
            {
                _logger.LogError("Failed to update book with ID {BookId}. Error: {Error}", id, updateResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<SlimBookDTO>.Failure(updateResult.Error));
            }
            _logger.LogInformation($"Book with ID {id} updated successfully.");

            return Ok(GenericResult<SlimBookDTO>.Success(_mapper.Map<SlimBookDTO>(book)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimBookDTO>>> DeleteBook(int id)
        {
            var deleteResult = await _bookService.DeleteAsync(id);
            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"Book with ID {id} not found.");
                return NotFound(GenericResult<SlimBookDTO>.Failure($"Book with ID {id} not found."));
            }
            _logger.LogInformation($"Book with ID {id} deleted successfully.");

            return Ok(GenericResult<SlimBookDTO>.Success(_mapper.Map<SlimBookDTO>(deleteResult.Value)));
        }
    }
}
