using AutoMapper;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.Results;
using Library_System_Web_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorsController> _logger;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger, IMapper mapper)
        {
            _authorService = authorService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<AuthorDTO>>>> GetAuthors(string? name, int page = 1, int pageSize = 10)
        {


            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<AuthorDTO>>.Failure("Page and PageSize must be greater than 0."));
            }

            var result = await _authorService.GetAllAsync(name, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<AuthorDTO>>.Failure(result.Error));
            }

            var authors = result.Value.Data.Select(_mapper.Map<AuthorDTO>).ToList();
            

            _logger.LogInformation($"Retrieved {authors.Count} authors (Page: {page}, PageSize: {pageSize}, TotalCount: {result.Value.TotalCount}).");
            return Ok(GenericResult<PagedResult<AuthorDTO>>.Success(new PagedResult<AuthorDTO>(authors, page, pageSize, result.Value.TotalCount)));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<AuthorDTO>>> GetAuthor(int id)
        {
            var result = await _authorService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                _logger.LogWarning($"Author with ID {id} not found.");
                return NotFound(GenericResult<AuthorDTO>.Failure($"Author with ID {id} not found."));
            }
            _logger.LogInformation($"Author with ID {id} retrieved successfully.");

            return Ok(GenericResult<AuthorDTO>.Success(_mapper.Map<AuthorDTO>(result.Value)));
        }
        [HttpPost]
        public async Task<ActionResult<GenericResult<SlimAuthorDTO>>> CreateAuthor(CreateAuthorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {
                _logger.LogWarning($"Invalid author name provided: '{request.Name}'.");
                return BadRequest(GenericResult<SlimAuthorDTO>.Failure("Author valid name is required."));
            }
            if (!request.Birthdate.HasValue || request.Birthdate == default || request.Birthdate == DateOnly.MinValue || request.Birthdate >= DateOnly.FromDateTime(DateTime.Now.AddYears(-10)))
            {
                _logger.LogWarning($"Invalid author birthdate provided: '{request.Birthdate}'."); 
                return BadRequest(GenericResult<SlimAuthorDTO>.Failure("Author valid birthdate is required."));
            }
            var author = _mapper.Map<Entities.Author>(request);
            _logger.LogInformation($"Creating a new author with name '{author.Name}' and birthdate '{author.BirthDate}'.");
            var createResult = await _authorService.CreateAsync(author);
            if (createResult.IsFailure)
            {
                _logger.LogError("Failed to create author. Error: {Error}", createResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<SlimAuthorDTO>.Failure(createResult.Error));
            }

            var authorDTO = _mapper.Map<SlimAuthorDTO>(author);

            _logger.LogInformation($"Author with ID {author.Id} created successfully.");
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, GenericResult<SlimAuthorDTO>.Success(authorDTO));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<SlimAuthorDTO>>> UpdateAuthor(int id, UpdateAuthorRequest request)
        {
            var getResult = await _authorService.GetByIdAsync(id);

            if (getResult.IsFailure)
            {
                _logger.LogWarning($"Author with ID {id} not found.");
                return NotFound(GenericResult<SlimAuthorDTO>.Failure($"Author with ID {id} not found."));
            }
            var author = getResult.Value;
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
            var updateResult = await _authorService.UpdateAsync(author);
            if (updateResult.IsFailure)
            {
                _logger.LogError("Failed to update author with ID {AuthorId}. Error: {Error}", id, updateResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<SlimAuthorDTO>.Failure(updateResult.Error));
            }

            _logger.LogInformation($"Author with ID {id} updated successfully.");
            return Ok(GenericResult<SlimAuthorDTO>.Success(_mapper.Map<SlimAuthorDTO>(author)));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimAuthorDTO>>> DeleteAuthor(int id)
        {
            var deleteResult = await _authorService.DeleteAsync(id);
            if (deleteResult.IsFailure)
            {   
                _logger.LogWarning($"Author with ID {id} not found for deletion.");
                return NotFound(GenericResult<SlimAuthorDTO>.Failure($"Author with ID {id} not found."));
            }
            _logger.LogInformation($"Author with ID {id} found for deletion. Proceeding to delete.");

            return Ok(GenericResult<SlimAuthorDTO>.Success( _mapper.Map<SlimAuthorDTO>(deleteResult.Value) ));
        }
    }
}
