using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LemonAPI.Models
{
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static string EncodePassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public async static Task<dynamic> validarToken(ClaimsIdentity identity,dblemonContext DBContext)
        {
            try
            {
                if(identity.Claims.Count()==0)
                {
                    return new
                    {
                        success = false,
                        message = "Su token es incorrecto",
                        result = ""
                    };
                }

                var id = identity.Claims.FirstOrDefault(x=>x.Type== "idUser").Value;

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
                .FirstOrDefaultAsync(s => s.IdUsuario.ToString() == id );

                return new
                {
                    success = true,
                    message = "Exito",
                    result = usuario
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Error catch: "+ex.Message,
                    result = ""
                };
            }
        }
    }
}
