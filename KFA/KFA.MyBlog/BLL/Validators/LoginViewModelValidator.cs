using FluentValidation;
using KFA.MyBlog.BLL.ViewModels.User;

namespace KFA.MyBlog.BLL.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email не должен быть пуст!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Пароль не должен быть пуст!");
        }
    }
}
