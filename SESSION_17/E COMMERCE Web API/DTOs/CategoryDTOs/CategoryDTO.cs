using E_COMMERCE_Web_API.DTOs.ProductsDTOs;
using System.Text.Json.Serialization;


namespace E_COMMERCE_Web_API.DTOs.CategoryDTOs
{
    public class CategoryDTO : SlimCategoryDTO
    {
        [JsonPropertyName("products")]
        [JsonPropertyOrder(3)]
        public ICollection<SlimProductDTO> Products { get; set; }

        
    }
}
