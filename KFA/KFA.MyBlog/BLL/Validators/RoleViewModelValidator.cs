using FluentValidation;
using KFA.MyBlog.BLL.ViewModels.Role;

namespace KFA.MyBlog.BLL.Validators
{
    public class RoleViewModelValidator : AbstractValidator<RoleViewModel>
    {
        public RoleViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Название роли не должно быть пусто!");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Описание роли не должно быть пусто!");
        }
    }
}
