using KFA.MyBlog.API.ViewModels.Tag;
using KFA.MyBlog.DAL.Entities;

namespace KFA.MyBlog.API.Extentions
{
    public static class TagFromModel
    {
        public static Tag Convert(this Tag tag, TagViewModel tagViewModel)
        {
            tag.Id = tagViewModel.Id;
            tag.Tag_Name = tagViewModel.Tag_Name;

            return tag;
        }
    }
}
