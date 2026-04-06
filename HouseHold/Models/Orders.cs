using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseHold.Models
{
    public class Orders
    {
        [Key] public int order_id { get; set; }
        [Required, MaxLength(20)] public string order_number { get; set; }
        [Required] public int user_id { get; set; }
        [Required] public int status_id { get; set; }
        [Required] public int delivery_method_id { get; set; }
        [Required] public int payment_method_id { get; set; }
        [Required] public DateTime created_date { get; set; }
        [Required] public DateTime completion_date { get; set; }
        [Required] public int total_amount { get; set; }
        [Required, MaxLength(500)] public string delivery_address { get; set; }
        [Required, MaxLength(500)] public string order_comment { get; set; }
        [Required] public double discount_at_order { get; set; }

        [ForeignKey("user_id")] public Users user { get; set; }
        [ForeignKey("status_id")] public OrderStatus status { get; set; }
        [ForeignKey("delivery_method_id")] public DeliveryMethod deliveryMethod { get; set; }
        [ForeignKey("payment_method_id")] public PaymentMethod paymentMethod { get; set; }

        public ICollection<OrderItem> orderItems { get; set; }
    }
}
