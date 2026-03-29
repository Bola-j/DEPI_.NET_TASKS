namespace E_COMMERCE_Web_API.DTOs.ProductsDTOs
{
    public class UpdateProductRequest
    {
        public string? NewProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? NewCategoryId { get; set; }
        public string? NewCategoryName { get; set; }
    }
}
