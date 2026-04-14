using HouseHold.Models;

public class ShopViewModel
{
    public List<Product> Productt { get; set; }
    public List<Category> Categoryy { get; set; }
    public List<ParentCategory> parentCategories { get; set; }

    public int? UserId { get; set; }
    public string UserName { get; set; }

    public decimal MaxPrice { get; set; }

    public List<int> SelectedCategories { get; set; } = new();
    public decimal? MinPrice { get; set; }
}
