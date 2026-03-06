using System;
using System.ComponentModel.DataAnnotations;

namespace KFA.MyBlog.API.ViewModels.Comment
{
    public class CommentAddRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string Comment { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public int ArticleId { get; set; }
    }
}
