using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LemonAPI.Models
{
    public partial class Tipomovimiento
    {
        public Tipomovimiento()
        {
            Movimientos = new HashSet<Movimiento>();
        }

        public int IdTipoMovimiento { get; set; }
        public string Nombre { get; set; } = null!;
        public int Estado { get; set; }

        [JsonIgnore]
        public virtual ICollection<Movimiento> Movimientos { get; set; }
    }
}
