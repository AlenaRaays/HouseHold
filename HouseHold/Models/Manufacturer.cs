using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class Manufacturer
    {
        [Key] public int manufacturer_id { get; set; }
        [Required, MaxLength(200)] public string name { get; set; }
        [Required] public int country_id { get; set; }
        [Required, MaxLength(20)] public string support_phone { get; set; }
        [Required, MaxLength(250)] public string support_email { get; set; }

        [ForeignKey("country_id")] public Country country { get; set; }
        public ICollection<Product> products { get; set; }
    }
}
    