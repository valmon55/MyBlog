using FluentValidation;
using KFA.MyBlog.BLL.ViewModels.Comment;

namespace KFA.MyBlog.BLL.Validators
{
    public class CommentViewModelValidator : AbstractValidator<CommentViewModel>
    {
        public CommentViewModelValidator()
        {
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Комментарий не должен быть пуст!");
        }
    }
}
