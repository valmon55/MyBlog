using KFA.MyBlog.DAL.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KFA.MyBlog.BLL.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Поле имя обязательно к заполнению")]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Поле фамилия обязательно к заполнению")]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string Last_Name { get; set; }

        [Display(Name = "Отчество", Prompt = "Введите отчество")]
        public string? Middle_Name { get; set; }
        [Required(ErrorMessage = "Поле Email обязательно к заполнению")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Год")]
        public int? Year { get; set; }
        [Required]
        [Display(Name = "Месяц")]
        public int? Month { get; set; }
        [Required]
        [Display(Name = "День")]
        public int? Day { get; set; }
        [Required(ErrorMessage = "Поле Логин обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Логин", Description = "Введите логин")]
        public string Login { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public Dictionary<UserRole, bool> CheckedRolesDic { get; set; }
    }
}
