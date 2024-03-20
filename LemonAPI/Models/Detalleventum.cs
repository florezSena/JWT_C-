using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Detalleventum
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public float Cantidad { get; set; }
        public float PrecioKilo { get; set; }
        public float Subtotal { get; set; }
        public virtual Producto? IdProductoNavigation { get; set; } = null!;
        public virtual Ventum? IdVentaNavigation { get; set; } = null!;
    }
}
