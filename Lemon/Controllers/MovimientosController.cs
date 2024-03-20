using Lemon.Models;
using Lemon.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lemon.Controllers
{
    public class MovimientosController : Controller
    {
        public readonly IApiClient _client;

        public MovimientosController(IApiClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var movimientos = await _client.GetMovementAsync();
            var tipoMovimientos = await _client.GetTypeAsync();
            ViewData["Tipos"] = tipoMovimientos;
            var productos = await _client.GetProductAsync();
            ViewData["Prods"] = productos;
            return View(movimientos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.CreateMovementAsync(movimiento);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa

                    // Restar la cantidad movida del producto disponible
                    var producto = await _client.FindProductAsync(movimiento.IdProducto);
                    if (producto != null)
                    {
                        producto.Cantidad -= movimiento.Cantidad;

                        // Actualizar la cantidad de productos a través de la API
                        var updateResponse = await _client.UpdateProductAsync(producto);

                        if (!updateResponse.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError(string.Empty, "Error al actualizar la cantidad del producto");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No se encontró el producto para actualizar la cantidad");
                        return View();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la creacion del movimiento");
                    return View();
                }
            }
            return View();
        }


        public async Task<IActionResult> Update(int id)
        {
            var artic = await _client.FindMovementAsync(id);
            return View(artic);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Movimiento movimiento)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.UpdateMovementAsync(movimiento);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la acualización del movimiento");
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Obtener la información del movimiento que se va a eliminar
            var movimientoEliminado = await _client.FindMovementAsync(id);

            // Si el movimiento no existe, devolver NotFound
            if (movimientoEliminado == null)
            {
                return NotFound();
            }

            // Eliminar el movimiento
            await _client.DeleteMovementAsync(id);

            // Actualizar el inventario sumando la cantidad eliminada
            var producto = await _client.FindProductAsync(movimientoEliminado.IdProducto);
            if (producto != null)
            {
                producto.Cantidad += movimientoEliminado.Cantidad;

                // Actualizar la cantidad de productos a través de la API
                var updateResponse = await _client.UpdateProductAsync(producto);

                if (!updateResponse.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar la cantidad del producto");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No se encontró el producto para actualizar la cantidad");
                return View();
            }

            // Redireccionar a la página de índice
            return RedirectToAction("Index");
        }
    }
}
