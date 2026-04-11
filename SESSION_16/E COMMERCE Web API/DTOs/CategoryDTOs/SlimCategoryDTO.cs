using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace E_COMMERCE_Web_API.DTOs.CategoryDTOs
{
    public class SlimCategoryDTO
    {
        [JsonPropertyName("CategoryId")]
        [JsonPropertyOrder(1)]
        public int Id { get; set; }
        [JsonPropertyName("CategoryName")]
        [JsonPropertyOrder(2)]
        public string Name { get; set; } = string.Empty;
    }
}
