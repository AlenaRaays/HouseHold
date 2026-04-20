// Models/FavoriteViewModel.cs
namespace HouseHold.Models
{
    public class FavoriteViewModel
    {
        public List<Favorite> Favorites { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int TotalCount => Favorites?.Count ?? 0;
    }
}