using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class PriceHistory
    {
        [Key] public int history_id { get; set; }
        [Required] public int product_id { get; set; }
        public double old_price { get; set; }
        public double new_price { get; set; }
        public DateTime change_date { get; set; }
        [ForeignKey("product_id")] public Product Product { get; set; }
    }
}
