using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class DeliveryMethod
    {
        [Key] public int delivery_method_id { get; set; }
        [Required, MaxLength(100)] public string name { get; set; }
        [Required] public double cost { get; set; }
        [Required] public int delivery_days { get; set; }

        [Required] public ICollection<Orders> orders { get; set; }
    }
}
