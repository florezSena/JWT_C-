using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}


        private readonly dblemonContext DBContext;

        public RolesController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetRoles")]
        public async Task<ActionResult<List<Rol>>> Get()
        {
            var List = await DBContext.Rols.Select(
                s => new Rol
                {
                    IdRol = s.IdRol,
                    Nombre = s.Nombre,
                    Estado = s.Estado,
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

        [HttpGet("GetRolesById")]
        public async Task<ActionResult<Rol>> GetRolesById(int Id)
        {
            Rol? Rol = await DBContext.Rols.Select(
                    s => new Rol
                    {
                        IdRol = s.IdRol,
                        Nombre = s.Nombre,
                        Estado = s.Estado
                    })
                .FirstOrDefaultAsync(s => s.IdRol == Id);

            if (Rol == null)
            {
                return NotFound();
            }
            else
            {
                return Rol;
            }
        }

        [HttpPost("InsertRoles")]
        public async Task<HttpStatusCode> Create(Rol rol)
        {
            var entity = new Rol()
            {
                IdRol = rol.IdRol,
                Nombre = rol.Nombre,
                Estado = rol.Estado
            };

            DBContext.Rols.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateRoles")]
        public async Task<HttpStatusCode> Update(Rol rol)
        {
            var entity = await DBContext.Rols.FirstOrDefaultAsync(s => s.IdRol == rol.IdRol);

            entity.IdRol = rol.IdRol;
            entity.Nombre = rol.Nombre;
            entity.Estado = rol.Estado;


            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteRol/{Id}")]
        public async Task<HttpStatusCode> Delete(int Id)
        {
            var entity = new Rol()
            {
                IdRol = Id
            };
            DBContext.Rols.Attach(entity);
            DBContext.Rols.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
