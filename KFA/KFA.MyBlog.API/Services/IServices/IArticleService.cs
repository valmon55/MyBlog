using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.API.ViewModels.Article;
using System.Collections.Generic;

namespace KFA.MyBlog.API.Services.IServices
{
    public interface IArticleService
    {
        public int AddArticle(ArticleAddRequest article, User user);
        public List<ArticleViewRequest> AllUserArticles();
        public List<ArticleViewRequest> AllArticles(User user = null);
        public void DeleteArticle(int id);
        public void UpdateArticle(ArticleEditRequest article, User user);
    }
}
