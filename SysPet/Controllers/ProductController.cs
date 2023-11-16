using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysPet.Data;
using SysPet.Exception;
using SysPet.Models;

namespace SysPet.Controllers
{
    [ServiceFilter(typeof(ManageExceptionFilter))]
    public class ProductController : Controller
    {
        private readonly ProductsData productsData;
        public ProductController()
        {
            productsData = new ProductsData();
        }

       
        // GET: ProductController
        public async Task<ActionResult> Index()
        {
            try
            {
                ViewBag.Url = "Shared/EmptyData";
                var products = await productsData.GetAll();
                return View(products);

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ProductController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var product = await productsData.GetItem(id);
                return View(product);

            }
            catch (System.Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductosViewModel model)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    model.NombreArchivo = model.Image.FileName;
                    model.TipoContenido = model.Image.ContentType;
                    model.Imagen = ms.ToArray();

                    var result = productsData.Create(model);
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: ProductController/Edit/5
        public async Task<ActionResult> Update(int id)
        {
            var product = await productsData.GetItem(id);
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, ProductosViewModel model)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    model.NombreArchivo = model.Image.FileName;
                    model.TipoContenido = model.Image.ContentType;
                    model.Imagen = ms.ToArray();

                    id = id > 0 ? id : model.IdProducto;
                    var product = await productsData.GetItem(id);
                    if (product == null) { RedirectToAction(nameof(Index)); }

                    var result = productsData.Update(model, id);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        
        public ActionResult Delete(int id)
        {
            try
            {
                if (id <= 0) { RedirectToAction(nameof(Index)); }
                var result = productsData.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
