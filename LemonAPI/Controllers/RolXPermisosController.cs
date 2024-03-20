using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolXPermisosController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly dblemonContext DBContext;

        public RolXPermisosController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetRolXPermisos")]
        public async Task<ActionResult<List<Rolpermiso>>> Get()
        {
            var List = await DBContext.Rolpermisos.Select(
                s => new Rolpermiso
                {
                    IdRolPermiso = s.IdRolPermiso,
                    IdRol = s.IdRol,
                    IdPermiso = s.IdPermiso,
                    IdRolNavigation = s.IdRolNavigation,
                    IdPermisoNavigation = s.IdPermisoNavigation
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

        [HttpGet("GetRolXPermisoById")]
        public async Task<ActionResult<List<Rolpermiso>>> GetPermisoById(int Id)
        {
            var List = await DBContext.Rolpermisos
            .Where(s => s.IdRol == Id)
            .Select(s => new Rolpermiso
            {
                IdRolPermiso = s.IdRolPermiso,
                IdRol = s.IdRol,
                IdPermiso = s.IdPermiso,
                IdRolNavigation = s.IdRolNavigation,
                IdPermisoNavigation = s.IdPermisoNavigation
            })
            .ToListAsync();

            if (List == null)
            {
                return NotFound();
            }
            else
            {
                return List;
            }

        }

        [HttpPost("InsertRolXPermiso")]
        public async Task<HttpStatusCode> Create(Rolpermiso rolpermiso)
        {
            var entity = new Rolpermiso()
            {
                IdRolPermiso = rolpermiso.IdRolPermiso,
                IdRol = rolpermiso.IdRol,
                IdPermiso = rolpermiso.IdPermiso
            };

            DBContext.Rolpermisos.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateRolXPermisos")]
        public async Task<HttpStatusCode> Update(Rolpermiso rolpermiso)
        {
            var entity = await DBContext.Rolpermisos.FirstOrDefaultAsync(s => s.IdRolPermiso == rolpermiso.IdRolPermiso);

            entity.IdRolPermiso = rolpermiso.IdRolPermiso;
            entity.IdRol = rolpermiso.IdRol;
            entity.IdPermiso = rolpermiso.IdPermiso;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteRolXPermiso/{Id}")]
        public async Task<HttpStatusCode> Delete(int Id)
        {
            var entity = new Rolpermiso()
            {
                IdRolPermiso = Id
            };
            DBContext.Rolpermisos.Attach(entity);
            DBContext.Rolpermisos.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
