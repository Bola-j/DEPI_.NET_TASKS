using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using System.Text.Json.Serialization;

namespace E_COMMERCE_Web_API.DTOs.ProductsDTOs
{
    public class ProductDTO : SlimProductDTO
    {

        [JsonPropertyName("Category")]
        [JsonPropertyOrder(4)]
        public SlimCategoryDTO? Category { get; set; }


    }
}
