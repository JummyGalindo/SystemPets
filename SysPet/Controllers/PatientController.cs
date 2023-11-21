using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SysPet.Data;
using SysPet.Exception;
using SysPet.Models;
using SysPet.Services;
using System.Reflection;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]
    public class PatientController : Controller
    {
        private readonly PetsData data;
        private readonly PersonsData personData;
        private readonly UsersData usersData;
        private readonly IUserIdProvider _userIdProvider;
        public PatientController(IUserIdProvider userIdProvider)
        {
            data = new PetsData();
            personData = new PersonsData();
            _userIdProvider = userIdProvider;
            usersData = new UsersData();    
        }

        // GET: PatientController
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Url = "Shared/EmptyData";
                var userId = _userIdProvider.GetUserId();

                var user = await usersData.GetItem(userId.Value);

                IEnumerable<MascotasViewModel> result;

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

        // GET: PatientController/Details/5
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

        // GET: PatientController/Create
        public async Task<ActionResult> Create()
        {
            var model = new MascotasViewModel();
            
            var persons = await personData.GetAll();
            
            var lista = persons.Select(x => new SelectListItem
            {
                Value = x.IdPersona.ToString(),
                Text = $"{x.Nombre} {x.ApellidoPaterno} {x.ApellidoMaterno}"
            });

            model.Personas = persons.ToList();
            model.ListaPersonas = lista.ToList();
            return View(model);
        }

        // POST: PatientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MascotasViewModel model, IFormFile file)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    model.NombreArchivo = model.Image.FileName;
                    model.TipoContenido = model.Image.ContentType;
                    model.Imagen = ms.ToArray();
                    model.UserId = _userIdProvider.GetUserId();

                    var result = data.Create(model);
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: PatientController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await data.GetItem(id));
        }

        // POST: PatientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MascotasViewModel model)
        {
            try
            {
                id = id > 0 ? id : model.IdPaciente;
                var item = data.GetItem(id);
                if (item == null) { RedirectToAction(nameof(Index)); }

                if (model?.Image != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        model.Image.CopyTo(ms);
                        model.NombreArchivo = model.Image.FileName;
                        model.TipoContenido = model.Image.ContentType;
                        model.Imagen = ms.ToArray();

                        var res = data.Update(model, id);
                        return RedirectToAction(nameof(Index));
                    }
                }

                var result = data.Update(model, id);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [Authorize(Roles = "Administrador")]
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
