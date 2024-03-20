namespace Lemon.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public float ValorCompra { get; set; }
        public int Estado { get; set; }
        public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
        public virtual ICollection<DetalleCompra> Detallecompras { get; set; }
    }
}
