using Lemon.Models;
using Lemon.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using X.PagedList;

namespace Lemon.Controllers
{
    public class RolesController : Controller
    {
        private static readonly List<int> permisos;
        static RolesController()
        {
            permisos = new List<int>();
        }
        private readonly IApiClient _client;

        public RolesController(IApiClient client/*, IWebHostEnvironment hostingEnvironment*/)
        {
            _client = client;
            //_hostEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Número de elementos por página
            int pageNumber = page ?? 1;

            var rol = await _client.GetRolesAsync();

            var pagedRoles = await rol.ToPagedListAsync(pageNumber, pageSize);

            ViewData["Roles"] = rol;
            return View(pagedRoles);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AlmacenarPermisos(int id)
        {
            permisos.Add(id);
            Console.WriteLine("\nAgregados:");
            foreach (int pr in permisos)
            {
                Console.WriteLine(pr);
            }
            return Json(new { success = true, message = "Permiso guardado exitosamente" });
        }
        [HttpGet]
        public IActionResult EliminarPermiso(int id)
        {
            permisos.RemoveAll(permiso => permiso == id);

            Console.WriteLine("\nDespues:");
            foreach (int pr in permisos)
            {
                Console.WriteLine(pr);
            }

            return Json(new { success = true, message = "Permiso eliminado exitosamente" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Rol rol)
        {
            if (ModelState.IsValid)
            {
                // 1. Crear el rol
                var response = await _client.CreateRolesAsync(rol);
                if (!response.IsSuccessStatusCode)
                {
                    // Manejar errores de API
                    return View(rol);
                }

                var roles = await _client.GetRolesAsync();
                var ultimoIdRol = roles.Max(r => r.IdRol);
                foreach (int permiso in permisos)
                {
                    Rolpermiso nuevoRolPermiso = new Rolpermiso
                    {
                        IdPermiso = permiso,
                        IdRol = ultimoIdRol
                    };

                    await _client.CreateRolXPermisoAsync(nuevoRolPermiso);

                }
                return RedirectToAction(nameof(Index));
            }

            return View(rol);
        }

        //este no lo tome en cuenta
        public IActionResult CreateRP()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRP(Rolpermiso rolper)
        {
            if (ModelState.IsValid)
            {

                var response = await _client.CreateRolXPermisoAsync(rolper);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error en la creación del rol");
                    return View();
                }

            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var rol = await _client.FindRolesAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            else if (rol.Estado == 0)
            {
                //No se puede hacer esta accion con los clientes inhabilitados
                return Json(new { success = false, message = "rol inhabilitado no se puede eliminar" });
            }

            var permisos = await _client.GetRolXPermisoAsync();

            var rolwithpermisos = permisos.Any(p => p.IdRol == rol.IdRol);
            if (rolwithpermisos)
            {
                return Json(new { success = false, message = "Rol con permisos" });
            }

            await _client.DeleteRolesAsync(rol.IdRol);

            return Json(new { success = true, message = "Rol eliminado exitosamente" });
        }

        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el proveedor con el ID especificado
            var rol = await _client.FindRolesAsync(id.Value);
            if (rol == null)
            {
                return NotFound();
            }

            // Cambiar el estado del proveedor
            rol.Estado = (rol.Estado == 1) ? 0 : 1;

            // Actualizar el proveedor utilizando el cliente de API
            var response = await _client.UpdateRolesAsync(rol);

            if (response.IsSuccessStatusCode)
            {
                // El estado del proveedor se cambió correctamente
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Si la actualización falla, manejar el error según sea necesario
                ModelState.AddModelError(string.Empty, "Error al cambiar el estado del proveedor");
                return View();
            }
        }

    }
}
