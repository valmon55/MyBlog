using KFA.MyBlog.API.ViewModels.Tag;
using FluentValidation;

namespace KFA.MyBlog.API.Validators
{
    public class TagViewModelValidator : AbstractValidator<TagViewModel>
    {
        public TagViewModelValidator() 
        { 
            RuleFor(x => x.Tag_Name).NotEmpty().WithMessage("Название тега не должно быть пусто!");
        }
    }
}
