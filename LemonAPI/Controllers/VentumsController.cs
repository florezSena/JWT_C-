using LemonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentumsController : ControllerBase
    {
        private readonly dblemonContext DBContext;

        public VentumsController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }
        /*Obtenemos la lista de ventas*/
        [HttpGet("GetVentas")]
        public async Task<ActionResult<List<Ventum>>> Get()
        {
            var List = await DBContext.Venta.Select(
                s => new Ventum
                {
                    IdVenta = s.IdVenta,
                    IdCliente = s.IdCliente,
                    Fecha = s.Fecha,
                    Total = s.Total,
                    Estado = s.Estado,
                    IdClienteNavigation = s.IdClienteNavigation
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }
        /*Traer un cliente por id*/
        [HttpGet("GetVentaById")]
        public async Task<ActionResult<Ventum>> GetVentaById(int Id)
        {
            /*este signo ? es para que no salga la advertencia que se esta guardando un nulo en un campo que no acepta nulos*/
            Ventum? venta = await DBContext.Venta.Select(
                    s => new Ventum
                    {
                        IdVenta = s.IdVenta,
                        IdCliente = s.IdCliente,
                        Fecha = s.Fecha,
                        Total = s.Total,
                        Estado = s.Estado,
                        IdClienteNavigation= s.IdClienteNavigation
                    })
                .FirstOrDefaultAsync(s => s.IdVenta == Id);

            if (venta == null)
            {
                return NotFound();
            }
            else
            {
                return venta;
            }
        }
        /*Insertamos un cliente*/
        [HttpPost("InsertVenta")]
        public async Task<HttpStatusCode> InsertVenta(Ventum venta)
        {
            var entity = new Ventum()
            {
                IdVenta = venta.IdVenta,
                IdCliente = venta.IdCliente,
                Fecha = venta.Fecha,
                Total = venta.Total,
                Estado = venta.Estado
            };

            DBContext.Venta.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }
        /*Actualizar el estado de la venta*/
        [HttpPut("UpdateVenta")]
        public async Task<HttpStatusCode> UpdateClient(Ventum venta)
        {
            var entity = await DBContext.Venta.FirstOrDefaultAsync(s => s.IdVenta == venta.IdVenta);

            if (entity == null)
            {
                throw new Exception("El objeto response es nulo");
            }

            entity.Estado = venta.Estado;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
