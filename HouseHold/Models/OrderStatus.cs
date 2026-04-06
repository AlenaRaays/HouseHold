using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class OrderStatus
    {
        [Key] public int status_id { get; set; }
        [Required] public string status_name { get; set; }
        [Required] public string description { get; set; }

        [Required] public ICollection<Orders> orders { get; set; }
    }
}
