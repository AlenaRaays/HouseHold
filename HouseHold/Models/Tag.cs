using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class Tag
    {
        [Key] public int tag_id { get; set; }
        [Required, MaxLength(50)] public string tag_name { get; set; }

        public ICollection<ProductTag> productTags { get; set; }
    }
}
