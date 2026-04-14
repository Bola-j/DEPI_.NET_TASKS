using System.ComponentModel.DataAnnotations;

namespace E_COMMERCE_Web_API.DTOs.OrderDetailDTO
{


    public class CreateOrderDetailDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }

    public class UpdateOrderDetailDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int? Quantity { get; set; }
    }



    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
