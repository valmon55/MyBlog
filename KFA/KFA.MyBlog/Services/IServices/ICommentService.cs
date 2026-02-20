using KFA.MyBlog.BLL.ViewModels.Comment;
using KFA.MyBlog.DAL.Entities;

namespace KFA.MyBlog.Services.IServices
{
    public interface ICommentService
    {
        public CommentViewModel AddComment(int articleId);
        public void AddComment(CommentViewModel model, User user);
        public List<CommentViewModel> AllArticleComments(int articleId);
        public int? DeleteComment(int id);
        public CommentViewModel UpdateComment(int id);
        public int UpdateComment(CommentViewModel model);
    }
}
