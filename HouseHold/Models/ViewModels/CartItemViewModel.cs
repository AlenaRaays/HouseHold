using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }

    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();
        public decimal GrandTotal => Items.Sum(x => x.Total);
    }
}