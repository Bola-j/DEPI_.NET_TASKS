using AutoMapper;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly ILogger<AuthorsController> _logger;
        private readonly IMapper _mapper;

        public AuthorsController(LibraryDBContext context, ILogger<AuthorsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<AuthorDTO>>>> GetAuthors(string? name, int page = 1, int pageSize = 10)
        {


            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(Result.Failure("Page and PageSize must be greater than 0."));
            }

            var query = _context.Authors
                .Include(a => a.Books)
                .AsNoTracking();

            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{searchName}%"));

            var totalCount = await query.CountAsync();

            var authors = await query
                .OrderBy(a => a.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => _mapper.Map<AuthorDTO>(a))
            .ToListAsync();
            

            _logger.LogInformation($"Retrieved {authors.Count} authors (Page: {page}, PageSize: {pageSize}, TotalCount: {totalCount}).");
            return Ok(GenericResult<PagedResult<AuthorDTO>>.Success(new PagedResult<AuthorDTO>(authors, page, pageSize, totalCount)));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<AuthorDTO>>> GetAuthor(int id)
        {
            var author  = await  _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
                

            if (author == null)
            {
                _logger.LogWarning($"Author with ID {id} not found.");
                return NotFound(GenericResult<AuthorDTO>.Failure($"Author with ID {id} not found."));
            }
            _logger.LogInformation($"Author with ID {id} retrieved successfully.");

            return Ok(GenericResult<AuthorDTO>.Success(_mapper.Map<AuthorDTO>(author)));
        }
        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimAuthorDTO>>> CreateAuthor(CreateAuthorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {
                _logger.LogWarning($"Invalid author name provided: '{request.Name}'.");
                return BadRequest(Result.Failure("Author valid name is required."));
            }
            if (!request.Birthdate.HasValue || request.Birthdate == default || request.Birthdate == DateOnly.MinValue || request.Birthdate >= DateOnly.FromDateTime(DateTime.Now.AddYears(-10)))
            {
                _logger.LogWarning($"Invalid author birthdate provided: '{request.Birthdate}'."); 
                return BadRequest(Result.Failure("Author valid birthdate is required."));
            }
            var author = _mapper.Map<Entities.Author>(request);

            _context.Authors.Add(author);
            _logger.LogInformation($"Creating a new author with name '{author.Name}' and birthdate '{author.BirthDate}'.");
            await _context.SaveChangesAsync();

            var authorDTO = _mapper.Map<SlimAuthorDTO>(author);

            _logger.LogInformation($"Author with ID {author.Id} created successfully.");
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, GenericResult<SlimAuthorDTO>.Success(authorDTO));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SlimAuthorDTO>> UpdateAuthor(int id, UpdateAuthorRequest request)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"Author with ID {id} not found.");
                return NotFound(GenericResult<SlimAuthorDTO>.Failure($"Author with ID {id} not found."));
            }
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {   
                _logger.LogWarning($"Invalid author name provided for update: '{request.Name}'\nThe same stays as it is.");
                request.Name = author.Name; // Keep the existing name if the new one is invalid
            }
            if (!request.Birthdate.HasValue || request.Birthdate == default || request.Birthdate == DateOnly.MinValue || request.Birthdate >= DateOnly.FromDateTime(DateTime.Now.AddYears(-10)))
            {
                _logger.LogWarning($"Invalid author birthdate provided for update: '{request.Birthdate}'\nthe birthdate stays as it is.");
                request.Birthdate = author.BirthDate; // Keep the existing birthdate if the new one is invalid
            }

            _mapper.Map(request, author);
            _logger.LogInformation($"Author with ID {id} found for update. Proceeding to update with provided data.");
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Author with ID {id} updated successfully.");
            return Ok(GenericResult<SlimAuthorDTO>.Success(_mapper.Map<SlimAuthorDTO>(_mapper.Map(author,new SlimAuthorDTO()))));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimAuthorDTO>>> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {   
                _logger.LogWarning($"Author with ID {id} not found for deletion.");
                return NotFound(GenericResult<SlimAuthorDTO>.Failure($"Author with ID {id} not found."));
            }

            _context.Authors.Remove(author);
            _logger.LogInformation($"Author with ID {id} found for deletion. Proceeding to delete.");
            
            await _context.SaveChangesAsync();
            

            return Ok(GenericResult<SlimAuthorDTO>.Success( _mapper.Map<SlimAuthorDTO>(author) ));
        }
    }
}
