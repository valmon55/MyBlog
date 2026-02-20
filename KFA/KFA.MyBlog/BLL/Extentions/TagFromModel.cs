using KFA.MyBlog.BLL.ViewModels.Tag;
using KFA.MyBlog.DAL.Entities;

namespace KFA.MyBlog.BLL.Extentions
{
    public static class TagFromModel
    {
        public static Tag Convert(this Tag tag, TagViewModel tagViewModel)
        {
            tag.ID = tagViewModel.Id;
            tag.Tag_Name = tagViewModel.Tag_Name;

            return tag;
        }
    }
}
