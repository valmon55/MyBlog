using KFA.MyBlog.BLL.ViewModels.Role;
using KFA.MyBlog.DAL.Entities;

namespace KFA.MyBlog.BLL.Extentions
{
    public static class RoleFromModel
    {
        public static UserRole Convert(this UserRole role, RoleViewModel roleeditvm)
        {
            role.Name = roleeditvm.Name;
            role.Description = roleeditvm.Description;

            return role;
        }
    }
}
