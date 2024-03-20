using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly dblemonContext DBContext;
        //Modificamos el constructor para utilizar el JWT
        public UsuariosController(dblemonContext DBContext, IConfiguration configuration)
        {
            this.DBContext = DBContext;
            this._configuration = configuration;    
        }
        [HttpPost("Login")]
        public async Task<dynamic> inicioSesion([FromBody] Object optData)
        {
            var data =JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string user = data.usuario.ToString();

            string password = data.password.ToString();

            string passwordHash= Jwt.EncodePassword(password);

            Usuario? usuario = await DBContext.Usuarios.Select(
                    s => new Usuario
                    {
                        IdUsuario = s.IdUsuario,
                        Correo = s.Correo,
                        NombreUsuario = s.NombreUsuario,
                        Contraseña = s.Contraseña,
                        IdRol = s.IdRol,
                        Estado = s.Estado,
                        IdRolNavigation = s.IdRolNavigation
                    })
                .FirstOrDefaultAsync(s => s.Correo == user && s.Contraseña == passwordHash);

            if (usuario == null)
            {
                return new
                {
                    success = false,
                    mesage = "Usuario no encontrado",
                    result = ""
                };
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("idUser",usuario.IdUsuario.ToString()),
                new Claim("correo",usuario.Correo),
                new Claim("idRol",usuario.IdRol.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));

            var singIn = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: singIn
                );

            return new
            {
                success = true,
                message = "Exito",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }


        /*Obtenemos la lista de usuarios*/
        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var List = await DBContext.Usuarios.Select(
                s => new Usuario
                {
                    IdUsuario = s.IdUsuario,
                    Correo = s.Correo,
                    NombreUsuario = s.NombreUsuario,
                    Contraseña = s.Contraseña,
                    IdRol = s.IdRol,
                    Estado = s.Estado,
                    IdRolNavigation = s.IdRolNavigation
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
        [HttpGet("GetUserById")]
        public async Task<ActionResult<Usuario>> GetUserById(int Id)
        {
            /*este signo ? es para que no salga la advertencia que se esta guardando un nulo en un campo que no acepta nulos*/
            Usuario? usuario = await DBContext.Usuarios.Select(
                    s => new Usuario
                    {
                        IdUsuario = s.IdUsuario,
                        Correo = s.Correo,
                        NombreUsuario = s.NombreUsuario,
                        Contraseña = s.Contraseña,
                        IdRol = s.IdRol,
                        Estado = s.Estado,
                        IdRolNavigation = s.IdRolNavigation
                    })
                .FirstOrDefaultAsync(s => s.IdUsuario == Id);

            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return usuario;
            }
        }
        /*Insertamos un usuario*/
        [HttpPost("InsertUser")]
        public async Task<HttpStatusCode> InsertUser(Usuario usuario)
        {
            var entity = new Usuario()
            {
                Correo = usuario.Correo,
                NombreUsuario = usuario.NombreUsuario,
                Contraseña = usuario.Contraseña,
                IdRol = usuario.IdRol,
                Estado = usuario.Estado
            };

            DBContext.Usuarios.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }
        /*Actualizar un usuario*/
        [HttpPut("UpdateUser")]
        public async Task<HttpStatusCode> UpdateUser(Usuario usuario)
        {
            var entity = await DBContext.Usuarios.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);

            if (entity == null)
            {
                throw new Exception("El objeto response es nulo");
            }

            entity.Correo = usuario.Correo;
            entity.NombreUsuario = usuario.NombreUsuario;
            entity.Contraseña = usuario.Contraseña;
            entity.IdRol = usuario.IdRol;
            entity.Estado = usuario.Estado;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
        /*Eliminar un usuario*/
        [HttpDelete("DeleteUser")]
        public async Task<HttpStatusCode> DeleteUser(int Id)
        {
            var entity = new Usuario()
            {
                IdUsuario = Id
            };
            DBContext.Usuarios.Attach(entity);
            DBContext.Usuarios.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
