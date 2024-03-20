using Lemon.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Lemon.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpclient;

        private static string keyJwt;

        public ApiClient(HttpClient httpClient)
        {
            _httpclient = httpClient;
            _httpclient.BaseAddress = new Uri("https://localhost:7219/api/");//solicitud


        }
        //Comienza Permisos

        public async Task<IEnumerable<Permiso>> GetPermisosAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Permiso>>("Permisos/GetPermisos");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> CreatePermisosAsync(Permiso permiso)
        {
            var response = await _httpclient.PostAsJsonAsync("Permisos/InsertPermisos", permiso);
            return response;
        }

        public async Task<Permiso> FindPermisosAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Permiso>($"Permisos/GetPermisoById?id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> UpdatePermisosAsync(Permiso permiso)
        {
            var response = await _httpclient.PutAsJsonAsync("Permisos/UpdatePermisos", permiso);
            return response;
        }

        public async Task<HttpResponseMessage> DeletePermisosAsync(int id)
        {
            var response = await _httpclient.DeleteAsync($"Permisos/DeletePermiso/{id}");
            return response;
        }

        //Comienza Roles
        public async Task<IEnumerable<Rol>> GetRolesAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Rol>>("Roles/GetRoles");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> CreateRolesAsync(Rol rol)
        {
            var response = await _httpclient.PostAsJsonAsync("Roles/InsertRoles", rol);
            return response;
        }

        public async Task<Rol> FindRolesAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Rol>($"Roles/GetRolesById?id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> UpdateRolesAsync(Rol rol)
        {
            var response = await _httpclient.PutAsJsonAsync("Roles/UpdateRoles", rol);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteRolesAsync(int id)
        {
            var response = await _httpclient.DeleteAsync($"Roles/DeleteRol/{id}");
            return response;
        }

        //Comienza RolXPermiso
        public async Task<IEnumerable<Rolpermiso>> GetRolXPermisoAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Rolpermiso>>("RolXPermisos/GetRolXPermisos");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> CreateRolXPermisoAsync(Rolpermiso rolpermiso)
        {
            var response = await _httpclient.PostAsJsonAsync("RolXPermisos/InsertRolXPermiso", rolpermiso);
            return response;
        }

        public async Task<IEnumerable<Rolpermiso>> FindRolXPermioAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Rolpermiso>>($"RolXPermisos/GetRolXPermisoById?id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> UpdateRolXPermisoAsync(Rolpermiso rolpermiso)
        {
            var response = await _httpclient.PutAsJsonAsync("RolXPermisos/UpdateRolXPermisos", rolpermiso);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteRolXPermisoAsync(int id)
        {
            var response = await _httpclient.DeleteAsync($"RolXPermisos/DeleteRolXPermiso/{id}");
            return response;
        }

        /*Comienza usuarios-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<dynamic> Login(string usuario, string password)
        {
            dynamic nuevoUsuario = new
            {
                usuario = usuario,
                password = password
            };
            string json = JsonConvert.SerializeObject(nuevoUsuario);
            HttpResponseMessage response = await _httpclient.PostAsJsonAsync("Usuarios/Login", json);
            Console.WriteLine(response);

            response.EnsureSuccessStatusCode();

            string responseContent = await response.Content.ReadAsStringAsync();
            dynamic responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

            Console.WriteLine(responseData);
            Console.WriteLine(responseData.result);


            keyJwt = responseData.result;

            return responseData;
        }
        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Usuario>>("Usuarios/GetUsers");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> CreateUsuarioAsync(Usuario usuario)
        {
            var response = await _httpclient.PostAsJsonAsync("Usuarios/InsertUser", usuario);
            return response;
        }
        public async Task<UsuarioUpdate> FindUsuarioAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<UsuarioUpdate>($"Usuarios/GetUserById?Id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> UpdateUsuarioAsync(UsuarioUpdate usuario)
        {
            var response = await _httpclient.PutAsJsonAsync("Usuarios/UpdateUser", usuario);
            return response;
        }
        public async Task<HttpResponseMessage> DeleteUsuarioAsync(int idDelete)
        {
            var response = await _httpclient.DeleteAsync($"Usuarios/DeleteUser?Id={idDelete}");
            return response;
        }

        /*Comienzaa Compras-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<IEnumerable<Compra>> GetComprasAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Compra>>("Compras/GetCompras");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> CreateComprasAsync(Compra compra)
        {
            var response = await _httpclient.PostAsJsonAsync("Compras/InsertCompra", compra);
            return response;
        }
        public async Task<Compra> FindComprasAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Compra>($"Compras/GetCompraById?Id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> UpdateComprasAsync(Compra compra)
        {
            var response = await _httpclient.PutAsJsonAsync("Compras/UpdateCompra", compra);
            return response;
        }
        /*Comienza detalle de Compras-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<HttpResponseMessage> CreateDetalleComprasAsync(DetalleCompra detallecompra)
        {
            var response = await _httpclient.PostAsJsonAsync("DetalleCompras/InsertDetalleCompra", detallecompra);
            return response;
        }
        public async Task<IEnumerable<DetalleCompra>> GetDetalleCompras(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<DetalleCompra>>($"DetalleCompras/GetDetallesCompra?Id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        //Inicia Movimiento
        public async Task<IEnumerable<Movimiento>> GetMovementAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Movimiento>>("Movimientos/GetMovement");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> CreateMovementAsync(Movimiento movimiento)
        {
            var response = await _httpclient.PostAsJsonAsync("Movimientos/InsertMovement", movimiento);
            return response;
        }

        public async Task<Movimiento> FindMovementAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Movimiento>($"Movimientos/GetMovementById?id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> UpdateMovementAsync(Movimiento movimiento)
        {
            var response = await _httpclient.PutAsJsonAsync("Movimientos/UpdateMovement", movimiento);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteMovementAsync(int id)
        {
            var response = await _httpclient.DeleteAsync($"Movimientos/DeleteMovement/{id}");
            return response;
        }

        //Inicia TipoMovimiento
        public async Task<IEnumerable<Tipomovimiento>> GetTypeAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Tipomovimiento>>("TipoMovimientos/GetType");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> CreateTypeAsync(Tipomovimiento tipomovimiento)
        {
            var response = await _httpclient.PostAsJsonAsync("TipoMovimientos/InsertType", tipomovimiento);
            return response;
        }

        public async Task<Tipomovimiento> FindTypeAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Tipomovimiento>($"Tipomovimiento/GetTypeById?id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> UpdateTypeAsync(Tipomovimiento tipomovimiento)
        {
            var response = await _httpclient.PutAsJsonAsync("Tipomovimiento/UpdateType", tipomovimiento);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteTypeAsync(int id)
        {
            var response = await _httpclient.DeleteAsync($"Tipomovimiento/DeleteType/{id}");
            return response;
        }

        /*Comienza Productos-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<IEnumerable<Producto>> GetProductAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Producto>>("Productos/GetProduct");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> CreateProductAsync(Producto producto)
        {
            var response = await _httpclient.PostAsJsonAsync("Productos/InsertProduct", producto);
            return response;
        }

        public async Task<Producto> FindProductAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Producto>($"Productos/GetProductById?id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> UpdateProductAsync(Producto producto)
        {
            var response = await _httpclient.PutAsJsonAsync("Productos/UpdateProduct/", producto);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteProductAsync(int id)
        {
            var response = await _httpclient.DeleteAsync($"Productos/DeleteProduct/{id}");
            return response;
        }

        /*Comienza ventas-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<IEnumerable<Ventum>> GetVentasAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Ventum>>("Ventums/GetVentas");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> CreateVentaAsync(Ventum ventum)
        {
            var response = await _httpclient.PostAsJsonAsync("Ventums/InsertVenta", ventum);
            return response;
        }
        public async Task<Ventum> FindVentaAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Ventum>($"Ventums/GetVentaById?Id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> UpdateVentaAsync(Ventum ventum)
        {
            var response = await _httpclient.PutAsJsonAsync("Ventums/UpdateVenta", ventum);
            return response;
        }

        /*Comienza detalle de venta-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<HttpResponseMessage> CreateDetalleventaAsync(Detalleventum detalleventum)
        {
            var response = await _httpclient.PostAsJsonAsync("DetalleVentums/InsertDetalleVenta", detalleventum);
            return response;
        }
        public async Task<IEnumerable<Detalleventum>> GetDetallesventaAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Detalleventum>>($"DetalleVentums/GetDetallesVenta?Id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        /*Comienza clientes-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<IEnumerable<Cliente>> GetClientesAsync()
        {
            Console.WriteLine("Key guardada: " + keyJwt);
            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", keyJwt);

            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Cliente>>("Clientes/GetClients");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> CreateClienteAsync(Cliente cliente)
        {
            var response = await _httpclient.PostAsJsonAsync("Clientes/InsertClient", cliente);
            return response;
        }
        public async Task<Cliente> FindClienteAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Cliente>($"Clientes/GetClientById?Id={id}");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }
        public async Task<HttpResponseMessage> UpdateClienteAsync(Cliente cliente)
        {
            var response = await _httpclient.PutAsJsonAsync("Clientes/UpdateClient", cliente);
            return response;
        }
        public async Task<HttpResponseMessage> DeleteClienteAsync(int idDelete)
        {
            var response = await _httpclient.DeleteAsync($"Clientes/DeleteClient?Id={idDelete}");
            return response;
        }

        /*Comienza Proveedores-----------------------------------------------------------------------------------------------------------------------------------------------*/
        public async Task<IEnumerable<Proveedor>> GetProveedorAsync()
        {
            var response = await _httpclient.GetFromJsonAsync<IEnumerable<Proveedor>>("Proveedores/GetProveedores");
            if (response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> CreateProveedorAsync(Proveedor proveedor)
        {
            var response = await _httpclient.PostAsJsonAsync("Proveedores/InsertProveedores", proveedor);
            return response;
        }

        public async Task<Proveedor> FindProveedorAsync(int id)
        {
            var response = await _httpclient.GetFromJsonAsync<Proveedor>($"Proveedores/GetProveedoresById?id={id}");
            if(response == null)
            {
                throw new Exception("El objeto response es nulo");
            }
            return response;
        }

        public async Task<HttpResponseMessage> UpdateProveedorAsync(Proveedor proveedor)
        {
            var response = await _httpclient.PutAsJsonAsync("Proveedores/UpdateProveedores", proveedor);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteProveedorAsync(int id)
        {
            var response = await _httpclient.DeleteAsync($"Proveedores/DeleteProveedor/{id}");
            return response;
        }

    }
}
