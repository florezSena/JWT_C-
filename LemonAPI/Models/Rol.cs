using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Rolpermisos = new HashSet<Rolpermiso>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdRol { get; set; }
        public string Nombre { get; set; } = null!;
        public int Estado { get; set; }

        [JsonIgnore]
        public virtual ICollection<Rolpermiso> Rolpermisos { get; set; }
        [JsonIgnore]
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
