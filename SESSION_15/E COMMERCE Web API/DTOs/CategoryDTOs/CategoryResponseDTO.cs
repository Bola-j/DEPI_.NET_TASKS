using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using System.Text.Json.Serialization;


namespace E_COMMERCE_Web_API.DTOs.CategoryDTOs
{
    public class CategoryResponseDTO : SlimCategoryResponseDTO
    {
        [JsonPropertyName("products")]
        [JsonPropertyOrder(3)]
        public ICollection<SlimProductsResponseDTO> Products { get; set; }

        
    }
}
