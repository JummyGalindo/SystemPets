using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SysPet.Data;
using SysPet.Exception;
using SysPet.Models;
using SysPet.Services;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]
    [TypeFilter(typeof(AuthorizePermissionFilter), Arguments = new object[] { "Administrador" })]
    public class HistoryController : Controller
    {
        private readonly HistoriesData data;
        private readonly PetsData petsData;
        private readonly UsersData usersData;
        private readonly IUserIdProvider _userIdProvider;
        public HistoryController(IUserIdProvider userIdProvider) 
        {
            data = new HistoriesData();
            petsData = new PetsData();
            _userIdProvider = userIdProvider;
            usersData = new UsersData();
        }

        // GET: HistoryController
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Url = "Shared/EmptyData";
                var userId = _userIdProvider.GetUserId();

                var user = await usersData.GetItem(userId.Value);

                IEnumerable<HistorialesViewModel> result;

                if (user.Rol.Equals("Administrador"))
                {
                    result = await data.GetAll();
                }
                else
                {
                    result = await data.GetAll(userId);
                }

                return View(result);

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: HistoryController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                return View(await data.GetItem(id));

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: HistoryController/Create
        public async Task<ActionResult> Create()
        {
            var model = new HistorialesViewModel();
            var pacientes = await petsData.GetAll();
            var list = pacientes.Select(x => new SelectListItem
            {
                Value = x.IdPaciente.ToString(),
                Text = x.Nombre
            }).ToList();

            model.Pacientes = list;
            return View(model);
        }

        // POST: HistoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HistorialesViewModel model)
        {
            try
            {
                model.UserId = _userIdProvider.GetUserId();
                var result = data.Create(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: HistoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await data.GetItem(id));
        }

        // POST: HistoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HistorialesViewModel model)
        {
            try
            {
                var item = data.GetItem(id);
                if (item == null) { RedirectToAction(nameof(Index)); }

                var result = data.Update(model, id);

                return RedirectToAction(nameof(Index));
            }
            catch 
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [TypeFilter(typeof(RoleAuthorizationFilter), Arguments = new object[] { "Administrador" })]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) { RedirectToAction(nameof(Index)); }
                var result = data.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
