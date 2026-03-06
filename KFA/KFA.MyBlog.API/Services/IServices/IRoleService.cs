using KFA.MyBlog.API.ViewModels.Role;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KFA.MyBlog.API.Services.IServices
{
    public interface IRoleService
    {
        public RoleRequest AddRole();
        public Task AddRole(RoleAddRequest model);
        public List<RoleRequest> AllRoles();
        public Task<RoleRequest> UpdateRole(string roleId);
        public Task UpdateRole(RoleRequest model);
        public Task DeleteRole(string roleId);
    }
}
