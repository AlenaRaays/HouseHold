using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class Favorite
    {
        [Key]
        public int favorite_id { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }
        public Users User { get; set; }

        [ForeignKey("Product")]
        public int product_id { get; set; }
        public Product Product { get; set; }

        public DateTime added_date { get; set; } = DateTime.Now;
    }
}