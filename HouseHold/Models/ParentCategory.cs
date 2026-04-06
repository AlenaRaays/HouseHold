using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class ParentCategory
    {
        [Key] public int parent_category_id { get; set; }
        [Required, MaxLength(50)] public string name { get; set; }

        [Required] public ICollection<Category> categories{ get; set; }
    }
}
