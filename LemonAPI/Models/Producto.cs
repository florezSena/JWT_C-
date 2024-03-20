using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Detallecompras = new HashSet<Detallecompra>();
            Detalleventa = new HashSet<Detalleventum>();
            Movimientos = new HashSet<Movimiento>();
        }

        public int IdProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public float Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public float Costo { get; set; }
        public int Estado { get; set; }
        [JsonIgnore]
        public virtual ICollection<Detallecompra> Detallecompras { get; set; }
        [JsonIgnore]
        public virtual ICollection<Detalleventum> Detalleventa { get; set; }
        [JsonIgnore]
        public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
