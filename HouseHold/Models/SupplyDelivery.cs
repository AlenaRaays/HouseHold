using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class SupplyDelivery
    {
        [Key] public int supply_id { get; set; }
        [Required] public int supplier_id { get; set; }
        [Required, MaxLength(50)] public string invoice_number { get; set; }
        [Required] public DateTime delivery_date { get; set; }
        [Required] public int total_amount { get; set; }
        [Required] public int status_id { get; set; }
        [ForeignKey("status_id")] public Statuses statuses { get; set; }
        [ForeignKey("supplier_id")] public Supplier supplier { get; set; }
    }
}
