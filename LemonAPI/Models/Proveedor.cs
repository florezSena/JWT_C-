using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Proveedor
    {
        public Proveedor()
        {
            Compras = new HashSet<Compra>();
        }

        public int IdProveedor { get; set; }
        public string TipoDocumento { get; set; } = null!;
        public int Documento { get; set; }
        public string NombreRazonSocial { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public int Estado { get; set; }
        [JsonIgnore]
        public virtual ICollection<Compra> Compras { get; set; }
    }
}
