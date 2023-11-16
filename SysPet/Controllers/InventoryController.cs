using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SysPet.Data;
using SysPet.Exception;
using SysPet.Models;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]
    public class InventoryController : Controller
    {
        private readonly InventoriesData data;
        private readonly ProductsData productData;
        public InventoryController()
        {
            data = new InventoriesData();
            productData = new ProductsData();
        }

        // GET: InventoryController
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Url = "Shared/EmptyData";
                return View(await data.GetAll());

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: InventoryController/Details/5
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

        // GET: InventoryController/Create
        public async Task<ActionResult> Create()
        {
            var model = new InventariosViewModel();

            var products = await productData.GetAll();
            var productList = products.Select(x => new SelectListItem
            {
                Value = x.IdProducto.ToString(),
                Text = x.Nombre
            }).ToList();

            model.Productos = productList;

            return View(model);
        }

        // POST: InventoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventariosViewModel model)
        {
            try
            {
                var result = data.Create(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: InventoryController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await data.GetItem(id));
        }

        // POST: InventoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InventariosViewModel model)
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
