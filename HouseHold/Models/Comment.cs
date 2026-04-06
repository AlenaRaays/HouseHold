using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class Comment
    {
        [Key] public int comment_id { get; set; }
        [Required] public int product_id { get; set; }
        [Required] public int user_id { get; set; }
        [Required, MaxLength(2000)] public string comment_text { get; set; }
        [Required] public int rating { get; set; }
        [Required] public DateTime created_date { get; set; }
        [Required] public bool moderation_status { get; set; }
        [ForeignKey("product_id")] public Product product { get; set; }
        [ForeignKey("user_id")] public Users users{ get; set; }


    }
}
