using KFA.MyBlog.API.ViewModels.Role;
using FluentValidation;

namespace KFA.MyBlog.API.Validators
{
    public class RoleViewModelValidator : AbstractValidator<RoleRequest>
    {
        public RoleViewModelValidator() 
        { 
            RuleFor(x => x.Name).NotEmpty().WithMessage("Название роли не должно быть пусто!");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Описание роли не должно быть пусто!");
        }
    }
}
