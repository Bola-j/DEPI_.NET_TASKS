using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.DTOs.Loan;
using AutoMapper;
using Library_System_Web_API.Results;
using Library_System_Web_API.Services;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<DashboardController> _logger;
        private readonly IMapper _mapper;
        public DashboardController(ILoanService loanService, ILogger<DashboardController> logger, IMapper mapper)
        {
            _loanService = loanService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("Loans/DueDate")]
        public async Task<ActionResult<GenericResult<PagedResult<LoanDTO>>>> GetLoansDue([FromBody] GetLoansDueRequest request, int page = 1, int pageSize = 10)
        {
            if (request is null)
            {
                _logger.LogWarning("GetLoansDue request body is null.");
                return BadRequest(GenericResult<PagedResult<LoanDTO>>.Failure("Request body is required."));
            }

            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<LoanDTO>>.Failure("Page and PageSize must be greater than 0."));
            }

            if (request.Date == default)
            {
                _logger.LogWarning("Invalid due date provided in request body.");
                return BadRequest(GenericResult<PagedResult<LoanDTO>>.Failure("Date is required."));
            }

            var result = await _loanService.GetDueLoansAsync(request.Date, page, pageSize);
            if (result.IsFailure)
            {
                _logger.LogError("Failed to retrieve due loans for {Date}. Error: {Error}", request.Date, result.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<LoanDTO>>.Failure(result.Error));
            }

            var loans = result.Value.Data.Select(_mapper.Map<LoanDTO>).ToList();
            _logger.LogInformation("Retrieved {Count} due loans for {Date}.", loans.Count, request.Date);
            return Ok(GenericResult<PagedResult<LoanDTO>>.Success(new PagedResult<LoanDTO>(loans, page, pageSize, result.Value.TotalCount)));
        }
        
    }
}