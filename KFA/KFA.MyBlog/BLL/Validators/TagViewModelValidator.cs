using FluentValidation;
using KFA.MyBlog.BLL.ViewModels.Tag;

namespace KFA.MyBlog.BLL.Validators
{
    public class TagViewModelValidator : AbstractValidator<TagViewModel>
    {
        public TagViewModelValidator()
        {
            RuleFor(x => x.Tag_Name).NotEmpty().WithMessage("Название тега не должно быть пусто!");
        }
    }
}
