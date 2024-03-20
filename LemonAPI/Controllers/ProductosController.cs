using LemonAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace LemonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly dblemonContext DBContext;

        public ProductosController(dblemonContext DBContext)
        {
            this.DBContext = DBContext;
        }

        [HttpGet("GetProduct")]
        public async Task<ActionResult<List<Producto>>> Get()
        {
            var List = await DBContext.Productos.Select(
                s => new Producto
                {
                    IdProducto = s.IdProducto,
                    Nombre = s.Nombre,
                    Cantidad = s.Cantidad,
                    Descripcion = s.Descripcion,
                    Costo = s.Costo,
                    Estado = s.Estado
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("GetProductById")]
        public async Task<ActionResult<Producto>> GetProductById(int Id)
        {
            Producto User = await DBContext.Productos.Select(
                    s => new Producto
                    {
                        IdProducto = s.IdProducto,
                        Nombre = s.Nombre,
                        Cantidad = s.Cantidad,
                        Descripcion = s.Descripcion,
                        Costo = s.Costo,
                        Estado = s.Estado
                    })
                .FirstOrDefaultAsync(s => s.IdProducto == Id);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }

        [HttpPost("InsertProduct")]
        public async Task<HttpStatusCode> Create(Producto Producto)
        {
            var entity = new Producto()
            {
                IdProducto = Producto.IdProducto,
                Nombre = Producto.Nombre,
                Cantidad = Producto.Cantidad,
                Descripcion = Producto.Descripcion,
                Costo = Producto.Costo,
                Estado = Producto.Estado
            };

            DBContext.Productos.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateProduct")]
        public async Task<HttpStatusCode> Update(Producto Producto)
        {
            var entity = await DBContext.Productos.FirstOrDefaultAsync(s => s.IdProducto == Producto.IdProducto);

            entity.IdProducto = Producto.IdProducto;
            entity.Nombre = Producto.Nombre;
            entity.Cantidad = Producto.Cantidad;
            entity.Descripcion = Producto.Descripcion;
            entity.Costo = Producto.Costo;
            entity.Estado = Producto.Estado;


            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteProduct/{Id}")]
        public async Task<HttpStatusCode> Delete(int Id)
        {
            var entity = new Producto()
            {
                IdProducto = Id
            };
            DBContext.Productos.Attach(entity);
            DBContext.Productos.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

    }
}
