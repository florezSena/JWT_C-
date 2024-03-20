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
    public class ComprasController : Controller
    {
        public readonly IApiClient _client;
        private static readonly List<Producto> detalles = new();
        private static int proveedorId;


        public ComprasController(IApiClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index(int? page)
        {
            //var ventas = await _client.GetVentasAsync();
            //ViewData["Ventas"] = ventas;
            //return View(ventas);
            detalles.Clear();
            proveedorId = 0;

            int pageSize = 5; // Número de elementos por página
            int pageNumber = page ?? 1;

            var compras = await _client.GetComprasAsync();

            var pagedCompras = await compras.ToPagedListAsync(pageNumber, pageSize);

            ViewData["Compras"] = compras;
            return View(pagedCompras);
        }



        public async Task<IActionResult> CreateDetalle()
        {
            var productos = await _client.GetProductAsync();

            var productosFiltrados = productos.Where(p => p.Estado == 1).ToList();
            if (proveedorId != 0)
            {
                ViewData["proveedor"] = proveedorId;
            }
            ViewData["detalles"] = detalles;
            return View(productosFiltrados);
        }
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


        /*En la vista createproveedor necesitamos buscar el cliente por eso vamos a mandar los clientes con estado activo*/
        public async Task<IActionResult> CreateProveedor()
        {
            var proveedores = await _client.GetClientesAsync();
            var proveedoresFiltrados = proveedores.Where(p => p.Estado == 1).ToList();

            if (detalles.Any())
            {

                ViewData["Detalles"] = 1;
            }

            if (proveedorId != 0)
            {
                var clienteGuardado = await _client.FindClienteAsync(proveedorId);
                ViewData["Proveedores"] = proveedoresFiltrados;

                ViewData["Documento"] = clienteGuardado.Documento;
                ViewData["NombreRS"] = clienteGuardado.NombreRazonSocial;
                ViewData["Correo"] = clienteGuardado.Correo;
                ViewData["Telefono"] = clienteGuardado.Telefono;

                return View();
            }

            ViewData["Proveedores"] = proveedoresFiltrados;
            return View();
        }
        [HttpGet]
        public IActionResult GuardarProveedor(int id)
        {
            proveedorId = id;

            return Json(new { success = true, message = "Proveedor guardado exitosamente" });
        }

        /*Ya tenemos todos los datos para crear la venta solo falta la confirmacion*/
        public async Task<IActionResult> Create()
        {
            double total = 0;

            foreach (var detalle in detalles)
            {
                total += (detalle.Costo * detalle.Cantidad);
            }
            if (proveedorId != 0)
            {
                Cliente clienteGuardado = await _client.FindClienteAsync(proveedorId);
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

            ViewData["TotalCompra"] = totalString;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateAJAX(Compra compra)
        {
            if (!detalles.Any())
            {
                Console.WriteLine("La lista está vacía.");
                return Json(new { error = true, message = "No puedes crear una Compra sin detalles" });
            }

            var proveedores = await _client.GetClientesAsync();

            var proveedoresFiltrados = proveedores.Where(p => p.Estado == 1).ToList();

            double total = 0;

            foreach (var detalle in detalles)
            {
                total += (detalle.Costo * detalle.Cantidad);
            }

            // Convertir a cadena con dos decimales
            string totalString = total.ToString("0.00", CultureInfo.InvariantCulture);


            if (ModelState.IsValid)
            {

                var response = await _client.CreateComprasAsync(compra);


                if (response.IsSuccessStatusCode)
                {

                    var compras = await _client.GetComprasAsync();
                    var ultimoIdcompra = compras.Max(v => v.IdCompra);
                    foreach (var detalle in detalles)
                    {

                        DetalleCompra nuevoDetalle = new DetalleCompra
                        {
                            IdCompra = ultimoIdcompra,
                            IdProducto = detalle.IdProducto,
                            Cantidad = detalle.Cantidad,
                            PrecioKilo = detalle.Costo,
                            Subtotal = (detalle.Costo * detalle.Cantidad)
                        };

                        await _client.CreateDetalleComprasAsync(nuevoDetalle);

                        Producto productoReducir = await _client.FindProductAsync(detalle.IdProducto);

                        productoReducir.Cantidad -= detalle.Cantidad;

                        await _client.UpdateProductAsync(productoReducir);

                    }


                    detalles.Clear();
                    //La solicitud Post fue exitosa
                    return Json(new { success = true, message = "Compra creada exitosamente" });

                }
                else
                {
                    return Json(new { error = true, message = "Error en la creacion de la Compra" });
                }
            }
            return Json(new { error = true, message = "Error en el modelo" });
        }

        public async Task<IActionResult> CambiarEstado(int id)
        {
            Compra compra = await _client.FindComprasAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            var detallesCompra = await _client.GetDetalleCompras(compra.IdCompra);

            foreach (DetalleCompra detalle in detallesCompra)
            {

                Producto productoReducir = await _client.FindProductAsync(detalle.IdProducto);

                productoReducir.Cantidad += detalle.Cantidad;

                await _client.UpdateProductAsync(productoReducir);
            }

            compra.Estado = 0;

            var response = await _client.UpdateComprasAsync(compra);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> VisualizarCompra(int id)
        {
            Compra compra = await _client.FindComprasAsync(id);

            if (compra == null)
            {
                return NotFound();
            }

            var detallesCompra = await _client.GetDetalleCompras(compra.IdCompra);
            ViewData["DetallesCompra"] = detallesCompra;
            return View(compra);
        }
        //instalar iText7 (7.1.10)
        public async Task<IActionResult> GenerarPdf()
        {
            // Obtener los datos de proveedores desde la API
            var compras = await _client.GetComprasAsync();

            // Generar el PDF
            PdfWriter pdfWriter = new PdfWriter("ReporteCompras.pdf");
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
            datosCelda2.Add(new Paragraph("Estado Proveedores\n").SetFontSize(14)).SetFont(fontContenidoHeader);
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
            document.Add(new Paragraph("Reporte de Proveedores").SetTextAlignment(TextAlignment.CENTER).SetFontSize(16).SetFont(fontContenidoTitulo));
            document.Add(new Paragraph("\n"));
            // **Tabla de proveedores**

            // Definir fuentes
            PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            // Columnas
            string[] columnas = { "Tipo Doc", "Documento", "Nombre", "Correo", "Telefono", "Estado" };

            // Tamaños de las columnas
            float[] tamanios = { 2, 3, 5, 4, 3, 2 };

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
            //foreach (var proveedor in proveedores)
            //{
            //    tabla.AddCell(new Cell().Add(new Paragraph(proveedor.TipoDocumento).SetFont(fontContenido).SetFontSize(11)));
            //    tabla.AddCell(new Cell().Add(new Paragraph(proveedor.Documento.ToString()).SetFont(fontContenido).SetFontSize(11)));
            //    tabla.AddCell(new Cell().Add(new Paragraph(proveedor.NombreRazonSocial).SetFont(fontContenido).SetFontSize(11)));
            //    tabla.AddCell(new Cell().Add(new Paragraph(proveedor.Correo).SetFont(fontContenido).SetFontSize(11)));
            //    tabla.AddCell(new Cell().Add(new Paragraph(proveedor.Telefono).SetFont(fontContenido).SetFontSize(11)));
            //    string estadoTexto = proveedor.Estado == 1 ? "Activo" : "Inactivo";
            //    tabla.AddCell(new Cell().Add(new Paragraph(estadoTexto).SetFont(fontContenido).SetFontSize(11)));
            //}

            // Agregar tabla al documento
            document.Add(tabla);

            PieDePaginaEventHandler pieDePaginaEventHandler = new PieDePaginaEventHandler();

            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, pieDePaginaEventHandler);

            document.Close();

            return RedirectToAction(nameof(Index));
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
