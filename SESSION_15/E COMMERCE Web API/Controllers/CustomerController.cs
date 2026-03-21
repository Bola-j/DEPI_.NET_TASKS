using E_COMMERCE_Web_API.Data;
using E_COMMERCE_Web_API.DTOs;
using E_COMMERCE_Web_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_COMMERCE_Web_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ECommerceDbContext _context;

        public CustomersController(ECommerceDbContext context)
        {
            _context = context;
        }

        // GET api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email
                })
                .ToListAsync();

            return Ok(customers);
        }

        // GET api/customers/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerWithOrdersDto>> GetById(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer is null)
                return NotFound($"Customer with id {id} was not found.");

            var dto = new CustomerWithOrdersDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Orders = customer.Orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    CustomerId = o.CustomerId
                })
            };

            return Ok(dto);
        }

        // POST api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto dto)
        {
            var emailTaken = await _context.Customers
                .AnyAsync(c => c.Email == dto.Email);

            if (emailTaken)
                return Conflict("A customer with this email already exists.");

            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var responseDto = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, responseDto);
        }

        // PUT api/customers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer is null)
                return NotFound($"Customer with id {id} was not found.");

            if (dto.Name is not null) customer.Name = dto.Name;
            if (dto.Email is not null) customer.Email = dto.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/customers/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer is null)
                return NotFound($"Customer with id {id} was not found.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}