using AutoMapper;
using KFA.MyBlog.BLL.Extentions;
using KFA.MyBlog.BLL.ViewModels.Role;
using KFA.MyBlog.Controllers;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.DAL.UoW;
using KFA.MyBlog.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace KFA.MyBlog.Services
{
    public class RoleService : IRoleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        public RoleViewModel AddRole()
        {
            _logger.LogInformation("Выполняется переход на страницу добавления роли.");
            return new RoleViewModel();
        }

        public async Task AddRole(RoleViewModel model)
        {
            //Инициализируем так, чтобы заполнить ID
            var role = new UserRole();

            var roleData = _mapper.Map<UserRole>(model);
            role.Name = roleData.Name;
            role.Description = roleData.Description;

            await _roleManager.CreateAsync(role);
            _logger.LogInformation($"Создана роль {role.Name}");
        }

        public List<RoleViewModel> AllRoles()
        {
            var repo = _unitOfWork.GetRepository<UserRole>() as UserRoleRepository;
            var roles = repo.GetUserRoles();
            var rolesView = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                rolesView.Add(_mapper.Map<RoleViewModel>(role));
            }
            _logger.LogInformation($"Просмотр всех ролей");

            return rolesView;
        }

        public async Task DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                _logger.LogInformation($"Роль с ID = {roleId} удалена.");
            }
        }

        public async Task<RoleViewModel> UpdateRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var roleView = _mapper.Map<RoleViewModel>(role);
            _logger.LogInformation($"Роль для обновления: {roleView.Name}");

            return roleView;
        }

        public async Task UpdateRole(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.ID);
            role.Convert(model);

            await _roleManager.UpdateAsync(role);
            _logger.LogInformation($"Роль {role.Name} обновлена");
        }
    }
}
