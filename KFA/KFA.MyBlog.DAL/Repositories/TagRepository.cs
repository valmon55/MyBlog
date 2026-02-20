using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL;
using KFA.MyBlog.DAL.Configs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.MyBlog.DAL.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(MyBlogContext db) : base(db) { }

        public List<Tag> GetTags()
        {
            return Set.ToList();
        }
        public Tag GetTagById(int id)
        {
            return Set.AsEnumerable().Where(x => x.ID == id).FirstOrDefault();
        }
        public void AddTag(Tag tag)
        {
            Set.Add(tag);
        }
        public void UpdateTag(Tag tag)
        {
            Update(tag);
        }
        public void DeleteTag(Tag tag)
        {
            Delete(tag);
        }
    }
}
