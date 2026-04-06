using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class Users
    {
        [Key] public int user_id { get; set; }
        [Required, MaxLength(20)] public string last_name { get; set; }
        [Required, MaxLength(15)] public string first_name { get; set; }
        [Required, MaxLength(20)] public string phone { get; set; }
        [Required, MaxLength(250)] public string email { get; set; }
        [Required] public DateTime registration_date { get; set; }
        [Required] public bool is_active { get; set; }
        [Required, MaxLength(20)] public string password { get; set; }
        [Required] public int role_id { get; set; }
        public double discount { get; set; }
        public DateTime last_login_date { get; set; }

        public ICollection<Comment> comments { get; set; }
        public ICollection<Orders> orders { get; set; }
        [ForeignKey("role_id")] public UserRole UserRole { get; set; }
    }
}
