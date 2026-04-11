using E_COMMERCE_Web_API.DTOs.OrderDetailDTO;
using System.ComponentModel.DataAnnotations;

namespace E_COMMERCE_Web_API.DTOs.OrderDTOs
{


    public class CreateOrderDto
    {
        [Required]
        public int CustomerId { get; set; }

        /// <summary>
        /// Optional: defaults to UtcNow in the service layer if omitted.
        /// </summary>
        public DateTime? OrderDate { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "An order must have at least one detail.")]
        public ICollection<CreateOrderDetailDto> OrderDetails { get; set; } = new List<CreateOrderDetailDto>();
    }

    public class UpdateOrderDto
    {
        public DateTime? OrderDate { get; set; }
    }



    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
    }

    public class OrderWithDetailsDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }
}