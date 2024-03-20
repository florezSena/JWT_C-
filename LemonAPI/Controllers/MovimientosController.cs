using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimientosController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly dblemonContext DBContext;

        public MovimientosController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetMovement")]
        public async Task<ActionResult<List<Movimiento>>> Get()
        {
            var List = await DBContext.Movimientos.Select(
                s => new Movimiento
                {
                    IdMovimiento = s.IdMovimiento,
                    IdTipoMovimiento = s.IdTipoMovimiento,
                    Fecha = s.Fecha,
                    IdProducto = s.IdProducto,
                    Cantidad = s.Cantidad
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

        [HttpGet("GetMovementById")]
        public async Task<ActionResult<Movimiento>> GetMovementById(int Id)
        {
            Movimiento? User = await DBContext.Movimientos.Select(
                    s => new Movimiento
                    {
                        IdMovimiento = s.IdMovimiento,
                        IdTipoMovimiento = s.IdTipoMovimiento,
                        Fecha = s.Fecha,
                        IdProducto = s.IdProducto,
                        Cantidad = s.Cantidad
                    })
                .FirstOrDefaultAsync(s => s.IdMovimiento == Id);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }

        [HttpPost("InsertMovement")]
        public async Task<HttpStatusCode> Create(Movimiento Movimiento)
        {
            var entity = new Movimiento()
            {
                IdMovimiento = Movimiento.IdMovimiento,
                IdTipoMovimiento = Movimiento.IdTipoMovimiento,
                Fecha = Movimiento.Fecha,
                IdProducto = Movimiento.IdProducto,
                Cantidad = Movimiento.Cantidad
            };

            DBContext.Movimientos.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateMovement")]
        public async Task<HttpStatusCode> Update(Movimiento Movimiento)
        {
            var entity = await DBContext.Movimientos.FirstOrDefaultAsync(s => s.IdMovimiento == Movimiento.IdMovimiento);

            entity.IdMovimiento = Movimiento.IdMovimiento;
            entity.IdTipoMovimiento = Movimiento.IdTipoMovimiento;
            entity.Fecha = Movimiento.Fecha;
            entity.IdProducto = Movimiento.IdProducto;
            entity.Cantidad = Movimiento.Cantidad;


            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteMovement/{Id}")]
        public async Task<HttpStatusCode> Delete(int Id)
        {
            var entity = new Movimiento()
            {
                IdMovimiento = Id
            };
            DBContext.Movimientos.Attach(entity);
            DBContext.Movimientos.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

    }
}
