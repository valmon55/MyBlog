using KFA.MyBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFA.MyBlog.DAL.Repositories
{
    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(MyBlogContext db) : base(db) { }
        public List<UserRole> GetUserRoles()
        {
            return Set.ToList();
        }
        public UserRole GetUserRoleById(string id)
        {
            return Set.AsEnumerable().Where(x => x.Id == id).FirstOrDefault();
        }
        public UserRole GetUserRoleByName(string roleName)
        {
            return Set.Where(x => x.Name == roleName).FirstOrDefault();
        }
        public void AddUserRole(UserRole userRole)
        {
            Set.Add(userRole);
        }
        public void UpdateUserRole(UserRole userRole)
        {
            Update(userRole);
        }
        public void DeleteUserRole(UserRole userRole)
        {
            Delete(userRole);
        }

    }
}
