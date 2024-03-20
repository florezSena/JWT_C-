using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleComprasController : ControllerBase
    {
        private readonly dblemonContext DBContext;

        public DetalleComprasController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }
        /*Obtenemos la lista de Compras*/
        [HttpGet("GetDetallesCompra")]
        public async Task<ActionResult<List<Detallecompra>>> Get(int Id)
        {
            var List = await DBContext.Detallecompras
            .Where(s => s.IdCompra == Id)
            .Select(s => new Detallecompra
            {
                IdDetalleCompra = s.IdDetalleCompra,
                IdCompra = s.IdCompra,
                IdProducto = s.IdProducto,
                Cantidad = s.Cantidad,
                PrecioKilo = s.PrecioKilo,
                Subtotal = s.Subtotal,
                IdProductoNavigation = s.IdProductoNavigation,
                IdCompraNavigation = s.IdCompraNavigation
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
        /*Insertamos un detalle de compra*/
        [HttpPost("InsertDetalleCompra")]
        public async Task<HttpStatusCode> InsertDetalleCompra(Detallecompra detallecompra)
        {
            var entity = new Detallecompra()
            {
                IdDetalleCompra = detallecompra.IdDetalleCompra,
                IdCompra = detallecompra.IdCompra,
                IdProducto = detallecompra.IdProducto,
                Cantidad = detallecompra.Cantidad,
                PrecioKilo = detallecompra.PrecioKilo,
                Subtotal = detallecompra.Subtotal,
            };

            DBContext.Detallecompras.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }
    }
}
