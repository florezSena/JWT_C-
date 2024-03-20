using System;
using System.Collections.Generic;

namespace LemonAPI.Models
{
    public partial class Usuario
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public int IdRol { get; set; }
        public int Estado { get; set; }
        public virtual Rol? IdRolNavigation { get; set; } = null!;
    }
}
