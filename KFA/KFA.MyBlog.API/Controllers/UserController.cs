using KFA.MyBlog.DAL.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using AutoMapper;
using KFA.MyBlog.API.Services.IServices;
using KFA.MyBlog.API.ViewModels;

namespace KFA.MyBlog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUserService _userService;

        public UserController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager,
                IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userService = userService;
        }
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "first_Name": "string",
        ///  "last_Name": "string",
        ///  "middle_Name": "string",
        ///  "email": "user@example.com",
        ///  "year": 0,
        ///  "month": 0,
        ///  "day": 0,
        ///  "passwordReg": "string",
        ///  "passwordConfirm": "string",
        ///  "login": "string"
        /// }
        /// </remarks>
        /// <param name="model"> Данные регистрируемого пользователя </param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(KFA.MyBlog.API.ViewModels.RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                ///Создание пользователей с 3 разными ролями
                var userRole = new UserRole() { Name = "User", Description = "Пользователь" };
                //var userRole = new UserRole() { Name = "Admin", Description = "Администратор" };
                //var userRole = new UserRole() { Name = "Moderator", Description = "Модератор" };

                if (!_roleManager.RoleExistsAsync(userRole.Name).Result)
                {
                    await _roleManager.CreateAsync(userRole);
                }

                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var currentUser = await _userManager.FindByIdAsync(Convert.ToString(user.Id));

                    ///добавляет в таблицу [AspNetUserRoles] соответствие между ролью и пользователем
                    await _userManager.AddToRoleAsync(currentUser, userRole.Name);

                    await _signInManager.RefreshSignInAsync(currentUser);

                    _logger.LogInformation($"Пользователь {1} {2} зарегистрирован.", user.Last_Name, user.First_Name);

                    return StatusCode(201);
                }
                else
                {
                    _logger.LogError("Возникли ошибки при регистрации:");
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Код ошибки: {1} {Environment.NewLine} Описание: {2}", error.Code, error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return StatusCode(403);
                }
            }
            else
            {
                _logger.LogError("Возникли ошибки в данных для регистрации пользователя.");
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Аутентификация пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "email": "user@example.com",
        ///  "password": "string"
        ///}
        /// </remarks>
        /// <param name="model"> Данные для входа в систему </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="AuthenticationException"></exception>
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(KFA.MyBlog.API.ViewModels.LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException($"Запрос не корректен. Email = {model.Email}, Password = {model.Password}");

            var result = await _userService.Login(model);

            if (!result.Succeeded)
                throw new AuthenticationException("Введенный пароль не корректен или не найден аккаунт");

            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            if (roles.Contains("Администратор"))
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, "Администратор"));
            }
            else
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, roles.First()));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            return StatusCode(200);
        }
        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation($"Выполнен Logout.");
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(403);
            }
        }
        /// <summary>
        /// Вывод списка всех пользователей
        /// Доступно только для пользователей с ролью Admin
        /// </summary>
        /// <returns> Список пользоватей </returns>
        [Authorize(Roles = "Admin")]
        [Route("AllUsers")]
        [HttpGet]
        public async Task<List<UserViewRequest>> AllUsersAsync()
        {
            return await _userService.AllUsers();
        }
        /// <summary>
        /// Обновление данных пользователя
        /// Доступно только для пользователей с ролью Admin
        /// </summary>
        /// <remarks>
        /// Пример запроса
        /// POST
        /// {
        ///  "id": "string",
        ///  "first_Name": "string",
        ///  "last_Name": "string",
        ///  "middle_Name": "string",
        ///  "email": "user@example.com",
        ///  "birthDate": "2024-08-09",
        ///  "login": "string",
        ///  "userRoles": [
        ///    {
        ///      "id": "string",
        ///      "name": "string",
        ///      "description": "string"
        ///    }
        ///  ]
        /// }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UserEditRequest model)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUser(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель UserViewModel не прошла проверку!");
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Удаление прользователя
        /// Доступно только для пользователей с ролью Admin
        /// </summary>
        /// <param name="userId">Id удаляемого пользователя </param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [Route("Delete")]
        [HttpDelete]
        public IActionResult Delete(string userId)
        {
            try
            {
                _userService.DeleteUser(userId);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
