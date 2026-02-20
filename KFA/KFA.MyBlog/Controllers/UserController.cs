using AutoMapper;
using KFA.MyBlog.BLL.ViewModels.User;
using KFA.MyBlog.DAL.Entities;
using KFA.MyBlog.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KFA.MyBlog.Controllers
{
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

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            _logger.LogInformation("Регистрация нового пользователя.");
            return View(new RegisterViewModel());
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ///Создание пользователей с 3 разными ролями
                var userRole = new UserRole() { Name = "User", Description = "Пользователь" };
                //var userRole = new UserRole() { Name = "Admin", Description = "Администратор" };
                //var userRole = new UserRole() { Name = "Moderator", Description = "Модератор" };

                //var roles = _roleManager.Roles.ToList();
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

                    _logger.LogInformation($"Пользователь {user.Last_Name} {user.First_Name} зарегистрирован.");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogError("Возникли ошибки при регистрации:");
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Код ошибки: {error.Code}{Environment.NewLine}Описание: {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);
                User signedUser = _userManager.Users.Include(x => x.userRole).FirstOrDefault(u => u.Email == model.Email);
                var userRole = _userManager.GetRolesAsync(signedUser).Result.FirstOrDefault();
                if (signedUser is null)
                {
                    _logger.LogError($"Логин {user.Email} не найден");
                    ModelState.AddModelError("", "Неверный логин!");
                }
                /// Если ролей почему-то нет, то устанавливаем:
                /// для пользователя Admin - роль Admin
                /// для остальных - User
                if (userRole is null)
                {
                    _logger.LogError($"У пользователя {signedUser.UserName} нет роли!");
                    if (signedUser.UserName == "Admin")
                    {
                        await _userManager.AddToRoleAsync(signedUser, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(signedUser, "User");
                    }
                    userRole = _userManager.GetRolesAsync(signedUser).Result.FirstOrDefault();
                    _logger.LogWarning($"Пользователю {signedUser.userRole} присвоили роль {userRole}");
                }

                if (signedUser != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
                    };

                    await _signInManager.SignInWithClaimsAsync(signedUser, isPersistent: false, claims);
                }
                else
                {
                    _logger.LogError($"Логин {user.Email} не найден");
                    ModelState.AddModelError("", $"Логин {user.Email} не найден");
                }
            }
            _logger.LogInformation($"Перенаправление на главную страницу.");
            return RedirectToAction("Index", "Home");
        }
        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"Выполнен Logout.");

            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        [Route("AllUsers")]
        [HttpGet]
        public async Task<IActionResult> AllUsersAsync()
        {
            //_logger.LogInformation($"Вывод списка всех пользователей.");
            //var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            //var users = repo.GetUsers();

            //var usersView = new List<UserViewModel>();

            //foreach (var user in users)
            //{
            //    var userView = _mapper.Map<UserViewModel>(user);

            //    var userRoleNames = await _userManager.GetRolesAsync(user);
            //    var allRoles = await _roleManager.Roles.ToListAsync();
            //    var userRoles = new List<UserRole>();

            //    foreach(var role in allRoles)
            //    {
            //        if(userRoleNames.Contains(role.Name))
            //        {
            //            userRoles.Add(role);
            //        }
            //    }
            //    userView.UserRoles = userRoles;

            //    usersView.Add(userView);
            //}

            //return View(usersView);
            return View(await _userService.AllUsers());
        }

        [Authorize(Roles = "Admin")]
        [Route("User/Update")]
        [HttpGet]
        public IActionResult Update(string userId)
        {
            //var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            //var user = repo.GetUserById(userId);
            //var userView = _mapper.Map<UserViewModel>(user);
            //_logger.LogInformation($"Пользователь для обновления: {user.UserName}");

            //var allRoles = _roleManager.Roles.ToList();
            //var checkedRolesDic = new Dictionary<UserRole, bool>();
            //foreach (var role in allRoles)
            //{
            //    if (_userManager.IsInRoleAsync(user, role.Name).Result)
            //    {
            //        checkedRolesDic.Add(role, true);
            //    }
            //    else
            //    {
            //        checkedRolesDic.Add(role, false);
            //    }
            //}

            //userView.CheckedRolesDic = checkedRolesDic;

            //return View("EditUser", userView);
            return View("EditUser", _userService.UpdateUser(userId));
        }

        [Authorize(Roles = "Admin")]
        [Route("User/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UserViewModel model, List<string> SelectedRoles)
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.FindByIdAsync(model.Id);

                //var roles = await _roleManager.Roles.ToListAsync();

                //foreach (var role in roles)
                //{
                //    //определяем есть ли роль у пользователя
                //    var IsInRole = await _userManager.IsInRoleAsync(user, role.Name);

                //    //добавляем роль
                //    if (SelectedRoles.Contains(role.Id) && !IsInRole)
                //    {
                //        await _userManager.AddToRoleAsync(user, role.Name);
                //    }
                //    //убираем роль
                //    if (!SelectedRoles.Contains(role.Id) && IsInRole)
                //    {
                //        await _userManager.RemoveFromRoleAsync(user, role.Name);
                //    }

                //}

                //user.Convert(model);
                //await _userManager.UpdateAsync(user);
                //_logger.LogInformation($"Пользователь {user.UserName} обновлен.");
                await _userService.UpdateUser(model, SelectedRoles);
            }
            else
            {
                _logger.LogError("Модель UserViewModel не прошла проверку!");
                ModelState.AddModelError("", "Некорректные данные");
            }
            _logger.LogInformation($"Перенаправление на страницу просмотра всех пользователей");

            return RedirectToAction("AllUsers");
        }
        [Authorize(Roles = "Admin")]
        [Route("User/Delete")]
        [HttpPost]
        public IActionResult Delete(string userId)
        {
            //var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            //var user = repo.GetUserById(userId);
            //repo.DeleteUser(user);
            //_logger.LogInformation($"Пользователь с ID = {userId} удален.");
            _userService.DeleteUser(userId);

            return RedirectToAction("AllUsers");
        }

    }
}
