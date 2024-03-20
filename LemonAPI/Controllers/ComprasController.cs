using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly dblemonContext DBContext;

        public ComprasController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }
        /*Obtenemos la lista de compras*/
        [HttpGet("GetCompras")]
        public async Task<ActionResult<List<Compra>>> Get()
        {
            var List = await DBContext.Compras.Select(
                s => new Compra
                {
                    IdCompra = s.IdCompra,
                    IdProveedor = s.IdProveedor,
                    Fecha = s.Fecha,
                    ValorCompra = s.ValorCompra,
                    Estado = s.Estado,
                    IdProveedorNavigation = s.IdProveedorNavigation
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
        /*Traer una compra por id*/
        [HttpGet("GetCompraById")]
        public async Task<ActionResult<Compra>> GetCompraById(int Id)
        {
            /*este signo ? es para que no salga la advertencia que se esta guardando un nulo en un campo que no acepta nulos*/
            Compra? compra = await DBContext.Compras.Select(
                    s => new  Compra
                    {
                        IdCompra = s.IdCompra,
                        IdProveedor = s.IdProveedor,
                        Fecha = s.Fecha,
                        ValorCompra = s.ValorCompra,
                        Estado = s.Estado,
                        IdProveedorNavigation = s.IdProveedorNavigation
                    })
                .FirstOrDefaultAsync(s => s.IdCompra == Id);

            if (compra == null)
            {
                return NotFound();
            }
            else
            {
                return compra;
            }
        }
        /*Insertamos una compra*/
        [HttpPost("InsertCompra")]
        public async Task<HttpStatusCode> InsertCompra(Compra compra)
        {
            var entity = new Compra()
            {
                IdCompra = compra.IdCompra,
                IdProveedor = compra.IdProveedor,
                Fecha = compra.Fecha,
                ValorCompra = compra.ValorCompra,
                Estado = compra.Estado
            };

            DBContext.Compras.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }
        /*Actualizar el estado de la Compra*/
        [HttpPut("UpdateCompra")]
        public async Task<HttpStatusCode> UpdateCompra(Compra compra)
        {
            var entity = await DBContext.Compras.FirstOrDefaultAsync(s => s.IdCompra == compra.IdCompra);

            if (entity == null)
            {
                throw new Exception("El objeto response es nulo");
            }

            entity.Estado = compra.Estado;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
