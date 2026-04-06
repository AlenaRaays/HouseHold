using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class ProductImage
    {
        [Key] public int image_id { get; set; }
        [Required] public int product_id { get; set; }
        [Required, MaxLength(500)] public string image_url { get; set; }
        [Required] public bool is_primary { get; set; }
        [ForeignKey("product_id")] public Product Product { get; set; }
    }
}
