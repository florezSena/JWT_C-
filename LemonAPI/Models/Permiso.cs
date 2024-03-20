using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Permiso
    {
        public Permiso()
        {
            Rolpermisos = new HashSet<Rolpermiso>();
        }

        public int IdPermiso { get; set; }
        public string Permiso1 { get; set; } = null!;

        [JsonIgnore]

        public virtual ICollection<Rolpermiso> Rolpermisos { get; set; }
    }
}
