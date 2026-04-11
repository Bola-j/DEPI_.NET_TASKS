using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Data;
using Microsoft.EntityFrameworkCore;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Loan;
using AutoMapper;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly ILogger<DashboardController> _logger;
        private readonly IMapper _mapper;
        public DashboardController(LibraryDBContext context, ILogger<DashboardController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("Loans/DueDate")]
        public async Task<ActionResult<GenericResult<PagedResult<LoanDTO>>>> GetLoansDue(DateOnly date, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<LoanDTO>>.Failure("Page and PageSize must be greater than 0."));
            }

            var query = _context.Loans
                .Where(l => l.ReturnDate == date)
                .Include(l => l.Borrower)
                .Include(l => l.Book)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var loans = await query
                .OrderBy(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => _mapper.Map<LoanDTO>(l))
                .ToListAsync();
            return Ok(GenericResult<PagedResult<LoanDTO>>.Success(new PagedResult<LoanDTO>(loans, page, pageSize, totalCount)));
        }
        
    }
}