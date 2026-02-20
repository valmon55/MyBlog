using KFA.MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.MyBlog.DAL.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(MyBlogContext db) : base(db) { }
        public List<Comment> GetComments()
        {
            return Set.ToList();
        }
        public Comment GetCommentById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public List<Comment> GetCommentsByArticleId(int articleId)
        {
            return Set.AsEnumerable().Where(c => c.ArticleId == articleId).ToList();
        }
        public void AddComment(Comment comment)
        {
            Set.Add(comment);
        }
        public void UpdateComment(Comment comment)
        {
            Update(comment);
        }
        public void DeleteComment(Comment comment)
        {
            Delete(comment);
        }
    }
}
