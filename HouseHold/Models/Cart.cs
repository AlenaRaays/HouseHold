using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class Cart
    {
        [Key] public int cart_id { get; set; }
        public int user_id { get; set; }
        public Users User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
