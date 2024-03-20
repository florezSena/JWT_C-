using Lemon.Models;

namespace Lemon.Services
{
    public interface IApiClient
    {
        //Metodos de Permisos
        Task<IEnumerable<Permiso>> GetPermisosAsync();
        Task<HttpResponseMessage> CreatePermisosAsync(Permiso permiso);
        Task<Permiso> FindPermisosAsync(int id);
        Task<HttpResponseMessage> UpdatePermisosAsync(Permiso permiso);
        Task<HttpResponseMessage> DeletePermisosAsync(int id);

        //Metodo de Roles
        Task<IEnumerable<Rol>> GetRolesAsync();
        Task<HttpResponseMessage> CreateRolesAsync(Rol rol);
        Task<Rol> FindRolesAsync(int id);
        Task<HttpResponseMessage> UpdateRolesAsync(Rol rol);
        Task<HttpResponseMessage> DeleteRolesAsync(int id);

        //Metodo de RolXPermiso
        Task<IEnumerable<Rolpermiso>> GetRolXPermisoAsync();
        Task<HttpResponseMessage> CreateRolXPermisoAsync(Rolpermiso rolpermiso);
        Task<IEnumerable<Rolpermiso>> FindRolXPermioAsync(int id);
        Task<HttpResponseMessage> UpdateRolXPermisoAsync(Rolpermiso rolpermiso);
        Task<HttpResponseMessage> DeleteRolXPermisoAsync(int id);

        //Metodos de compras
        Task<IEnumerable<Compra>> GetComprasAsync();
        Task<HttpResponseMessage> CreateComprasAsync(Compra compra);
        Task<Compra> FindComprasAsync(int id);
        Task<HttpResponseMessage> UpdateComprasAsync(Compra compra);

        /*Metodos detalles de compra*/
        Task<IEnumerable<DetalleCompra>> GetDetalleCompras(int id);
        Task<HttpResponseMessage> CreateDetalleComprasAsync(DetalleCompra detalleCompra);

        //TipoMovimiento
        Task<IEnumerable<Tipomovimiento>> GetTypeAsync();
        Task<HttpResponseMessage> CreateTypeAsync(Tipomovimiento tipomovimiento);//PARA PODER CREAR

        Task<Tipomovimiento> FindTypeAsync(int id);

        Task<HttpResponseMessage> UpdateTypeAsync(Tipomovimiento tipomovimiento);

        Task<HttpResponseMessage> DeleteTypeAsync(int id);

        //Movimiento
        Task<IEnumerable<Movimiento>> GetMovementAsync();
        Task<HttpResponseMessage> CreateMovementAsync(Movimiento movimiento);//PARA PODER CREAR

        Task<Movimiento> FindMovementAsync(int id);

        Task<HttpResponseMessage> UpdateMovementAsync(Movimiento movimiento);

        Task<HttpResponseMessage> DeleteMovementAsync(int id);

        //Productos

        Task<IEnumerable<Producto>> GetProductAsync();
        Task<HttpResponseMessage> CreateProductAsync(Producto producto);//PARA PODER CREAR

        Task<Producto> FindProductAsync(int id);

        Task<HttpResponseMessage> UpdateProductAsync(Producto producto);

        Task<HttpResponseMessage> DeleteProductAsync(int id);


        /*Metodos de venta*/
        Task<IEnumerable<Ventum>> GetVentasAsync();
        Task<HttpResponseMessage> CreateVentaAsync(Ventum ventum);
        Task<Ventum> FindVentaAsync(int id);
        Task<HttpResponseMessage> UpdateVentaAsync(Ventum ventum);

        /*Metodos detalles de venta*/
        Task<IEnumerable<Detalleventum>> GetDetallesventaAsync(int id);
        Task<HttpResponseMessage> CreateDetalleventaAsync(Detalleventum detalleventum);

        /*Metodos de clinetes*/
        Task<IEnumerable<Cliente>> GetClientesAsync();
        Task<HttpResponseMessage> CreateClienteAsync(Cliente cliente);
        Task<Cliente> FindClienteAsync(int id);
        Task<HttpResponseMessage> UpdateClienteAsync(Cliente cliente);
        Task<HttpResponseMessage> DeleteClienteAsync(int id);

        /*Metodos de usuarios*/
        Task<dynamic> Login(string correo, string password);
        Task<IEnumerable<Usuario>> GetUsuariosAsync();
        Task<HttpResponseMessage> CreateUsuarioAsync(Usuario usuario);
        Task<UsuarioUpdate> FindUsuarioAsync(int id);
        Task<HttpResponseMessage> UpdateUsuarioAsync(UsuarioUpdate usuario);
        Task<HttpResponseMessage> DeleteUsuarioAsync(int id);

        /*Metodos de proveedores*/
        Task<IEnumerable<Proveedor>> GetProveedorAsync();
        Task<HttpResponseMessage> CreateProveedorAsync(Proveedor proveedor);//PARA PODER CREAR
        Task<Proveedor> FindProveedorAsync(int id);
        Task<HttpResponseMessage> UpdateProveedorAsync(Proveedor proveedor);
        Task<HttpResponseMessage> DeleteProveedorAsync(int id);
    }
}
