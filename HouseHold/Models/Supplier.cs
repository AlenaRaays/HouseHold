using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class Supplier
    {
        [Key] public int supplier_id { get; set; }
        [Required, MaxLength(200)] public string name { get; set; }
        [Required, MaxLength(500)] public string legal_address { get; set; }
        [MaxLength(20)] public string phone { get; set; } 
        [Required, MaxLength(100)] public string email { get; set; }
        [MaxLength(200)] public string contact_person { get; set; }

        public ICollection<SupplyDelivery> supplyDeliveries { get; set; }
        public ICollection<SupplyItem> supplyItems { get; set; }
        public ICollection<Product> products { get; set; }
    }
}
