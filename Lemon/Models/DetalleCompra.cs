namespace Lemon.Models
{
    public class DetalleCompra
    {

        public int IdDetalleCompra { get; set; }
        public int IdCompra { get; set; }
        public int IdProducto { get; set; }
        public float Cantidad { get; set; }
        public float PrecioKilo { get; set; }
        public float Subtotal { get; set; }

        public virtual Compra IdCompraNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}