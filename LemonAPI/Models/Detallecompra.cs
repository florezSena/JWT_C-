using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Detallecompra
    {
        public int IdDetalleCompra { get; set; }
        public int IdCompra { get; set; }
        public int IdProducto { get; set; }
        public float Cantidad { get; set; }
        public float PrecioKilo { get; set; }
        public float Subtotal { get; set; }
        [JsonIgnore]
        public virtual Compra IdCompraNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}
