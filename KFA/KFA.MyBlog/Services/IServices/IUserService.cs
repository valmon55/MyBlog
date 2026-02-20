using KFA.MyBlog.BLL.ViewModels.User;

namespace KFA.MyBlog.Services.IServices
{
    public interface IUserService
    {
        public void Register(RegisterViewModel model);
        public void Login(LoginViewModel model);
        public Task<List<UserViewModel>> AllUsers();
        public UserViewModel UpdateUser(string userId);
        public Task UpdateUser(UserViewModel model, List<string> SelectedRoles);
        public void DeleteUser(string userId);
    }
}
