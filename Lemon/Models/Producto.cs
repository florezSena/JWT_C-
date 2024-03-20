namespace Lemon.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public float Cantidad { get; set; }
        public string? Descripcion { get; set; }
        public float Costo { get; set; }
        public int Estado { get; set; }

    }
}
