using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Venta = new HashSet<Ventum>();
        }

        public int IdCliente { get; set; }
        public string TipoDocumento { get; set; } = null!;
        public int Documento { get; set; }
        public string NombreRazonSocial { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public int Estado { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ventum> Venta { get; set; }
    }
}
