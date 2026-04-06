using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class PaymentMethod
    {
        [Key] public int payment_method_id { get; set; }
        [Required] public string name { get; set; }
        
        [Required] public ICollection<Orders> orders { get; set; }
    }
}
