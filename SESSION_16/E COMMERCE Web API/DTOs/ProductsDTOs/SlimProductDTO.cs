using System.Text.Json.Serialization;


namespace E_COMMERCE_Web_API.DTOs.ProductsDTOs
{
    public class SlimProductDTO
    {
        [JsonPropertyName("ProductId")]
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyName("ProductName")]
        [JsonPropertyOrder(2)]
        public string Name { get; set; }
        [JsonPropertyName("ProductPrice")]
        [JsonPropertyOrder(3)]
        public decimal Price { get; set; }
    }
}
