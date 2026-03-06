using System.ComponentModel.DataAnnotations;

namespace KFA.MyBlog.API.ViewModels.Tag
{
    public class TagAddRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string Tag_Name { get; set; }
    }
}
