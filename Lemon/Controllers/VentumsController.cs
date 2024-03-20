using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Lemon.Models;
using Lemon.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using X.PagedList;

namespace Lemon.Controllers
{
    public class VentumsController : Controller
    {
        public readonly IApiClient _client;
        private static readonly List<Producto> detalles = new();
        private static int clienteId;
        //Para poder redirigir al pdf cuando guarde el pdf

        private readonly IWebHostEnvironment _hostingEnvironment;

        public VentumsController(IApiClient client, IWebHostEnvironment hostingEnvironment)
        {
            _client = client;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int? page)
        {
            //var ventas = await _client.GetVentasAsync();
            //ViewData["Ventas"] = ventas;
            //return View(ventas);
            detalles.Clear();
            clienteId = 0;

            int pageSize = 5; // Número de elementos por página
            int pageNumber = page ?? 1;

            var ventas = await _client.GetVentasAsync();

            var pagedVentas = await ventas.ToPagedListAsync(pageNumber, pageSize);

            ViewData["Ventas"] = ventas;
            return View(pagedVentas);
        }
        
        public async Task<IActionResult> CreateDetalle()
        {
            var productos = await _client.GetProductAsync();

            var productosFiltrados = productos.Where(p => p.Estado == 1).ToList();
            if(clienteId!=0)
            {
                ViewData["cliente"] = clienteId;
            }
            ViewData["detalles"] = detalles;
            return View(productosFiltrados);
        }
        [HttpGet]
        public IActionResult EliminarDetalle(int id)
        {
            detalles.RemoveAll(detalle => detalle.IdProducto == id);
            
            return Json(new { success = true, message = "Detalle eliminado exitosamente" });
        }
        [HttpGet]
        public IActionResult AlmacenarDetalles(Producto detalle)
        {
            detalles.Add(detalle);
            return Json(new { success = true, message = "Detalles guardados exitosamente" });
        }


        /*En la vista createCliente necesitamos buscar el cliente por eso vamos a mandar los clientes con estado activo*/
        public async Task<IActionResult> CreateCliente()
        {
            var clientes = await _client.GetClientesAsync();
            var clientesFiltrados = clientes.Where(p => p.Estado == 1).ToList();
            ViewData["Clientes"] = clientesFiltrados;

            if (detalles.Any())
            {
                ViewData["Detalles"] = "Si hay detalles";
            }

            if (clienteId!=0)
            {
                var clienteGuardado = await _client.FindClienteAsync(clienteId);

                ViewData["Documento"] = clienteGuardado.Documento;
                ViewData["NombreRS"] = clienteGuardado.NombreRazonSocial;
                ViewData["Correo"] = clienteGuardado.Correo;
                ViewData["Telefono"] = clienteGuardado.Telefono;

                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult GuardarCliente(int id)
        {
            clienteId = id;

            return Json(new { success = true, message = "Cliente guardado exitosamente" });
        }

        /*Ya tenemos todos los datos para crear la venta solo falta la confirmacion*/
        public async Task<IActionResult> Create()
        {
            double total = 0;

            foreach (var detalle in detalles)
            {
                total += (detalle.Costo * detalle.Cantidad);
            }
            if (clienteId != 0)
            {
                Cliente clienteGuardado = await _client.FindClienteAsync(clienteId);
                ViewData["Documento"] = clienteGuardado.Documento;
                ViewData["NombreRS"] = clienteGuardado.NombreRazonSocial;
                ViewData["IdCliente"] = clienteGuardado.IdCliente;

            }
            if (detalles.Any())
            {
                ViewData["Detalles"] = detalles;
            }
            // Convertir a cadena con dos decimales
            string totalString = total.ToString("0.00", CultureInfo.InvariantCulture);

            ViewData["TotalVenta"] = totalString;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAJAX(Ventum venta)
        {
            if (!detalles.Any())
            {
                return Json(new { error = true, message = "No puedes crear una venta sin detalles" });
            }

            if (ModelState.IsValid)
            {

                var response = await _client.CreateVentaAsync(venta);

                if (response.IsSuccessStatusCode)
                {

                    var ventas = await _client.GetVentasAsync();
                    var ultimoIdVenta = ventas.Max(v => v.IdVenta);
                    foreach (var detalle in detalles)
                    {

                        Detalleventum nuevoDetalle = new Detalleventum
                        {
                            IdVenta = ultimoIdVenta,
                            IdProducto = detalle.IdProducto,
                            Cantidad = detalle.Cantidad,
                            PrecioKilo = detalle.Costo,
                            Subtotal = (detalle.Costo * detalle.Cantidad)
                        };

                        await _client.CreateDetalleventaAsync(nuevoDetalle);

                        Producto productoReducir = await _client.FindProductAsync(detalle.IdProducto);

                        productoReducir.Cantidad += detalle.Cantidad;

                        await _client.UpdateProductAsync(productoReducir);

                    }


                    detalles.Clear();
                    //La solicitud Post fue exitosa
                    return Json(new { success = true, message = "Venta creada exitosamente" });

                }
                else
                {
                    return Json(new { error = true, message = "Error en la creacion de la venta" });
                }
            }
            return Json(new { error = true, message = "Error en el modelo" });
        }

        public async Task<IActionResult> CambiarEstado(int id)
        {
            Ventum ventum = await _client.FindVentaAsync(id);

            if (ventum == null)
            {
                return NotFound();
            }

            var detallesVenta= await _client.GetDetallesventaAsync(ventum.IdVenta);

            foreach(Detalleventum detalle in detallesVenta)
            {

                Producto productoReducir = await _client.FindProductAsync(detalle.IdProducto);

                productoReducir.Cantidad += detalle.Cantidad;

                await _client.UpdateProductAsync(productoReducir);
            }

            ventum.Estado = 0;

            var response = await _client.UpdateVentaAsync(ventum);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> VisualizarVenta(int id)
        {
            Ventum ventum = await _client.FindVentaAsync(id);

            if (ventum == null)
            {
                return NotFound();
            }

            var detallesVenta= await _client.GetDetallesventaAsync(ventum.IdVenta);
            ViewData["DetallesVenta"] = detallesVenta;
            return View(ventum);
        }


        public async Task<IActionResult> GenerarPdf()
        {
            // Obtener los datos de proveedores desde la API
            var ventas = await _client.GetVentasAsync();

            // Generar el PDF
            //Aqui le estamos diciendo que lo mande al wwwroot para poder acceder a el al momento de querer redirigirlo al pdf
            PdfWriter pdfWriter = new PdfWriter(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "ReporteVentas.pdf"));
            PdfDocument pdf = new PdfDocument(pdfWriter);
            Document document = new Document(pdf, PageSize.LETTER);

            // Márgenes del documento
            document.SetMargins(60, 20, 55, 20);

            // Imagen
            string imagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Lemon_Logo2.png");
            Image logo = new Image(ImageDataFactory.Create(imagePath)).SetHeight(80).SetWidth(145);

            // **Encabezado**

            // Tabla para el encabezado
            Table encabezado = new Table(3);
            encabezado.SetWidth(UnitValue.CreatePercentValue(100));

            PdfFont fontContenidoHeader = PdfFontFactory.CreateFont(StandardFonts.TIMES_ITALIC);

            // Celda para la imagen
            Cell imageCell = new Cell().Add(logo).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);

            // Celda para los datos
            Cell datosCelda = new Cell();
            datosCelda.SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
            datosCelda.Add(new Paragraph("LEMON\n").SetFontSize(14)).SetFont(fontContenidoHeader);
            datosCelda.Add(new Paragraph("Direccion: Cl. 55 #57-80\n").SetFontSize(12));
            datosCelda.Add(new Paragraph("sector 3 - local 166\n").SetFontSize(10));

            Cell datosCelda2 = new Cell();
            datosCelda2.SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
            // Obtener la fecha actual
            DateTime fechaActual = DateTime.Now;
            // Formatear la fecha en el formato deseado
            string fechaFormateada = fechaActual.ToString("dd/MM/yyyy");
            // Agregar la fecha al documento
            datosCelda2.Add(new Paragraph($"Fecha: {fechaFormateada}\n").SetFontSize(12));
            datosCelda2.Add(new Paragraph("Celular: 3154385456\n").SetFontSize(12)).SetFont(fontContenidoHeader);
            datosCelda2.Add(new Paragraph("Correo: hernanocampo07@hotmail.com\n").SetFontSize(10));

            // Agregar celdas al encabezado
            encabezado.AddCell(imageCell);
            encabezado.AddCell(datosCelda);
            encabezado.AddCell(datosCelda2);


            // Agregar encabezado al documento
            document.Add(encabezado);

            // **Título**
            PdfFont fontContenidoTitulo = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            document.Add(new Paragraph("Reporte de Ventas").SetTextAlignment(TextAlignment.CENTER).SetFontSize(16).SetFont(fontContenidoTitulo));
            document.Add(new Paragraph("\n"));
            // **Tabla de proveedores**

            // Definir fuentes
            PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            // Columnas
            string[] columnas = { "#", "Cliente", "Fecha", "Total", "Estado" };

            // Tamaños de las columnas
            float[] tamanios = { 1, 4, 4, 2, 1 };

            // Crear tabla
            Table tabla = new Table(UnitValue.CreatePercentArray(tamanios));
            tabla.SetWidth(UnitValue.CreatePercentValue(100));

            // Color verde oscuro
            Color verdeOscuro = new DeviceRgb(7, 115, 54);

            // Agregar encabezados
            foreach (string columna in columnas)
            {
                Cell headerCell = new Cell().Add(new Paragraph(columna).SetFont(fontColumnas));
                headerCell.SetBackgroundColor(verdeOscuro);
                headerCell.SetFontColor(ColorConstants.WHITE);
                tabla.AddHeaderCell(headerCell);
            }

            // Agregar datos
            foreach (var venta in ventas)
            {
                tabla.AddCell(new Cell().Add(new Paragraph(venta.IdVenta.ToString()).SetFont(fontContenido).SetFontSize(11)));
                if (venta.IdClienteNavigation != null)
                {
                    tabla.AddCell(new Cell().Add(new Paragraph(venta.IdClienteNavigation.NombreRazonSocial).SetFont(fontContenido).SetFontSize(11)));
                }
                else
                {
                    // Agregar una celda vacía o un valor predeterminado en caso de que IdClienteNavigation sea nulo
                    tabla.AddCell(new Cell().Add(new Paragraph("Cliente no disponible").SetFont(fontContenido).SetFontSize(11)));
                }
                tabla.AddCell(new Cell().Add(new Paragraph(venta.Fecha.ToString()).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(string.Format("${0:N}", venta.Total)).SetFont(fontContenido).SetFontSize(11)));
                string estadoTexto = venta.Estado == 1 ? "Realizada" : "Anulada";
                tabla.AddCell(new Cell().Add(new Paragraph(estadoTexto).SetFont(fontContenido).SetFontSize(11)));
            }

            // Agregar tabla al documento
            document.Add(tabla);

            PieDePaginaEventHandler pieDePaginaEventHandler = new PieDePaginaEventHandler();

            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, pieDePaginaEventHandler);

            document.Close();

            return Json(new { success = true, message = "Reporte creado exitosamente" });

        }

        public class PieDePaginaEventHandler : IEventHandler
        {
            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

                canvas
                    .BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 10)
                    .MoveText(page.GetPageSize().GetWidth() / 2 - 30, 20)
                    .ShowText("Pie de página - Página " + pdfDoc.GetPageNumber(page))
                    .EndText();
                canvas.Release();
            }
        }
    }
}
