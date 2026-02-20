using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.MyBlog.DAL.Entities
{
    public class Comment
    {
        public int ID { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment_Text { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
