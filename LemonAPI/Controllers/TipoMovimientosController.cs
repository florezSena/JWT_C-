using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoMovimientosController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly dblemonContext DBContext;

        public TipoMovimientosController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetType")]
        public async Task<ActionResult<List<Tipomovimiento>>> Get()
        {
            var List = await DBContext.Tipomovimientos.Select(
                s => new Tipomovimiento
                {
                    IdTipoMovimiento = s.IdTipoMovimiento,
                    Nombre = s.Nombre,
                    Estado = s.Estado
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

        [HttpGet("GetTypeById")]
        public async Task<ActionResult<Tipomovimiento>> GetTypeById(int Id)
        {
            Tipomovimiento User = await DBContext.Tipomovimientos.Select(
                    s => new Tipomovimiento
                    {
                        IdTipoMovimiento = s.IdTipoMovimiento,
                        Nombre = s.Nombre,
                        Estado = s.Estado
                    })
                .FirstOrDefaultAsync(s => s.IdTipoMovimiento == Id);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }

        [HttpPost("InsertType")]
        public async Task<HttpStatusCode> Create(Tipomovimiento Tipomovimiento)
        {
            var entity = new Tipomovimiento()
            {
                IdTipoMovimiento = Tipomovimiento.IdTipoMovimiento,
                Nombre = Tipomovimiento.Nombre,
                Estado = Tipomovimiento.Estado
            };

            DBContext.Tipomovimientos.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateType")]
        public async Task<HttpStatusCode> Update(Tipomovimiento Tipomovimiento)
        {
            var entity = await DBContext.Tipomovimientos.FirstOrDefaultAsync(s => s.IdTipoMovimiento == Tipomovimiento.IdTipoMovimiento);

            entity.IdTipoMovimiento = Tipomovimiento.IdTipoMovimiento;
            entity.Nombre = Tipomovimiento.Nombre;
            entity.Estado = Tipomovimiento.Estado;


            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteType/{Id}")]
        public async Task<HttpStatusCode> Delete(int Id)
        {
            var entity = new Tipomovimiento()
            {
                IdTipoMovimiento = Id
            };
            DBContext.Tipomovimientos.Attach(entity);
            DBContext.Tipomovimientos.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

    }
}
