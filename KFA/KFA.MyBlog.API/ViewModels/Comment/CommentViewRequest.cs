using System;
using System.ComponentModel.DataAnnotations;

namespace KFA.MyBlog.API.ViewModels.Comment
{
    public class CommentViewRequest
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Comment { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CommentDate { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string UserId { get; set; }
    }
}
