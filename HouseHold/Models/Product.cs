using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class Product
    {
        [Key] public int product_id { get; set; }
        [Required, MaxLength(500)] public string name { get; set; }
        [Required, MaxLength(20)] public string article { get; set; }
        public double discount_percent { get; set; }
        [Required] double price { get; set; }
        [Required] public int amount { get; set; }
        [MaxLength(500)] public string description { get; set; }
        [Required] public bool is_visible { get; set; }
        [Required] public DateTime created_date { get; set; }
        [Required] public int supplier_id { get; set; }
        [Required] public int manufacturer_id { get; set; }
        [Required] public int category_id { get; set; }
        [Required] public double weight_kg { get; set; }
        [ForeignKey("supplier_id")] public Supplier supplier { get; set; }
        [ForeignKey("manufacturer_id")] public Manufacturer manufacturer { get; set; }
        [ForeignKey("category_id")] public Category category { get; set; }

        public ICollection<SupplyItem> supplyItems { get; set; }
        public ICollection<PriceHistory> priceHistories { get; set; }
        public ICollection<OrderItem> orderItems { get; set; }
        public ICollection<Comment> comments { get; set; }
        public ICollection<ProductImage> productImages { get; set; }
        public ICollection<ProductTag> productTags { get; set; }
    }
}
