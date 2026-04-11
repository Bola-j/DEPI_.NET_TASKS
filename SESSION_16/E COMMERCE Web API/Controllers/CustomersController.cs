using Azure.Core;
using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs.CustomerDTOs;
using E_COMMERCE_Web_API.DTOs.OrderDTOs;
using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;
        private readonly ILogger<CustomersController> _logger;
        private readonly IMapper _mapper;


        public CustomersController(ECommerceDbContext context, ILogger<CustomersController> logger, IMapper mapper)
        {
            _context = context;
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

            var query = _context.Customers
                .Select(c => _mapper.Map<CustomerDto>(c))
                .AsNoTracking();

            var searchTerm = search?.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(c => EF.Functions.Like(c.Name.ToLower(), $"%{searchTerm}%"));

            if(!query.Any())
            {
                _logger.LogWarning($"No customers found matching search term: {searchTerm}.");
                return NotFound(GenericResult<PagedResult<CustomerDto>>.Failure("No customers found matching the search criteria."));
            }

            var totalCount = await query.CountAsync();

            var customers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation($"Retrieved {customers.Count} customers (Page: {page}, PageSize: {pageSize}, TotalCount: {totalCount}).");

            return Ok(GenericResult<PagedResult<CustomerDto>>.Success(new PagedResult<CustomerDto>(customers, page, pageSize, totalCount)));
        }

        // GET api/customers/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenericResult<CustomerWithOrdersDto>>> GetById(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer is null)
            {
                _logger.LogWarning($"Customer with id {id} was not found.");
                return NotFound(GenericResult<CustomerWithOrdersDto>.Failure($"Customer with id {id} was not found."));
            }

            var dto = _mapper.Map<CustomerWithOrdersDto>(customer);

            return Ok(GenericResult<CustomerWithOrdersDto>.Success(dto));
        }

        

        // POST api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto dto)
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
                if(!EF.Functions.Like(dto.Email, "%@%.%"))
                {
                    _logger.LogWarning("Invalid email format provided.");
                    return BadRequest(GenericResult<CustomerDto>.Failure("Invalid email format."));
                }
            }
            var emailTaken = await _context.Customers
                .AnyAsync(c => c.Email == dto.Email);

            if (emailTaken )
            {
                _logger.LogWarning($"A customer with email {dto.Email} already exists.");
                return Conflict(GenericResult<CustomerDto>.Failure("A customer with this email already exists."));
            }

            var customer = _mapper.Map<Customer>(dto);

            _logger.LogInformation($"Creating new customer: {customer.Name} with email {customer.Email}.");
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Customer created with id {customer.Id}.");


            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, GenericResult<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer)));
        }

        // PUT api/customers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer is null)
            {
                _logger.LogWarning($"Customer with id {id} was not found.");
                return NotFound(GenericResult<CustomerDto>.Failure($"Customer with id {id} was not found."));
            }

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
                if (!EF.Functions.Like(dto.Email, "%@%.%"))
                {
                    _logger.LogWarning("Invalid email format provided.");
                    return BadRequest(GenericResult<CustomerDto>.Failure("Invalid email format."));
                }
            }

            _mapper.Map(dto, customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Customer with id {id} updated successfully.");
            return Ok(GenericResult<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer)));
        }

        // DELETE api/customers/{id}
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GenericResult<CustomerDto>>> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer is null)
            {
                _logger.LogWarning($"Customer with id {id} was not found.");
                return NotFound(GenericResult<CustomerDto>.Failure($"Customer with id {id} was not found."));
            }


            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Customer with id {id} deleted successfully.");

            return Ok(GenericResult<CustomerDto>.Success(_mapper.Map<CustomerDto>(customer)));
        }
    }
}