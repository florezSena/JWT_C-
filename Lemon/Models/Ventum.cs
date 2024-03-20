namespace Lemon.Models
{
    public class Ventum
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public float Total { get; set; }
        public int Estado { get; set; }
        public virtual Cliente? IdClienteNavigation { get; set; } = null!;
    }
}
