using System.ComponentModel.DataAnnotations;

namespace JWTPolicyBasedAuthorization.Models
{
    public class Product : BaseModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Product Name should not exceed 50 chars")]
        public string Name { get; set; }

        [Required]
        public int BrandId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [StringLength(20, ErrorMessage = "Product Name should not exceed 20 chars")]
        public string ManufacturerPartNumber { get; set; }
        public string Specifications { get; set; }

        [Required]
        public double Price { get; set; }

        public ConfigValue Brand { get; set; }

        public ConfigValue Category { get; set; }
    }
}