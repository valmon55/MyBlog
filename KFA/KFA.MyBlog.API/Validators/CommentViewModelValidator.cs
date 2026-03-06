using KFA.MyBlog.API.ViewModels.Comment;
using FluentValidation;

namespace KFA.MyBlog.API.Validators
{
    public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator() 
        { 
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Комментарий не должен быть пуст!");
        }
    }
}
