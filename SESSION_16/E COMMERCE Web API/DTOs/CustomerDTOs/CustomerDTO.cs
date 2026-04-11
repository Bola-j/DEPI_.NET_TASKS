using System.ComponentModel.DataAnnotations;
using E_COMMERCE_Web_API.DTOs.OrderDTOs;
namespace E_COMMERCE_Web_API.DTOs.CustomerDTOs
{

    public class CreateCustomerDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
    }

    public class UpdateCustomerDto
    {
        [StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }
    }



    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class CustomerWithOrdersDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}