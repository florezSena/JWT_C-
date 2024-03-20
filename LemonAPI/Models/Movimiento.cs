using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Movimiento
    {
        public int IdMovimiento { get; set; }
        public int IdTipoMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public int IdProducto { get; set; }
        public float Cantidad { get; set; }
        public virtual Producto? IdProductoNavigation { get; set; } = null!;
        public virtual Tipomovimiento? IdTipoMovimientoNavigation { get; set; } = null!;
    }
}
