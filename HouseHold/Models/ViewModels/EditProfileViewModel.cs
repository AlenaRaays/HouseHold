using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models.ViewModels
{
    public class EditProfileViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [StringLength(50, ErrorMessage = "Имя не может превышать 50 символов")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна")]
        [StringLength(50, ErrorMessage = "Фамилия не может превышать 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [StringLength(100, ErrorMessage = "Email не может превышать 100 символов")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Телефон обязателен")]
        [Phone(ErrorMessage = "Некорректный формат телефона")]
        [StringLength(20, ErrorMessage = "Телефон не может превышать 20 символов")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = string.Empty;

        // Только для отображения (не редактируются)
        public string? RoleName { get; set; }
        public bool? Status { get; set; }
        public double PersonalDiscount { get; set; }
    }
}