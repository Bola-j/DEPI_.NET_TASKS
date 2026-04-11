using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Loan;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly ILogger<BorrowersController> _logger;
        private readonly IMapper _mapper;
        public BorrowersController(LibraryDBContext context, ILogger<BorrowersController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<BorrowerDTO>>>> GetBorrowers(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(Result.Failure("Page and PageSize must be greater than 0."));
            }

            var query = _context.Borrowers
                .Include(b => b.Loans)
                .ThenInclude(l => l.Book)
                .ThenInclude(b => b.Author)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var borrowers = await query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => _mapper.Map<BorrowerDTO>(b))
                .ToListAsync();

            _logger.LogInformation($"Retrieved {borrowers.Count} borrowers (Page: {page}, PageSize: {pageSize}, TotalCount: {totalCount}).");
            return Ok(GenericResult<PagedResult<BorrowerDTO>>.Success(new PagedResult<BorrowerDTO>(borrowers, page, pageSize, totalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<BorrowerDTO>>> GetBorrowerById(int id)
        {
            var borrower = await _context.Borrowers
                .Include(b => b.Loans) 
                    .ThenInclude(l => l.Book)
                        .ThenInclude(b => b.Author)
                .Select(b => _mapper.Map<BorrowerDTO>(b))
                .FirstOrDefaultAsync(b => b.Id == id);

            if(borrower == null)
            {
                _logger.LogWarning($"Borrower with ID {id} not found.");
                return NotFound(GenericResult<BorrowerDTO>.Failure($"Borrower with ID {id} not found."));
            }
            _logger.LogInformation($"Retrieved borrower with ID {id}.");
            return Ok(GenericResult<BorrowerDTO>.Success(borrower));
        }
        
        [HttpPost]
        public async Task<ActionResult<GenericResult<BorrowerDTO>>> CreateBorrower(CreateBorrowerRequest createBorrowerRequest)
        {   
            if (string.IsNullOrEmpty(createBorrowerRequest.Name) || createBorrowerRequest.Name == "" || createBorrowerRequest.Name == "string")
            {
                _logger.LogWarning("Invalid borrower name provided.");
                return BadRequest(GenericResult<BorrowerDTO>.Failure("Name is required."));
            }
            if(createBorrowerRequest.MembershipDate == null || createBorrowerRequest.MembershipDate == default || createBorrowerRequest.MembershipDate.Value <= DateOnly.FromDateTime(DateTime.Now))
            {
                _logger.LogWarning("Invalid membership date provided, must be a recent or past date.");  
                return BadRequest(GenericResult<BorrowerDTO>.Failure("Membership Date is required and must be a recent or past date."));
            }
            var borrower = _mapper.Map<Entities.Borrower>(createBorrowerRequest);


            _context.Borrowers.Add(borrower);
            await _context.SaveChangesAsync();

            var borrowerDTO = _mapper.Map<BorrowerDTO>(borrower);
            return CreatedAtAction(nameof(GetBorrowerById), new { id = borrower.Id }, GenericResult<BorrowerDTO>.Success(borrowerDTO));
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
