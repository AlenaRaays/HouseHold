using System.ComponentModel.DataAnnotations;

namespace HouseHold.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Введите фамилию"), MaxLength(20)] public string last_name { get; set; }
        [Required(ErrorMessage = "Введите имя"), MaxLength(15)] public string first_name { get; set; }
        [Required(ErrorMessage = "Введите телефон"), MaxLength(20)] public string phone { get; set; }
        [Required(ErrorMessage = "Введите email"), MaxLength(250), EmailAddress(ErrorMessage = "Неверный формат email")] public string email { get; set; }
        [Required(ErrorMessage = "Введите пароль"), DataType(DataType.Password)] public string password { get; set; }
        [Required(ErrorMessage = "Повторите пароль"), DataType(DataType.Password), Compare("Password", ErrorMessage ="Пароли не совпадают")] public string confirmPassword { get; set; }

    }
}
