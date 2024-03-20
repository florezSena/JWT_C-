using Lemon.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lemon.Controllers
{
    public class AccesoController : Controller
    {
        public readonly IApiClient _client;
        public AccesoController(IApiClient client)
        {
            _client = client;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Ingresar(string usuario,string password)
        {
            if(usuario == null || password == null) return RedirectToAction("Index", "Acceso");

            Console.WriteLine("El usuario es"+usuario);
            Console.WriteLine("La contraseña es" + password);

            var response = await _client.Login(usuario,password);

            if(response.success!=false) {
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Index", "Ventums");


        }
    }
}
