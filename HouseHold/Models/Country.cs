using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class Country
    {
        [Key] public int country_id { get; set; }
        [Required, MaxLength(50)] public string name { get; set; }

        [Required] public ICollection<Manufacturer> manufacturers { get; set; }
    }
}
