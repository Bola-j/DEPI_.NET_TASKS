using E_COMMERCE_Web_API.Entities;
using E_COMMERCE_Web_API.DTOs.CategoryDTOs;
using System.Text.Json.Serialization;

namespace E_COMMERCE_Web_API.DTOs.ProductsDTOs
{
    public class ProductsResponseDTO : SlimProductsResponseDTO
    {

        [JsonPropertyName("Category")]
        [JsonPropertyOrder(4)]
        public SlimCategoryResponseDTO? Category { get; set; }


    }
}
