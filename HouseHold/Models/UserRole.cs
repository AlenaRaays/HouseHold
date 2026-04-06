using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class UserRole
    {
        [Key] public int role_id { get; set; }
        [Required] public string role_name { get; set; }

        [Required] public ICollection<Users> Users { get; set; }
    }
}
