using KFA.MyBlog.BLL.ViewModels.Tag;

namespace KFA.MyBlog.Services.IServices
{
    public interface ITagService
    {
        public TagViewModel AddTag();
        public void AddTag(TagViewModel model);
        public List<TagViewModel> AllTags();
        public void DeleteTag(int id);
        public TagViewModel UpdateTag(int id);
        public void UpdateTag(TagViewModel model);

    }
}
