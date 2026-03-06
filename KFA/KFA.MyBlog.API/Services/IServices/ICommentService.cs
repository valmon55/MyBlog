using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.API.ViewModels.Comment;
using System.Collections.Generic;

namespace KFA.MyBlog.API.Services.IServices
{
    public interface ICommentService
    {
        public CommentViewModel AddComment(int articleId);
        public void AddComment(CommentAddRequest model, User user);
        public List<CommentViewRequest> AllArticleComments(int articleId);
        public int? DeleteComment(int id);
        public CommentViewModel UpdateComment(int id);
        public int UpdateComment(CommentEditRequest model, User user);
    }
}
