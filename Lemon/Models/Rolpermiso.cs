namespace Lemon.Models
{
    public partial class Rolpermiso
    {
        public int IdRolPermiso { get; set; }
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }

        public virtual Permiso? IdPermisoNavigation { get; set; } = null!;
        public virtual Rol? IdRolNavigation { get; set; } = null!;
    }
}
