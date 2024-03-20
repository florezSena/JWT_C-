using Lemon.Models;
using Lemon.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lemon.Controllers
{
    public class ProductosController : Controller
    {
        public readonly IApiClient _client;

        public ProductosController(IApiClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _client.GetProductAsync();
            return View(productos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.CreateProductAsync(producto);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la creacion del producto");
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            var artic = await _client.FindProductAsync(id);
            return View(artic);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.UpdateProductAsync(producto);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la actualización del producto");
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var artic = await _client.DeleteProductAsync(id);
            if (artic == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index");
        }
    }
}
