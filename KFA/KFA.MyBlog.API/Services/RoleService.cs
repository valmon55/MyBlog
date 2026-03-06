using KFA.MyBlog.API.Controllers;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.DAL.Repositories;
using KFA.MyBlog.DAL.UoW;
using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.ViewModels.Role;
using KFA.MyBlog.API.Extentions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KFA.MyBlog.API.Services
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
        public RoleRequest AddRole()
        {
            _logger.LogInformation("Выполняется переход на страницу добавления роли.");
            return new RoleRequest();
        }

        public async Task AddRole(RoleAddRequest model)
        {
            //Инициализируем так, чтобы заполнить ID
            var role = new UserRole();

            //var roleData = _mapper.Map<UserRole>(model);
            role.Name = model.Name;
            role.Description = model.Description;

            await _roleManager.CreateAsync(role);
            _logger.LogInformation($"Создана роль {role.Name}");
        }

        public List<RoleRequest> AllRoles()
        {
            var repo = _unitOfWork.GetRepository<UserRole>() as UserRoleRepository;
            var roles = repo.GetUserRoles();
            var rolesView = new List<RoleRequest>();
            foreach (var role in roles)
            {
                rolesView.Add(_mapper.Map<RoleRequest>(role));
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

        public async Task<RoleRequest> UpdateRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var roleView = _mapper.Map<RoleRequest>(role);
            _logger.LogInformation($"Роль для обновления: {roleView.Name}");
            
            return roleView;
        }

        public async Task UpdateRole(RoleRequest model)
        {
            var role = await _roleManager.FindByIdAsync(model.ID);
            role.Convert(model);

            await _roleManager.UpdateAsync(role);
            _logger.LogInformation($"Роль {role.Name} обновлена");
        }
    }
}
