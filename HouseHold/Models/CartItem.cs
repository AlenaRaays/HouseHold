using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class CartItem
    {
        [Key] public int cart_item_id { get; set; }

        public int cart_id { get; set; }
        public Cart Cart { get; set; }

        public int product_id { get; set; }
        public Product Product { get; set; }

        public int quantity { get; set; }
    }
}