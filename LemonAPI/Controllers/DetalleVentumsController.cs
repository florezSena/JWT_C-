using LemonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentumsController : ControllerBase
    {
        private readonly dblemonContext DBContext;

        public DetalleVentumsController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }
        /*Obtenemos la lista de ventas*/
        [HttpGet("GetDetallesVenta")]
        public async Task<ActionResult<List<Detalleventum>>> Get(int Id)
        {
            var List = await DBContext.Detalleventa
            .Where(s => s.IdVenta == Id)
            .Select(s => new Detalleventum
            {
                IdDetalleVenta = s.IdDetalleVenta,
                IdVenta = s.IdVenta,
                IdProducto = s.IdProducto,
                Cantidad = s.Cantidad,
                PrecioKilo = s.PrecioKilo,
                Subtotal = s.Subtotal,
                IdProductoNavigation=s.IdProductoNavigation,
                IdVentaNavigation=s.IdVentaNavigation
            })
            .ToListAsync();

            if (List.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }
        /*Insertamos un cliente*/
        [HttpPost("InsertDetalleVenta")]
        public async Task<HttpStatusCode> InsertDetalleVenta(Detalleventum detalleventa)
        {
            var entity = new Detalleventum()
            {
                IdVenta = detalleventa.IdVenta,
                IdProducto = detalleventa.IdProducto,
                Cantidad = detalleventa.Cantidad,
                PrecioKilo = detalleventa.PrecioKilo,
                Subtotal = detalleventa.Subtotal
            };

            DBContext.Detalleventa.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }
    }
}
