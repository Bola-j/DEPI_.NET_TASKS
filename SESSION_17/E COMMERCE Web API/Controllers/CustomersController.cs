using Azure.Core;
using E_COMMERCE_Web_API.DTOs.CustomerDTOs;
using E_COMMERCE_Web_API.DTOs.OrderDTOs;
using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;
using E_COMMERCE_Web_API.Services;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;


namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;
        private readonly IMapper _mapper;


        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger, IMapper mapper)
        {
            _customerService = customerService;
            _logger = logger;
            _mapper = mapper;
        }

        // GET api/customers
        [HttpGet]
        public async Task<ActionResult<GenericResult<PagedResult<CustomerDto>>>> GetAll(string? search = null, int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                _logger.LogWarning($"Invalid pagination parameters: page={page}, pageSize={pageSize}.");
                return BadRequest(GenericResult<PagedResult<CustomerDto>>.Failure("Page and PageSize must be greater than 0."));
            }

            var result = await _customerService.GetAllAsync(search, page, pageSize);
            if (result.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<PagedResult<CustomerDto>>.Failure(result.Error));
            }

            if(!result.Value.Data.Any())
            {
                _logger.LogWarning($"No customers found matching search term: {search?.Trim().ToLower()}.");
                return NotFound(GenericResult<PagedResult<CustomerDto>>.Failure("No customers found matching the search criteria."));
            }

            var customers = result.Value.Data.Select(_mapper.Map<CustomerDto>).ToList();

            _logger.LogInformation($"Retrieved {customers.Count} customers (Page: {page}, PageSize: {pageSize}, TotalCount: {result.Value.TotalCount}).");

            return Ok(GenericResult<PagedResult<CustomerDto>>.Success(new PagedResult<CustomerDto>(customers, page, pageSize, result.Value.TotalCount)));
        }

        // GET api/customers/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenericResult<CustomerWithOrdersDto>>> GetById(int id)
        {
            var result = await _customerService.GetByIdAsync(id);

            if (result.IsFailure)
            {
                _logger.LogWarning($"Customer with id {id} was not found.");
                return NotFound(GenericResult<CustomerWithOrdersDto>.Failure($"Customer with id {id} was not found."));
            }

            var dto = _mapper.Map<CustomerWithOrdersDto>(result.Value);

            return Ok(GenericResult<CustomerWithOrdersDto>.Success(dto));
        }

        

        // POST api/customers
        [HttpPost]
        public async Task<ActionResult<GenericResult<CustomerDto>>> Create(CreateCustomerDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name == "" || dto.Name == "string")
            {
                _logger.LogWarning("Invalid customer name provided.");
                return BadRequest(GenericResult<CustomerDto>.Failure("Invalid customer name."));
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || dto.Email == "" || dto.Email == "string")
            {
                _logger.LogWarning("Invalid customer email provided.");
                return BadRequest(GenericResult<CustomerDto>.Failure("Invalid customer email."));
            }
            else
            {
                if(!System.Text.RegularExpressions.Regex.IsMatch(dto.Email, @".*@.*\..*"))
                {
                    _logger.LogWarning("Invalid email format provided.");
                    return BadRequest(GenericResult<CustomerDto>.Failure("Invalid email format."));
                }
            }
            var emailTaken = await _customerService.EmailExistsAsync(dto.Email);

            if (emailTaken )
            {
                _logger.LogWarning($"A customer with email {dto.Email} already exists.");
                return Conflict(GenericResult<CustomerDto>.Failure("A customer with this email already exists."));
            }

            var customer = _mapper.Map<Customer>(dto);

            _logger.LogInformation($"Creating new customer: {customer.Name} with email {customer.Email}.");
            var createResult = await _customerService.CreateAsync(customer);
            if (createResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<CustomerDto>.Failure(createResult.Error));
            }

            _logger.LogInformation($"Customer created with id {customer.Id} in the database.");

            _logger.LogInformation($"Setting CreatedBy for customer with id {customer.Id} to {customer.Id}.");
            customer.CreatedBy = customer.Id;
            await _customerService.UpdateAsync(customer);
            _logger.LogInformation($"CreatedBy for customer with id {customer.Id} set to {customer.Id} successfully.");



            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, GenericResult<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer)));
        }

        // PUT api/customers/{id}
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GenericResult<CustomerDto>>> Update(int id, UpdateCustomerDto dto)
        {
            var getResult = await _customerService.GetByIdAsync(id);

            if (getResult.IsFailure)
            {
                _logger.LogWarning($"Customer with id {id} was not found.");
                return NotFound(GenericResult<CustomerDto>.Failure($"Customer with id {id} was not found."));
            }

            var customer = getResult.Value;

            if (string.IsNullOrWhiteSpace(dto.Name) || dto.Name == "" || dto.Name == "string")
            {
                _logger.LogWarning("Invalid customer name provided.");
                return BadRequest(GenericResult<CustomerDto>.Failure("Invalid customer name."));
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || dto.Email == "" || dto.Email == "string")
            {
                _logger.LogWarning("Invalid customer email provided.");
                return BadRequest(GenericResult<CustomerDto>.Failure("Invalid customer email."));
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Email, @".*@.*\..*"))
                {
                    _logger.LogWarning("Invalid email format provided.");
                    return BadRequest(GenericResult<CustomerDto>.Failure("Invalid email format."));
                }
            }

            _mapper.Map(dto, customer);
            var updateResult = await _customerService.UpdateAsync(customer);
            if (updateResult.IsFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, GenericResult<CustomerDto>.Failure(updateResult.Error));
            }
            _logger.LogInformation($"Customer with id {id} updated successfully.");
            return Ok(GenericResult<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer)));
        }

        // DELETE api/customers/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GenericResult<CustomerDto>>> Delete(int id)
        {
            var deleteResult = await _customerService.DeleteAsync(id);

            if (deleteResult.IsFailure)
            {
                _logger.LogWarning($"Customer with id {id} was not found.");
                return NotFound(GenericResult<CustomerDto>.Failure($"Customer with id {id} was not found."));
            }

            _logger.LogInformation($"Customer with id {id} deleted successfully.");

            return Ok(GenericResult<CustomerDto>.Success(_mapper.Map<CustomerDto>(deleteResult.Value)));
        }
    }
}