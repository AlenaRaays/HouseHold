using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class Statuses
    {
        [Key] public int status_id { get; set; }
        [Required, MaxLength(50)] public string name { get; set; }
        public ICollection<SupplyDelivery> supplyDeliveries { get; set; }
    }
}
