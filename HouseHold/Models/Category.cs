using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class Category
    {
        [Key] public int category_id { get; set; }
        [Required, MaxLength(100)] public string name { get; set; }
        [Required] public int parent_category_id { get; set; }

        public ICollection<Product> products { get; set; }
        [ForeignKey("parent_category_id")] public ParentCategory parentCategory { get; set; }
    }

}
