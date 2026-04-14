using AutoMapper;
using Library_System_Web_API.DTOs.Loan;
using Library_System_Web_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;
        private readonly ILogger<LoansController> _logger;
        private readonly IMapper _mapper;

        public LoansController(ILoanService loanService, ILogger<LoansController> logger, IMapper mapper)
        {
            _loanService = loanService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<LoanDTO>>>> GetLoans(string? name, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<LoanDTO>>.Failure("Page and PageSize must be greater than 0."));
            }

            var result = await _loanService.GetAllAsync(name, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<LoanDTO>>.Failure(result.Error));
            }

            var loans = result.Value.Data.Select(_mapper.Map<LoanDTO>).ToList();


            _logger.LogInformation($"Retrieved {loans.Count} loans (Page: {page}, PageSize: {pageSize}, TotalCount: {result.Value.TotalCount}).");
            return Ok(GenericResult<PagedResult<LoanDTO>>.Success(new PagedResult<LoanDTO>(loans, page, pageSize, result.Value.TotalCount)));
        }
        [HttpGet("{bookId}/{borrowerId}")]
        public async Task<ActionResult<GenericResult<LoanDTO>>> GetLoanById(int bookId, int borrowerId)
        {
            var result = await _loanService.GetByBookAndBorrowerAsync(bookId, borrowerId);

            if (result.IsFailure)
            {
                _logger.LogWarning($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}.");
                return NotFound(GenericResult<LoanDTO>.Failure($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}."));

            }
            _logger.LogInformation($"Retrieved loan for BookId: {bookId}, BorrowerId: {borrowerId}.");
            return Ok(GenericResult<LoanDTO>.Success(_mapper.Map<LoanDTO>(result.Value)));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<LoanDTO>>> CreateLoan(CreateLoanRequest request)
        {
            var bookExists = await _loanService.BookExistsAsync(request.BookId);
            var borrowerResult = await _loanService.GetBorrowerByIdAsync(request.BorrowerId);
            if (!bookExists)
            {   _logger.LogWarning($"Invalid BookId: {request.BookId}.");
                return BadRequest(GenericResult<LoanDTO>.Failure($"Invalid BookId: {request.BookId}."));
            }
            if (borrowerResult.IsFailure)
            {
                _logger.LogWarning($"Invalid BorrowerId: {request.BorrowerId}.");
                return BadRequest(GenericResult<LoanDTO>.Failure($"Invalid BorrowerId: {request.BorrowerId}."));
            }
            var borrower = borrowerResult.Value;
            if (request.LoanDate == null || request.LoanDate == default || string.IsNullOrEmpty(request.LoanDate.ToString()) || request.LoanDate.ToString() == "" || request.LoanDate.ToString() == "string" || request.LoanDate.Value <= borrower.MembershipDate)
            {
                _logger.LogWarning($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}.");
                return BadRequest(GenericResult<LoanDTO>.Failure($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}."));
            }
        
            if (request.ReturnDate == null || request.ReturnDate == default || string.IsNullOrEmpty(request.ReturnDate.ToString()) || request.ReturnDate.ToString() == "" || request.ReturnDate.ToString() == "string" || request.ReturnDate.Value <= request.LoanDate.Value)
            {
                _logger.LogWarning($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate.");
                return BadRequest(GenericResult<LoanDTO>.Failure($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate."));
            }
            var loan = _mapper.Map<Entities.Loan>(request);
            var createResult = await _loanService.CreateAsync(loan);
            if (createResult.IsFailure)
            {
                _logger.LogError("Failed to create loan. Error: {Error}", createResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<LoanDTO>.Failure(createResult.Error));
            }
            
            _logger.LogInformation($"Created loan for BookId: {loan.BookId}, BorrowerId: {loan.BorrowerId}.");
            return CreatedAtAction(nameof(GetLoanById), new { bookId = loan.BookId, borrowerId = loan.BorrowerId }, GenericResult<LoanDTO>.Success(_mapper.Map<LoanDTO>(loan)));
        }

        [HttpPut("{bookId:int}/{borrowerId:int}")]
        public async Task<ActionResult<GenericResult<SlimLoanDTO>>> UpdateLoan(int bookId, int borrowerId, UpdateLoanRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("UpdateLoan request body is null.");
                return BadRequest(GenericResult<SlimLoanDTO>.Failure("Request body is required."));
            }

            var getResult = await _loanService.GetByBookAndBorrowerAsync(bookId, borrowerId);

            if (getResult.IsFailure)
            {
                _logger.LogWarning($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}.");
                return NotFound(GenericResult<SlimLoanDTO>.Failure($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}."));
            }
            var loan = getResult.Value;

            request.BookId = request.BookId <= 0 ? bookId : request.BookId;
            if (request.BookId != bookId)
            {
                _logger.LogInformation($"Updating BookId from {bookId} to {request.BookId} for BorrowerId: {borrowerId}.");
            }

            var bookExists = await _loanService.BookExistsAsync(request.BookId);
            if (!bookExists)
            {
                _logger.LogWarning($"Invalid BookId: {request.BookId}.");
                return BadRequest(GenericResult<SlimLoanDTO>.Failure($"Invalid BookId: {request.BookId}."));
            }


            request.BorrowerId = request.BorrowerId <= 0 ? borrowerId : request.BorrowerId;

            var borrowerResult = await _loanService.GetBorrowerByIdAsync(request.BorrowerId);

            if (borrowerResult.IsFailure)
            {
                _logger.LogWarning($"Invalid BorrowerId: {request.BorrowerId}.");
                return BadRequest(GenericResult<SlimLoanDTO>.Failure($"Invalid BorrowerId: {request.BorrowerId}."));
            }
            var borrower = borrowerResult.Value;

            if(request.BorrowerId != borrowerId)
            {
                _logger.LogInformation($"Updating BorrowerId from {borrowerId} to {request.BorrowerId} for BookId: {bookId}.");
            }
            else
            {
                _logger.LogInformation($"Keeping existing BorrowerId: {borrowerId} for BookId: {bookId}.");
            }


            if (!request.LoanDate.HasValue)
            {
                _logger.LogWarning("LoanDate is required in UpdateLoan request.");
                return BadRequest(GenericResult<SlimLoanDTO>.Failure("LoanDate is required."));
            }

            if (request.LoanDate.Value < borrower.MembershipDate)
            {
                _logger.LogWarning($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}.");
                return BadRequest(GenericResult<SlimLoanDTO>.Failure($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}."));
            }



            if (!request.ReturnDate.HasValue)
            {
                _logger.LogWarning("ReturnDate is required in UpdateLoan request.");
                return BadRequest(GenericResult<SlimLoanDTO>.Failure("ReturnDate is required."));
            }

            if (request.ReturnDate.Value <= loan.LoanDate)
            {
                _logger.LogWarning($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate.");
                return BadRequest(GenericResult<SlimLoanDTO>.Failure($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate."));
            }

            _mapper.Map(request, loan);
            var updateResult = await _loanService.UpdateAsync(loan);
            if (updateResult.IsFailure)
            {
                _logger.LogError("Failed to update loan with BookId {BookId} and BorrowerId {BorrowerId}. Error: {Error}", bookId, borrowerId, updateResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<SlimLoanDTO>.Failure(updateResult.Error));
            }

            var dto = _mapper.Map<SlimLoanDTO>(loan);
            _logger.LogInformation("Loan with BookId {BookId} and BorrowerId {BorrowerId} updated successfully.", bookId, borrowerId);

            return Ok(GenericResult<SlimLoanDTO>.Success(dto));
        }

        [HttpDelete("{bookId}/{borrowerId}")]
        public async Task<ActionResult<GenericResult<SlimLoanDTO>>> DeleteLoan(int bookId, int borrowerId)
        {
            var deleteResult = await _loanService.DeleteByBookAndBorrowerAsync(bookId, borrowerId);
            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"Loan with BookId {bookId} and BorrowerId {borrowerId} not found for deletion.");
                return NotFound(GenericResult<SlimLoanDTO>.Failure($"Loan with BookId {bookId} and BorrowerId {borrowerId} not found."));
            }
            _logger.LogInformation($"Deleted loan with BookId {bookId} and BorrowerId {borrowerId}.");

            return Ok(GenericResult<SlimLoanDTO>.Success(_mapper.Map<SlimLoanDTO>(deleteResult.Value)));
        }
    }
}
