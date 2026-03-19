namespace E_COMMERCE_Web_API.DTOs.ProductsDTOs
{
    public class CreateProductReuestDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
