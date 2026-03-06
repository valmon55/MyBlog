using System;
using System.ComponentModel.DataAnnotations;

namespace KFA.MyBlog.API.ViewModels.Comment
{
    public class CommentEditRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Comment { get; set; }
    }
}
