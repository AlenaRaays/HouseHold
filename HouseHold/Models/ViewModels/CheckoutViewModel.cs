// Models/ViewModels/CheckoutViewModel.cs
namespace HouseHold.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public double SubTotal { get; set; }
        public double Discount { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Total { get; set; }

        // Данные для формы
        public string DeliveryAddress { get; set; }
        public int DeliveryMethodId { get; set; }
        public int PaymentMethodId { get; set; }
        public string OrderComment { get; set; }

        // Списки для выбора
        public List<DeliveryMethod> DeliveryMethods { get; set; } = new();
        public List<PaymentMethod> PaymentMethods { get; set; } = new();
    }
}