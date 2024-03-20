using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Compra
    {
        public Compra()
        {
            Detallecompras = new HashSet<Detallecompra>();
        }

        public int IdCompra { get; set; }
        public int IdProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public float ValorCompra { get; set; }
        public int Estado { get; set; }
        public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Detallecompra> Detallecompras { get; set; }
    }
}
