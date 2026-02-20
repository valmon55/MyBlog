using KFA.MyBlog.BLL.ViewModels.Article;
using FluentValidation;

namespace KFA.MyBlog.BLL.Validators
{
    public class ArticleViewModelValidator : AbstractValidator<ArticleViewModel>
    {
        public ArticleViewModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Заголовок статьи не должен быть пуст!");
            //RuleFor(x => x.Title.Length).GreaterThan(5).WithMessage("Заголовок статьи должен быть длиннее 5 символов!");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Статья не должна быть пустая!");
            //RuleFor(x => x.ArticleDate).NotEmpty();
        }
    }
}
