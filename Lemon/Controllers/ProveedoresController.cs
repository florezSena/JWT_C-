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
using X.PagedList;

namespace Lemon.Controllers
{
    public class ProveedoresController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly IApiClient _client;

        //para poder redirigir el pdf cuando lo guardemos
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProveedoresController(IApiClient client, IWebHostEnvironment hostingEnvironment)
        {
            _client = client;
            _hostEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Número de elementos por página
            int pageNumber = page ?? 1;

            var pro = await _client.GetProveedorAsync();

            var pagedProveedores = await pro.ToPagedListAsync(pageNumber, pageSize);

            ViewData["Proveedores"] = pro;
            return View(pagedProveedores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                var proveedores = await _client.GetProveedorAsync();

                var docExistente = proveedores.Any(p => p.Documento == proveedor.Documento);

                var CorreoExistente = proveedores.Any(p => p.Correo == proveedor.Correo);

                var telExistente = proveedores.Any(p => p.Telefono == proveedor.Telefono);

                if (docExistente)
                {
                    ModelState.AddModelError(string.Empty, "el número de Documento ya está registrado");
                    return View();
                }
                else if (CorreoExistente)
                {
                    ModelState.AddModelError(string.Empty, "el correo electrónico ya está registrado");
                    return View();
                }
                else if (telExistente)
                {
                    ModelState.AddModelError(string.Empty, "el teléfono ya está registrado");
                    return View();
                }

                var response = await _client.CreateProveedorAsync(proveedor);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la creacion del proveedor");
                    return View();
                }
            }
            return View();
        }


        public async Task<IActionResult> Update(int id)
        {
            var artic = await _client.FindProveedorAsync(id);
            return View(artic);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.UpdateProveedorAsync(proveedor);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la creacion del proveedor");
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var artic = await _client.DeleteProveedorAsync(id);
            if (artic == null)
            {
                return NotFound();
            }

            return Json(new { success = true, message = "Cliente eliminado exitosamente" });
        }

        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Obtener el proveedor con el ID especificado
            var proveedor = await _client.FindProveedorAsync(id.Value);
            if (proveedor == null)
            {
                return NotFound();
            }

            // Cambiar el estado del proveedor
            proveedor.Estado = (proveedor.Estado == 1) ? 0 : 1;

            // Actualizar el proveedor utilizando el cliente de API
            var response = await _client.UpdateProveedorAsync(proveedor);

            if (response.IsSuccessStatusCode)
            {
                // El estado del proveedor se cambió correctamente
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Si la actualización falla, manejar el error según sea necesario
                ModelState.AddModelError(string.Empty, "Error al cambiar el estado del proveedor");
                return View();
            }
        }

        //instalar iText7 (7.1.10)
        public async Task<IActionResult> GenerarPdf()
        {
            // Obtener los datos de proveedores desde la API
            var proveedores = await _client.GetProveedorAsync();

            // Generar el PDF
            PdfWriter pdfWriter = new PdfWriter(System.IO.Path.Combine(_hostEnvironment.WebRootPath, "ReporteProveedores.pdf"));
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
            foreach (var proveedor in proveedores)
            {
                tabla.AddCell(new Cell().Add(new Paragraph(proveedor.TipoDocumento).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(proveedor.Documento.ToString()).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(proveedor.NombreRazonSocial).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(proveedor.Correo).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(proveedor.Telefono).SetFont(fontContenido).SetFontSize(11)));
                string estadoTexto = proveedor.Estado == 1 ? "Activo" : "Inactivo";
                tabla.AddCell(new Cell().Add(new Paragraph(estadoTexto).SetFont(fontContenido).SetFontSize(11)));
            }

            // Agregar tabla al documento
            document.Add(tabla);

            PieDePaginaEventHandler pieDePaginaEventHandler = new PieDePaginaEventHandler();

            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, pieDePaginaEventHandler);

            document.Close();

            return File("/ReporteProveedores.pdf", "application/pdf");
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