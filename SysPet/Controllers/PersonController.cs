using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysPet.Data;
using SysPet.Exception;
using SysPet.Extensions;
using SysPet.Models;
using SysPet.Services;
using System.Reflection;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]
    public class PersonController : Controller
    {
        private readonly PersonsData data;
        private readonly CostumerDetailData costumerDetailData;
        private readonly UsersData usersData;
        private readonly IUserIdProvider _userIdProvider;
        public PersonController(IUserIdProvider userIdProvider)
        {
            data = new PersonsData();
            costumerDetailData = new CostumerDetailData();
            _userIdProvider = userIdProvider;
            usersData = new UsersData();

        }

        // GET: PersonController
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Url = "Shared/EmptyData";
                var userId = _userIdProvider.GetUserId();

                var user = await usersData.GetItem(userId.Value);

                
                IEnumerable<PersonasViewModel> result;

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

        // GET: PersonController/Details/5
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

        public async Task<ActionResult> CostumerDetail(int id)
        {
            try
            {
                var result = await costumerDetailData.GetCostumerDetail(id);

                var expiredDate = DateTime.Now.AddHours(-2);
                var dateToExpired = DateTime.Now.AddDays(2);

                var expiredDates = result.Citas.Where(x => x.FechaCita <= expiredDate && x.FechaCita >= DateTime.Now.AddDays(-1));
                var dateToExpiredList = result.Citas.Where(x => x.FechaCita >= dateToExpired);
                if (expiredDates.Any())
                {
                    foreach (var item in expiredDates)
                    {
                        TempData.AddToastrMessage($"La cita expiró!, Fecha: {item.FechaCita}", $"Cita No. {item.Id}", ToastrMessageType.Error);
                    }
                }

                if (dateToExpiredList.Any())
                {
                    foreach (var item in expiredDates)
                    {
                        TempData.AddToastrMessage($"La cita expira pronto!, Fecha: {item.FechaCita}", $"Cita No. {item.Id}", ToastrMessageType.Info);
                    }
                }

                return View(result);
            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: PersonController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PersonController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonasViewModel model)
        {
            try
            {
                model.IdTipoPersona = 2;
                model.UserId = _userIdProvider.GetUserId();
                var result = data.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: PersonController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await data.GetItem(id));
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PersonasViewModel model)
        {
            try
            {
                id = id > 0 ? id : model.IdPersona;
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
