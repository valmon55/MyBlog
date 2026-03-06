using KFA.MyBlog.API.ViewModels;
using FluentValidation;

namespace KFA.MyBlog.API.Validators
{
    public class LoginViewModelValidator : AbstractValidator<LoginRequest>
    {
        public LoginViewModelValidator() 
        { 
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email не должен быть пуст!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Пароль не должен быть пуст!");
        }
    }
}
