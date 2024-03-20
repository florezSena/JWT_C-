namespace Lemon.Models
{
    public class Movimiento
    {
        public int IdMovimiento { get; set; }
        public int IdTipoMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public int IdProducto { get; set; }
        public float Cantidad { get; set; }
    }
}
