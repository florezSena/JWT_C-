using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisosController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}


        private readonly dblemonContext DBContext;

        public PermisosController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetPermisos")]
        public async Task<ActionResult<List<Permiso>>> Get()
        {
            var List = await DBContext.Permisos.Select(
                s => new Permiso
                {
                    IdPermiso = s.IdPermiso,
                    Permiso1 = s.Permiso1,
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

        [HttpGet("GetPermisoById")]
        public async Task<ActionResult<Permiso>> GetPermisoById(int Id)
        {
            Permiso? Permiso = await DBContext.Permisos.Select(
                    s => new Permiso
                    {
                        IdPermiso = s.IdPermiso,
                        Permiso1 = s.Permiso1,
                    })
                .FirstOrDefaultAsync(s => s.IdPermiso == Id);

            if (Permiso == null)
            {
                return NotFound();
            }
            else
            {
                return Permiso;
            }
        }

        [HttpPost("InsertPermisos")]
        public async Task<HttpStatusCode> Create(Permiso permiso)
        {
            var entity = new Permiso()
            {
                IdPermiso = permiso.IdPermiso,
                Permiso1 = permiso.Permiso1,
            };

            DBContext.Permisos.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdatePermisos")]
        public async Task<HttpStatusCode> Update(Permiso permiso)
        {
            var entity = await DBContext.Permisos.FirstOrDefaultAsync(s => s.IdPermiso == permiso.IdPermiso);

            entity.IdPermiso = permiso.IdPermiso;
            entity.Permiso1 = permiso.Permiso1;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeletePermiso/{Id}")]
        public async Task<HttpStatusCode> Delete(int Id)
        {
            var entity = new Permiso()
            {
                IdPermiso = Id
            };
            DBContext.Permisos.Attach(entity);
            DBContext.Permisos.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}

