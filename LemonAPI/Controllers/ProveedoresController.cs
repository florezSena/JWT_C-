using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LemonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}


        private readonly dblemonContext DBContext;

        public ProveedoresController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetProveedores")]
        public async Task<ActionResult<List<Proveedor>>> Get()
        {
            var List = await DBContext.Proveedors.Select(
                s => new Proveedor
                {
                    IdProveedor = s.IdProveedor,
                    TipoDocumento = s.TipoDocumento,
                    Documento = s.Documento,
                    NombreRazonSocial = s.NombreRazonSocial,
                    Correo = s.Correo,
                    Telefono = s.Telefono,
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

        [HttpGet("GetProveedoresById")]
        public async Task<ActionResult<Proveedor>> GetUserById(int Id)
        {
            Proveedor User = await DBContext.Proveedors.Select(
                    s => new Proveedor
                    {
                        IdProveedor = s.IdProveedor,
                        TipoDocumento = s.TipoDocumento,
                        Documento = s.Documento,
                        NombreRazonSocial = s.NombreRazonSocial,
                        Correo = s.Correo,
                        Telefono = s.Telefono,
                        Estado = s.Estado
                    })
                .FirstOrDefaultAsync(s => s.IdProveedor == Id);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }

        [HttpPost("InsertProveedores")]
        public async Task<HttpStatusCode> Create(Proveedor Proveedor)
        {
            var entity = new Proveedor()
            {
                IdProveedor = Proveedor.IdProveedor,
                TipoDocumento = Proveedor.TipoDocumento,
                Documento = Proveedor.Documento,
                NombreRazonSocial = Proveedor.NombreRazonSocial,
                Correo = Proveedor.Correo,
                Telefono = Proveedor.Telefono,
                Estado = Proveedor.Estado
            };

            DBContext.Proveedors.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateProveedores")]
        public async Task<HttpStatusCode> Update(Proveedor Proveedor)
        {
            var entity = await DBContext.Proveedors.FirstOrDefaultAsync(s => s.IdProveedor == Proveedor.IdProveedor);

            entity.TipoDocumento = Proveedor.TipoDocumento;
            entity.NombreRazonSocial = Proveedor.NombreRazonSocial;
            entity.Documento = Proveedor.Documento;
            entity.Correo = Proveedor.Correo;
            entity.Telefono = Proveedor.Telefono;
            entity.Estado = Proveedor.Estado;


            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteProveedor/{Id}")]
        public async Task<HttpStatusCode> Delete(int Id)
        {
            var entity = new Proveedor()
            {
                IdProveedor = Id
            };
            DBContext.Proveedors.Attach(entity);
            DBContext.Proveedors.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
