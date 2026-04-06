using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class OrderItem
    {
        [Key] public int orderItem_id { get; set; }
        [Required] public int order_id { get; set; }
        [Required] public int product_id { get; set; }
        [Required] public int quantity { get; set; }
        [Required] public double price_at_order { get; set; }

        [ForeignKey("order_id")] public Orders Orders { get; set; }
        [ForeignKey("product_id")] public Product Product { get; set; }
        
    }
}
