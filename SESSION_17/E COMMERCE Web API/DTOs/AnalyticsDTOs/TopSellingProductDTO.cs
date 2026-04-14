namespace E_COMMERCE_Web_API.DTOs.AnalyticsDTOs
{
    public class TopSellingProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
    }
}
