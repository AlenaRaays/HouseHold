using HouseHold.Models;

public class ShopViewModel
{
    public List<Product> Productt { get; set; }
    public List<Category> Categoryy { get; set; }
    public List<ParentCategory> parentCategories { get; set; }

    public int? UserId { get; set; }
    public string UserName { get; set; }

    public decimal? MaxPrice { get; set; }
    public decimal? MinPrice { get; set; }
    public List<int>? CategoryId { get; set; }
    public List<int>? PcategoryId { get; set; }
    public string? SortBy { get; set; }
    public string? SearchQuery { get; set; }
}
