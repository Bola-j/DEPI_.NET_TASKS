using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Library_System_Web_API.DTOs.Borrower;
using AutoMapper;
using Library_System_Web_API.Results;
using Library_System_Web_API.Services;

namespace Library_System_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private readonly IBorrowerService _borrowerService;
        private readonly ILogger<BorrowersController> _logger;
        private readonly IMapper _mapper;
        public BorrowersController(IBorrowerService borrowerService, ILogger<BorrowersController> logger, IMapper mapper)
        {
            _borrowerService = borrowerService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<BorrowerDTO>>>> GetBorrowers(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<BorrowerDTO>>.Failure("Page and PageSize must be greater than 0."));
            }

            var result = await _borrowerService.GetAllAsync(page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<BorrowerDTO>>.Failure(result.Error));
            }

            var borrowers = result.Value.Data.Select(_mapper.Map<BorrowerDTO>).ToList();

            _logger.LogInformation($"Retrieved {borrowers.Count} borrowers (Page: {page}, PageSize: {pageSize}, TotalCount: {result.Value.TotalCount}).");
            return Ok(GenericResult<PagedResult<BorrowerDTO>>.Success(new PagedResult<BorrowerDTO>(borrowers, page, pageSize, result.Value.TotalCount)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GenericResult<BorrowerDTO>>> GetBorrowerById(int id)
        {
            var result = await _borrowerService.GetByIdAsync(id);

            if(result.IsFailure)
            {
                _logger.LogWarning($"Borrower with ID {id} not found.");
                return NotFound(GenericResult<BorrowerDTO>.Failure($"Borrower with ID {id} not found."));
            }
            _logger.LogInformation($"Retrieved borrower with ID {id}.");
            return Ok(GenericResult<BorrowerDTO>.Success(_mapper.Map<BorrowerDTO>(result.Value)));
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
            var createResult = await _borrowerService.CreateAsync(borrower);
            if (createResult.IsFailure)
            {
                _logger.LogError("Failed to create borrower. Error: {Error}", createResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<BorrowerDTO>.Failure(createResult.Error));
            }

            var borrowerDTO = _mapper.Map<BorrowerDTO>(borrower);
            _logger.LogInformation("Borrower created successfully with ID {BorrowerId}.", borrower.Id);
            return CreatedAtAction(nameof(GetBorrowerById), new { id = borrower.Id }, GenericResult<BorrowerDTO>.Success(borrowerDTO));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<GenericResult<BorrowerDTO>>> UpdateBorrower(int id, UpdateBorrowerRequest updateBorrowerRequest)
        {
            var getResult = await _borrowerService.GetByIdAsync(id);
            if (getResult.IsFailure)
            {
                _logger.LogWarning("Borrower with ID {BorrowerId} was not found for update.", id);
                return NotFound(GenericResult<BorrowerDTO>.Failure($"Borrower with ID {id} not found."));
            }
            var borrower = getResult.Value;
            if (string.IsNullOrEmpty(updateBorrowerRequest.Name) || updateBorrowerRequest.Name == "" || updateBorrowerRequest.Name == "string")
            {
                _logger.LogWarning("Invalid borrower name provided for update for ID {BorrowerId}.", id);
                return BadRequest(GenericResult<BorrowerDTO>.Failure("Name is required."));
            }
            if (updateBorrowerRequest.MembershipDate == null || updateBorrowerRequest.MembershipDate == default || updateBorrowerRequest.MembershipDate.Value <= DateOnly.FromDateTime(DateTime.Now))
            {
                _logger.LogWarning("Invalid membership date provided for borrower update for ID {BorrowerId}.", id);
                return BadRequest(GenericResult<BorrowerDTO>.Failure("Membership Date is required and must be a recent or past date."));
            }
            borrower.Name = updateBorrowerRequest.Name;
            borrower.MembershipDate = updateBorrowerRequest.MembershipDate.Value;
            var updateResult = await _borrowerService.UpdateAsync(borrower);
            if (updateResult.IsFailure)
            {
                _logger.LogError("Failed to update borrower with ID {BorrowerId}. Error: {Error}", id, updateResult.Error);
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<BorrowerDTO>.Failure(updateResult.Error));
            }
            var borrowerDTO = _mapper.Map<BorrowerDTO>(borrower);
            _logger.LogInformation("Borrower with ID {BorrowerId} updated successfully.", id);
            return Ok(GenericResult<BorrowerDTO>.Success(borrowerDTO));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<GenericResult<SlimBorrowerDTO>>> DeleteBorrower(int id)
        {
            var deleteResult = await _borrowerService.DeleteAsync(id);
            if (deleteResult.IsFailure)
            {
                _logger.LogWarning("Borrower with ID {BorrowerId} was not found for deletion.", id);
                return NotFound(GenericResult<SlimBorrowerDTO>.Failure($"Borrower with ID {id} not found."));
            }

            _logger.LogInformation("Borrower with ID {BorrowerId} deleted successfully.", id);
            return Ok(GenericResult<SlimBorrowerDTO>.Success(_mapper.Map<SlimBorrowerDTO>(deleteResult.Value)));
        }
    }
}
