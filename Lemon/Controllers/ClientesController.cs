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
using Microsoft.EntityFrameworkCore;
using System.Net;
using X.PagedList;
 
namespace Lemon.Controllers
{
    public class ClientesController : Controller
    {
        public readonly IApiClient _client;

        //Para poder redirigir al pdf cuando guarde el pdf

        private readonly IWebHostEnvironment _hostingEnvironment;

        public ClientesController(IApiClient client, IWebHostEnvironment hostingEnvironment)
        {
            _client = client;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Número de elementos por página
            int pageNumber = page ?? 1;

            

            try
            {
                var clientes = await _client.GetClientesAsync();
                var pagedClientes = await clientes.ToPagedListAsync(pageNumber, pageSize);

                ViewData["Clientes"] = clientes;
                return View(pagedClientes);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
            {
                // Aquí manejas la excepción cuando el código de estado es 403 Forbidden
                return RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException ex)
            {
                // Maneja otras excepciones de HttpRequestException aquí
                // Puedes mostrar un mensaje genérico de error o redirigir a otra vista
                return View("ErrorView");
            }

            
            //var clientes = await _client.GetClientesAsync();
            //return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Cliente cliente)
        {
           
            if (ModelState.IsValid)
            {
                var clientes = await _client.GetClientesAsync();

                var existingDocumento = clientes.Any(c => c.Documento == cliente.Documento);

                var existingCorreo = clientes.Any(c => c.Correo == cliente.Correo);

                var existingTelefono = clientes.Any(c => c.Telefono == cliente.Telefono);


                if (existingDocumento)
                {
                    ModelState.AddModelError(string.Empty, "El nuemro de documento ya está registrado");
                    ViewData["DocumentoRegistrado"] = true;
                    return View();
                }else if (existingCorreo)
                {
                    ModelState.AddModelError(string.Empty, "El correo electrónico ya está registrado");
                    ViewData["CorreoRegistrado"] = true;
                    return View();
                }
                else if (existingTelefono)
                {
                    ModelState.AddModelError(string.Empty, "El telefono ya está registrado");
                    ViewData["TelefonoRegistrado"] = true;
                    return View();
                }

                var response = await _client.CreateClienteAsync(cliente);

                if (response.IsSuccessStatusCode)
                {

                    //La solicitud Post fue exitosa
                    TempData["SuccessMessage"] = "Cliente creado exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la creacion del cliente");
                    return View();
                }
            }
            return View();
        }


        public async Task<IActionResult> Update(int id)
        {
            var cliente = await _client.FindClienteAsync(id);
            if (cliente.Estado == 1)
            {
                return View(cliente);
            }
            //No se puede hacer esta accion con los clientes inhabilitados
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Update(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var clientes = await _client.GetClientesAsync();

                //var existingDocumento = clientes.Any(c => c.Documento == cliente.Documento);       se pone el && c.IdCliente != cliente.IdCliente para que no valide el mismo documento del cliente
                var existingDocumento = clientes.Any(c => c.Documento == cliente.Documento && c.IdCliente != cliente.IdCliente);

                var existingCorreo = clientes.Any(c => c.Correo == cliente.Correo && c.IdCliente != cliente.IdCliente);

                var existingTelefono = clientes.Any(c => c.Telefono == cliente.Telefono && c.IdCliente != cliente.IdCliente);


                if (existingDocumento)
                {
                    ModelState.AddModelError(string.Empty, "El nuemro de documento ya está registrado");
                    ViewData["DocumentoRegistrado"] = true;
                    return View();
                }
                else if (existingCorreo)
                {
                    ModelState.AddModelError(string.Empty, "El correo electrónico ya está registrado");
                    ViewData["CorreoRegistrado"] = true;
                    return View();
                }
                else if (existingTelefono)
                {
                    ModelState.AddModelError(string.Empty, "El telefono ya está registrado");
                    ViewData["TelefonoRegistrado"] = true;
                    return View();
                }


                var response = await _client.UpdateClienteAsync(cliente);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la actualizacion del cliente");
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _client.FindClienteAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }else if (cliente.Estado == 0)
            {
                //No se puede hacer esta accion con los clientes inhabilitados
                return Json(new { success = false, message = "Cliente inhabilitado no se puede eliminar" });
            }

            var ventas = await _client.GetVentasAsync();

            var existingVentaCliente = ventas.Any(c => c.IdCliente == cliente.IdCliente);

            if (existingVentaCliente)
            {
                return Json(new { success = false, message = "Cliente con ventas" });
            }

            await _client.DeleteClienteAsync(cliente.IdCliente);
            
            return Json(new { success = true, message = "Cliente eliminado exitosamente" });
        }

        public async Task<IActionResult> CambiarEstado(int id)
        {
            var cliente = await _client.FindClienteAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            cliente.Estado = cliente.Estado==1 ? 0:1;

            var response = await _client.UpdateClienteAsync(cliente);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> GenerarPdf()
        {
            // Obtener los datos de proveedores desde la API
            var clientes = await _client.GetClientesAsync();

            // Generar el PDF
            //Aqui le estamos diciendo que lo mande al wwwroot para poder acceder a el al momento de querer redirigirlo al pdf
            PdfWriter pdfWriter = new PdfWriter(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "ReporteClientes.pdf"));
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
            document.Add(new Paragraph("Reporte de Clientes").SetTextAlignment(TextAlignment.CENTER).SetFontSize(16).SetFont(fontContenidoTitulo));
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
            foreach (var cliente in clientes)
            {
                tabla.AddCell(new Cell().Add(new Paragraph(cliente.TipoDocumento).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(cliente.Documento.ToString()).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(cliente.NombreRazonSocial).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(cliente.Correo).SetFont(fontContenido).SetFontSize(11).SetMaxWidth(200f)));
                tabla.AddCell(new Cell().Add(new Paragraph(cliente.Telefono).SetFont(fontContenido).SetFontSize(11)));
                string estadoTexto = cliente.Estado == 1 ? "Activo" : "Inactivo";
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
