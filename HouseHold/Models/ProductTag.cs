using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class ProductTag
    {
        [Key] public int productTag_id { get; set; }
        [Required] public int product_id { get; set; }
        [Required] public int tag_id { get; set; }

        [ForeignKey("product_id")] public Product Product { get; set; }
        [ForeignKey("tag_id")] public Tag Tag { get; set; }

    }
}
