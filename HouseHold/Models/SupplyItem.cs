using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class SupplyItem
    {
        [Key] public int supplyItem_id { get; set; }
        [Required] public int supplier_id { get; set; }
        [Required] public int product_id { get; set; }
        [Required] public int quantity { get; set; }
        [Required] public double purchase_price { get; set; }

        [ForeignKey("product_id")] public Product product { get; set; }
        [ForeignKey("supplier_id")] public Supplier supplier { get; set; }

    }
}
