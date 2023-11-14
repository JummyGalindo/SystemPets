using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SysPet.Data;
using SysPet.Models;

using DinkToPdf;
using DinkToPdf.Contracts;
using SysPet.Exception;
using Rotativa.AspNetCore;
using SysPet.Services;
using System.Reflection;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]
    [TypeFilter(typeof(AuthorizePermissionFilter), Arguments = new object[] { "Administrador" })]
    public class InternmentController : Controller
    {
        private readonly InternmentsData data;
        private readonly PersonsData personsData;
        private readonly PetsData petsData;
        private readonly UsersData usersData;
        private readonly IConverter converter;
        private readonly IPdfService<InternamientosViewModel> _pdfService;
        private readonly IUserIdProvider _userIdProvider;
        public InternmentController(IConverter converter, IPdfService<InternamientosViewModel> pdfService, IUserIdProvider userIdProvider)
        {
            data = new InternmentsData();
            personsData = new PersonsData();
            petsData = new PetsData();
            this.converter = converter;
            _pdfService = pdfService;
            _userIdProvider = userIdProvider;
            usersData = new UsersData();

        }

        public IActionResult ShowPdf()
        {
            string currentPage = HttpContext.Request.Path;
            string pageUrl = HttpContext.Request.GetEncodedUrl();

            pageUrl = pageUrl.Replace(currentPage, "");
            pageUrl = $"{pageUrl}/Internment/Details";

            var pdfSettings = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = { new ObjectSettings
                {
                    Page = pageUrl
                } }
            };

            var pdf = converter.Convert(pdfSettings);

            return File(pdf,"application/pdf");
        }

        public async Task<IActionResult> DownloadPdf(InternamientosViewModel model)
        {
            try
            {
                var item = await data.GetItem(model.Id);
                var internment = new InternmentViewModel
                {
                    Id = model.Id.ToString(),
                    Antecedentes = item.Antecedentes,
                    Atendio = item.Atendio,
                    Fecha = item.FechaIngreso.ToString("f"),
                    Medicamento = item.Medicamento,
                    Paciente = item.Paciente,
                    Propietario = item.Propietario,
                    Tratamiento = item.Tratamiento,
                    Imagen = item.Imagen,
                    TipoContenido = item.TipoContenido,

                };

                string pdfName = $"Internamiento_{model.Id}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.pdf";

                return new ViewAsPdf("DownloadPdf", internment)
                {
                    FileName = $"Internamiento{model?.Id}.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4
                };

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> DownloadIndexPdf()
        {
            try
            {
                var items = await data.GetAll();

                var result = items.Select(x => new InternmentViewModel
                {
                    Id = x.Id.ToString(),
                    Antecedentes = x.Antecedentes,
                    Atendio = x.Atendio,
                    Fecha = x.FechaIngreso.ToString("dd/mm/yyyy"),
                    Medicamento = x.Medicamento,
                    Paciente = x.Paciente,
                    Propietario = x.Propietario,
                    Tratamiento = x.Tratamiento,

                }).ToList();

                string pdfName = $"Internamientos_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.pdf";


                return new ViewAsPdf("DownloadIndexPdf", result)
                {
                    FileName = pdfName,
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4
                };
            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        // GET: InternmentController
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Url = "Shared/EmptyData";
                var userId = _userIdProvider.GetUserId();
                var user = await usersData.GetItem(userId.Value);

                IEnumerable<InternamientosViewModel> result;

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

        public async Task<ActionResult> PDF(int id)
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

        // GET: InternmentController/Details/5
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

        // GET: InternmentController/Create
        public async Task<ActionResult> Create()
        {
            try

            {

                var model = new InternamientosViewModel();
                var persons = await personsData.GetAll(2);
                var doctors = await personsData.GetAll(3);
                var patients = await petsData.GetAll();

                var personList = persons.Select(x => new SelectListItem
                {
                    Value = x.IdPersona.ToString(),
                    Text = $"{x.Nombre} {x.ApellidoPaterno} {x.ApellidoMaterno}"
                }).ToList();

                var doctorList = doctors.Select(x => new SelectListItem
                {
                    Value = x.IdPersona.ToString(),
                    Text = $"{x.Nombre} {x.ApellidoPaterno} {x.ApellidoMaterno}"
                }).ToList();

                var patientList = patients.Select(x => new SelectListItem
                {
                    Value = x.IdPaciente.ToString(),
                    Text = $"{x.Nombre}"
                }).ToList();

                model.Personas = personList;
                model.Pacientes = patientList;
                model.Doctores = doctorList;
                return View(model);
            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: InternmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InternamientosViewModel model)
        {
            try
            {
                var internment = await data.GetPersonId(model.IdPaciente);
                model.IdPersona = internment.IdPersona;
                model.UserId = _userIdProvider.GetUserId();
                var resut = data.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: InternmentController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await data.GetItem(id));
        }

        // POST: InternmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InternamientosViewModel model)
        {
            try
            {
                id = id > 0 ? id : model.Id;
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
