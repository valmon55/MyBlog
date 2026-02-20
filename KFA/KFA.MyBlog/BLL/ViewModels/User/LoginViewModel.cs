using System.ComponentModel.DataAnnotations;

namespace KFA.MyBlog.BLL.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле Email обязательно к заполнению")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Поле должно иметь минимум {0} символов", MinimumLength = 6)]
        public string Password { get; set; }

    }
}
