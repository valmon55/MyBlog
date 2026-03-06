using KFA.MyBlog.API.ViewModels.Role;
using KFA.MyBlog.DAL.Entities;

namespace KFA.MyBlog.API.Extentions
{
    public static class RoleFromModel
    {
        public static UserRole Convert(this UserRole role, RoleRequest roleeditvm)
        {
            role.Name = roleeditvm.Name;
            role.Description = roleeditvm.Description;

            return role;
        }
    }
}
