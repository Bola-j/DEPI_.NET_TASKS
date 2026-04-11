using AutoMapper;
using Azure.Core;
using Library_System_Web_API.Data;
using Library_System_Web_API.DTOs.Author;
using Library_System_Web_API.DTOs.Book;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Borrower;
using Library_System_Web_API.DTOs.Loan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Library_System_Web_API.Results;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LibraryDBContext _context;
        private readonly ILogger<LoansController> _logger;
        private readonly IMapper _mapper;

        public LoansController(LibraryDBContext context, ILogger<LoansController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<LoanDTO>>>> GetLoans(string? name, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(Result.Failure("Page and PageSize must be greater than 0."));
            }

            var query = _context.Loans
                .Include(l => l.Borrower)
                .Include(l => l.Book)
                .AsNoTracking();

            var searchName = name?.Trim();
            if (!string.IsNullOrWhiteSpace(searchName))
                query = query.Where(p => EF.Functions.Like(p.Borrower.Name, $"%{searchName}%") || EF.Functions.Like(p.Book.Title, $"%{searchName}%"));

            var totalCount = await query.CountAsync();

            var loans = await query
                .OrderBy(l => l.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => _mapper.Map<LoanDTO>(l))
            .ToListAsync();


            _logger.LogInformation($"Retrieved {loans.Count} loans (Page: {page}, PageSize: {pageSize}, TotalCount: {totalCount}).");
            return Ok(GenericResult<PagedResult<LoanDTO>>.Success(new PagedResult<LoanDTO>(loans, page, pageSize, totalCount)));
        }
        [HttpGet("{bookId}/{borrowerId}")]
        public async Task<ActionResult<GenericResult<LoanDTO>>> GetLoanById(int bookId, int borrowerId)
        {
            var loan = await _context.Loans
                .Include(l => l.Borrower)
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.BookId == bookId && l.BorrowerId == borrowerId);

            if (loan == null)
            {
                _logger.LogWarning($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}.");
                return NotFound(Result.Failure($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}."));

            }
            _logger.LogInformation($"Retrieved loan for BookId: {bookId}, BorrowerId: {borrowerId}.");
            return Ok(GenericResult<LoanDTO>.Success(_mapper.Map<LoanDTO>(loan)));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResult<LoanDTO>>> CreateLoan(CreateLoanRequest request)
        {
            var book = await _context.Books.FindAsync(request.BookId);
            var borrower = await _context.Borrowers.FindAsync(request.BorrowerId);
            if (book == null)
            {   _logger.LogWarning($"Invalid BookId: {request.BookId}.");
                return BadRequest(Result.Failure($"Invalid BookId: {request.BookId}."));
            }
            if (borrower == null)
            {
                _logger.LogWarning($"Invalid BorrowerId: {request.BorrowerId}.");
                return BadRequest(Result.Failure($"Invalid BorrowerId: {request.BorrowerId}."));
            }
            if (request.LoanDate == null || request.LoanDate == default || string.IsNullOrEmpty(request.LoanDate.ToString()) || request.LoanDate.ToString() == "" || request.LoanDate.ToString() == "string" || request.LoanDate.Value <= borrower.MembershipDate)
            {
                _logger.LogWarning($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}.");
                return BadRequest(Result.Failure($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}."));
            }
        
            if (request.ReturnDate == null || request.ReturnDate == default || string.IsNullOrEmpty(request.ReturnDate.ToString()) || request.ReturnDate.ToString() == "" || request.ReturnDate.ToString() == "string" || request.ReturnDate.Value <= request.LoanDate.Value)
            {
                _logger.LogWarning($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate.");
                return BadRequest(Result.Failure($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate."));
            }
            var loan = _mapper.Map<Entities.Loan>(request);

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Created loan for BookId: {loan.BookId}, BorrowerId: {loan.BorrowerId}.");
            return CreatedAtAction(nameof(GetLoanById), new { bookId = loan.BookId, borrowerId = loan.BorrowerId }, _mapper.Map<LoanDTO>(loan));
        }

        [HttpPut("{bookId:int}/{borrowerId:int}")]
        public async Task<ActionResult<SlimLoanDTO>> UpdateLoan(int bookId, int borrowerId, UpdateLoanRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("UpdateLoan request body is null.");
                return BadRequest(Result.Failure("Request body is required."));
            }

            var loan = await _context.Loans
                .FindAsync(bookId , borrowerId);

            if (loan == null)
            {
                _logger.LogWarning($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}.");
                return NotFound(Result.Failure($"Loan not found for BookId: {bookId}, BorrowerId: {borrowerId}."));
            }

            request.BookId = (request.BookId == null || request.BookId == default || request.BookId <= 0) ? bookId : request.BookId;
            if (request.BookId != bookId)
            {
                _logger.LogInformation($"Updating BookId from {bookId} to {request.BookId} for BorrowerId: {borrowerId}.");
            }

            var bookExists = await _context.Books.AnyAsync(b => b.Id == request.BookId);
            if (!bookExists)
            {
                _logger.LogWarning($"Invalid BookId: {request.BookId}.");
                return BadRequest(Result.Failure($"Invalid BookId: {request.BookId}."));
            }


            if (request.BookId != bookId)
            {
                _logger.LogInformation($"Updating BookId from {bookId} to {request.BookId} for BorrowerId: {borrowerId}.");
            }
            else
            {
                _logger.LogInformation($"Keeping existing BookId: {bookId} for BorrowerId: {borrowerId}.");
            }



            request.BorrowerId = (request.BorrowerId == null || request.BorrowerId == default || request.BorrowerId <= 0) ? borrowerId : request.BorrowerId;

            var borrower = await _context.Borrowers
                .FirstOrDefaultAsync(b => b.Id == request.BorrowerId);

            if (borrower == null)
            {
                _logger.LogWarning($"Invalid BorrowerId: {request.BorrowerId}.");
                return BadRequest(Result.Failure($"Invalid BorrowerId: {request.BorrowerId}."));
            }

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
                return BadRequest(Result.Failure("LoanDate is required."));
            }

            if (request.LoanDate.Value < borrower.MembershipDate)
            {
                _logger.LogWarning($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}.");
                return BadRequest(Result.Failure($"Invalid LoanDate: {request.LoanDate} for BorrowerId: {request.BorrowerId}, it cannot be before MembershipDate: {borrower.MembershipDate}."));
            }



            if (!request.ReturnDate.HasValue)
            {
                _logger.LogWarning("ReturnDate is required in UpdateLoan request.");
                return BadRequest(Result.Failure("ReturnDate is required."));
            }

            if (request.ReturnDate.Value <= loan.LoanDate)
            {
                _logger.LogWarning($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate.");
                return BadRequest(Result.Failure($"Invalid ReturnDate: {request.ReturnDate} for LoanDate: {request.LoanDate}, it must be after LoanDate."));
            }

            _mapper.Map(request, loan);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<SlimLoanDTO>(loan);

            return Ok(GenericResult<SlimLoanDTO>.Success(dto));
        }

        [HttpDelete("{bookId}/{borrowerId}")]
        public async Task<ActionResult<GenericResult<SlimLoanDTO>>> DeleteLoan(int bookId, int borrowerId)
        {
            var loan = await _context.Loans.FindAsync(bookId, borrowerId);
            if (loan == null)
            {
                _logger.LogWarning($"Loan with BookId {bookId} and BorrowerId {borrowerId} not found for deletion.");
                return NotFound(Result.Failure($"Loan with BookId {bookId} and BorrowerId {borrowerId} not found."));
            }

            _context.Loans.Remove(loan);
            _logger.LogInformation($"Deleted loan with BookId {bookId} and BorrowerId {borrowerId}.");

            await _context.SaveChangesAsync();
            return Ok(GenericResult<SlimLoanDTO>.Success(_mapper.Map<SlimLoanDTO>(loan)));
        }
    }
}
