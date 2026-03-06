using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KFA.MyBlog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IRoleService _roleService;

        public RoleController(ILogger<UserController> logger,
                IRoleService roleService)
        {
            _logger = logger;
            _roleService = roleService;
        }
        /// <summary>
        /// Добавление роли
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "name": "string",
        ///  "description": "string"
        ///}
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleAddRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.AddRole(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Вывод всех ролей
        /// </summary>
        /// <returns></returns>
        [Route("AllRoles")]
        [HttpGet]
        public List<RoleRequest> AllRoles()
        {
            return _roleService.AllRoles();
        }
        /// <summary>
        /// Обновление роли
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "id": "string",
        ///  "name": "string",
        ///  "description": "string"
        ///}
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(RoleRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.UpdateRole(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Удаление роли
        /// Доступно только пользователям с ролью Admin
        /// </summary>
        /// <param name="roleId"> Id роли </param>
        /// <returns></returns>
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string roleId)
        {
            try
            {
                await _roleService.DeleteRole(roleId);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(403);
            }
        }
    }
}
