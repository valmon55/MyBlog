using KFA.MyBlog.BLL.ViewModels.Comment;
using KFA.MyBlog.DAL.Entities;

namespace KFA.MyBlog.BLL.Extentions
{
    public static class CommentFromModel
    {
        public static Comment Convert(this Comment comment, CommentViewModel commentViewModel)
        {
            comment.Id = commentViewModel.Id;
            comment.ArticleId = commentViewModel.ArticleId;
            comment.UserId = commentViewModel.UserId;
            comment.CommentDate = commentViewModel.CommentDate;
            comment.Comment_Text = commentViewModel.Comment;

            return comment;
        }
    }
}
