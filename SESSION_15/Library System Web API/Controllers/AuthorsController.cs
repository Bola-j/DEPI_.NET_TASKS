using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs.Book;
using Microsoft.EntityFrameworkCore;
using Library_System_Web_API.DTOs.Author;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public AuthorsController(LibraryDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<AuthorDTO>> GetAuthors()
        {
            var authors = await _context.Authors.Select(a => new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
                BirthDate = a.BirthDate,
                Books = a.Books.Select(b => new SlimBookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN
                }).ToList()
            })
            .ToListAsync();
            return authors;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetAuthor(int id)
        {
            var author = await _context.Authors.Where(a => a.Id == id).Select(a => new AuthorDTO
            {
                Id = a.Id,
                Name = a.Name,
                BirthDate = a.BirthDate,
                Books = a.Books.Select(b => new SlimBookDTO
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN
                }).ToList()
            }).FirstOrDefaultAsync();
            if (author == null)
            {
                return NotFound();
            }
            return author;
        }
        [HttpPost]
        public async Task<ActionResult<SlimAuthorDTO>> CreateAuthor(CreateAuthorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {
                return BadRequest("Author valid name is required.");
            }
            if (!request.Birthdate.HasValue || request.Birthdate == default || request.Birthdate == DateOnly.MinValue || request.Birthdate >= DateOnly.FromDateTime(DateTime.Now.AddYears(-10)))
            {
                return BadRequest("Author valid birthdate is required.");
            }
            var author = new Entities.Author
            {
                Name = request.Name,
                BirthDate = request.Birthdate.Value
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            var authorDTO = new SlimAuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                BirthDate = author.BirthDate
            };
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, authorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SlimAuthorDTO>> UpdateAuthor(int id, UpdateAuthorRequest request)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(request.Name) || request.Name == "" || request.Name == "string")
            {
                return BadRequest("Author valid name is required.");
            }
            if (!request.Birthdate.HasValue || request.Birthdate == default || request.Birthdate == DateOnly.MinValue || request.Birthdate >= DateOnly.FromDateTime(DateTime.Now.AddYears(-10)))
            {
                return BadRequest("Author valid birthdate is required.");
            }
            author.Name = request.Name;
            author.BirthDate = request.Birthdate.Value;
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SlimAuthorDTO>> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            var authorDTO = new SlimAuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                BirthDate = author.BirthDate
            };
            return authorDTO;
        }
    }
}
