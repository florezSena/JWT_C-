using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Ventum
    {
        public Ventum()
        {
            Detalleventa = new HashSet<Detalleventum>();
        }

        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public float Total { get; set; }
        public int Estado { get; set; }
        /*Debe ponerse que el cliente navigation puede ser nulo para que lo deje consumir desde el mvc*/
        public virtual Cliente? IdClienteNavigation { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Detalleventum> Detalleventa { get; set; }
    }
}
