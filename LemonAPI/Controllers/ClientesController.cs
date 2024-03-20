using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        private readonly dblemonContext DBContext;

        public ClientesController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }
        /*Obtenemos la lista de clientes*/
        [HttpGet("GetClients")]
        public async Task<ActionResult<List<Cliente>>> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var rToken = Jwt.validarToken(identity,DBContext);

            if (!rToken.Result.success) return StatusCode(403,new {succes=false,message="Su token es incorrecto",result="" });

            Usuario usuario = rToken.Result.result;

            if (usuario.IdRol != 1)
            {
                return StatusCode(403, new
                {
                    success = false,
                    message = "No tienes permisos para realizar esta operacion",
                    result = ""
                });
            }

            var List = await DBContext.Clientes.Select(
                s => new Cliente
                {
                    IdCliente = s.IdCliente,
                    TipoDocumento = s.TipoDocumento,
                    Documento = s.Documento,
                    NombreRazonSocial = s.NombreRazonSocial,
                    Correo = s.Correo,
                    Telefono = s.Telefono,
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
        /*Traer un cliente por id*/
        [HttpGet("GetClientById")]
        public async Task<ActionResult<Cliente>> GetClientById(int Id)
        {
            /*este signo ? es para que no salga la advertencia que se esta guardando un nulo en un campo que no acepta nulos*/
            Cliente? cliente = await DBContext.Clientes.Select(
                    s => new Cliente
                    {
                        IdCliente = s.IdCliente,
                        TipoDocumento = s.TipoDocumento,
                        Documento = s.Documento,
                        NombreRazonSocial = s.NombreRazonSocial,
                        Correo = s.Correo,
                        Telefono = s.Telefono,
                        Estado = s.Estado
                    })
                .FirstOrDefaultAsync(s => s.IdCliente == Id);

            if (cliente == null)
            {
                return NotFound();
            }
            else
            {
                return cliente;
            }
        }
        /*Insertamos un cliente*/
        [HttpPost("InsertClient")]
        public async Task<HttpStatusCode> InsertClient(Cliente cliente)
        {
            var entity = new Cliente()
            {
                TipoDocumento = cliente.TipoDocumento,
                Documento = cliente.Documento,
                NombreRazonSocial = cliente.NombreRazonSocial,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                Estado = cliente.Estado
            };

            DBContext.Clientes.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }
        /*Actualizar un cliente*/
        [HttpPut("UpdateClient")]
        public async Task<HttpStatusCode> UpdateClient(Cliente cliente)
        {
            var entity = await DBContext.Clientes.FirstOrDefaultAsync(s => s.IdCliente == cliente.IdCliente);

            if (entity == null)
            {
                throw new Exception("El objeto response es nulo");
            }

            entity.TipoDocumento = cliente.TipoDocumento;
            entity.Documento = cliente.Documento;
            entity.NombreRazonSocial = cliente.NombreRazonSocial;
            entity.Correo = cliente.Correo;
            entity.Telefono = cliente.Telefono;
            entity.Estado = cliente.Estado;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
        /*Eliminar un cliente*/
        [HttpDelete("DeleteClient")]
        public async Task<HttpStatusCode> DeleteClient(int Id)
        {
            var entity = new Cliente()
            {
                IdCliente = Id
            };
            DBContext.Clientes.Attach(entity);
            DBContext.Clientes.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

    }
}
