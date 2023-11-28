using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SysPet.Data;
using SysPet.Exception;
using SysPet.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]

    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UsersData _usersData;
        public UserController(ILogger<HomeController> logger)
        {
            _usersData = new UsersData();
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Singin()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear();

            return RedirectToAction("LogIn", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Singin(UsuariosViewModel model)
        {

            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Contrasenia))
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                return View(model);
            }
            else
            {
                var user = await _usersData.GetUserManager(model.Email, model.Contrasenia);
                if (user != null && model.Email == user.Email && model.Contrasenia == user.Contrasenia)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            
            return View(model);
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogIn(UsuariosViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Contrasenia))
                {
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                    return View(model);
                }
                else
                {
                    var user = await _usersData.GetUserManager(model.Email, model.Contrasenia);
                    if (user == null || model.Email != user.Email || model.Contrasenia != user.Contrasenia)
                    {
                        ModelState.AddModelError("", "Nombre de usuario o contraseña incorrectos.");
                        return View(model);
                    }

                    var rol = user.IdRol == 1 ? "Administrador" : "Usuario";

                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre), // Nombre de usuario
                    new Claim(ClaimTypes.Role, rol), // Rol del usuario
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(5),
                        AllowRefresh = false,
                    };

                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                    HttpContext.Session.SetString("User", user.Nombre);
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new System.Exception(ex.Message, ex);
            }
        }

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Url = "Shared/EmptyData";
                return View(await _usersData.GetAll());

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                return View(await _usersData.GetItem(id));

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: UserController/Create
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Create()
        {
            var model = new UsuariosViewModel();

            var roles = await _usersData.GetRoles();
            var listRoles = roles.Select(x => new SelectListItem
            {
                Value = x.IdRol.ToString(),
                Text = x.Nombre
            }).ToList();

            model.Roles = listRoles;
            return View(model);
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create(UsuariosViewModel model)
        {
            try
            {
                var result = _usersData.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UsuariosViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model.IdRol = 2;
                var result = _usersData.Create(model);
                return RedirectToAction(nameof(LogIn));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: UserController/Edit/5
        //[Authorize]
        [Authorize(Roles = "Administrador")]

        public async Task<ActionResult> Edit(int id)
        {
            var user = await _usersData.GetItem(id);

            var roles = await _usersData.GetRoles();
            var listRoles = roles.Select(x => new SelectListItem
            {
                Value = x.IdRol.ToString(),
                Text = x.Nombre
            }).ToList();

            user.Roles = listRoles;

            return View(user);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Edit(int id, UsuariosViewModel model)
        {
            try
            {
                id = id > 0 ? id : model.Id;
                var user = await _usersData.GetItem(id);
                if (user == null) { return RedirectToAction(nameof(Index)); }

                var result = _usersData.Update(model, id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var result = _usersData.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
