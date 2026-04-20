using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Article { get; set; }
        public double Price { get; set; }
        public double? OldPrice { get; set; }
        public int Quantity { get; set; }
        public double Total => Price * Quantity;


    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public double SubTotal => Items?.Sum(i => i.Total) ?? 0;

        public double Discount { get; set; }

        public double GrandTotal => SubTotal - Discount;
    }
}