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
using X.PagedList;

namespace Lemon.Controllers
{
    public class UsuariosController : Controller
    {
        public readonly IApiClient _client;

        //Para poder redirigir al pdf cuando guarde el pdf

        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsuariosController(IApiClient client, IWebHostEnvironment hostingEnvironment)
        {
            _client = client;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 5; // Número de elementos por página
            int pageNumber = page ?? 1;

            var usuarios = await _client.GetUsuariosAsync();

            var pagedUsuarios = await usuarios.ToPagedListAsync(pageNumber, pageSize);

            ViewData["Usuarios"] = usuarios;
            return View(pagedUsuarios);
        }

        public async Task<IActionResult> Create()
        {
            var roles = await _client.GetRolesAsync();

            var rolesFiltrados = roles.Where(p => p.Estado == 1).ToList();

            ViewData["Roles"] = rolesFiltrados;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            var roles = await _client.GetRolesAsync();

            var rolesFiltrados = roles.Where(p => p.Estado == 1).ToList();

            ViewData["Roles"] = rolesFiltrados;

            if (ModelState.IsValid)
            {
                var usuarios = await _client.GetUsuariosAsync();


                var existingCorreo = usuarios.Any(c => c.Correo == usuario.Correo);


                if (existingCorreo)
                {
                    ModelState.AddModelError(string.Empty, "El correo electrónico ya está registrado");
                    ViewData["CorreoRegistrado"] = true;
                    return View();
                }

                usuario.SetPassword(usuario.Contraseña);
                var response = await _client.CreateUsuarioAsync(usuario);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    TempData["SuccessMessage"] = "Usuario creado exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la creacion del usuario");
                    return View(usuario);
                }
            }
            return View(usuario);
        }

        public async Task<IActionResult> Update(int id)
        {
            var usuario = await _client.FindUsuarioAsync(id);
            if (usuario.Estado == 1)
            {
                var roles = await _client.GetRolesAsync();

                var rolesFiltrados = roles.Where(p => p.Estado == 1).ToList();

                ViewData["Roles"] = rolesFiltrados;
                return View(usuario);
            }
            //No se puede hacer esta accion con los clientes inhabilitados
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Update(UsuarioUpdate usuario)
        {
            var roles = await _client.GetRolesAsync();

            var rolesFiltrados = roles.Where(p => p.Estado == 1).ToList();

            ViewData["Roles"] = rolesFiltrados;
            if (ModelState.IsValid)
            {
                var usuarios = await _client.GetUsuariosAsync();

                var existingCorreo = usuarios.Any(c => c.Correo == usuario.Correo && c.IdUsuario != usuario.IdUsuario);

                if (existingCorreo)
                {
                    ModelState.AddModelError(string.Empty, "El correo electrónico ya está registrado");
                    ViewData["CorreoRegistrado"] = true;
                    return View();
                }

                var response = await _client.UpdateUsuarioAsync(usuario);

                if (response.IsSuccessStatusCode)
                {
                    //La solicitud Post fue exitosa
                    TempData["SuccessMessage"] = "Usuario actualizado exitosamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    //La solicitud POST falló, manejar el error segun sea necesario
                    ModelState.AddModelError(string.Empty, "Error en la actualizacion del usuario");
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = await _client.FindUsuarioAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            if (usuario.Estado == 2)
            {
                //No se le puede cambiar el estado a un usuario recien creado
                return RedirectToAction(nameof(Index));
            }

            usuario.Estado = usuario.Estado == 1 ? 0 : 1;

            var response = await _client.UpdateUsuarioAsync(usuario);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _client.FindUsuarioAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            else if (usuario.Estado == 0)
            {
                //No se puede hacer esta accion con los usuarios inhabilitados
                return Json(new { success = false, message = "Usuario inhabilitado no se puede eliminar" });
            }

            //var ventas = await _client.GetVentasAsync();

            //var existingVentaCliente = ventas.Any(c => c.IdCliente == cliente.IdCliente);

            //if (existingVentaCliente)
            //{
            //    return Json(new { success = false, message = "Cliente con ventas" });
            //}

            await _client.DeleteUsuarioAsync(usuario.IdUsuario);

            return Json(new { success = true, message = "Usuario eliminado exitosamente" });
        }

        [HttpGet]
        public async Task<IActionResult> CambiarContraseña(int id,string password)
        {
            var usuario = await _client.FindUsuarioAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            else if (usuario.Estado == 0)
            {
                //No se puede hacer esta accion con los usuarios inhabilitados
                return Json(new { success = false, message = "Usuario inhabilitado no se puede modificar" });
            }

            usuario.SetPassword(password);

            await _client.UpdateUsuarioAsync(usuario);

            return Json(new { success = true, message = "Contraseña cambiada exitosamente" });
        }


        public async Task<IActionResult> GenerarPdf()
        {
            // Obtener los datos de usuarios desde la API
            var usuarios = await _client.GetUsuariosAsync();

            // Generar el PDF
            //Aqui le estamos diciendo que lo mande al wwwroot para poder acceder a el al momento de querer redirigirlo al pdf
            PdfWriter pdfWriter = new PdfWriter(System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "ReporteUsuarios.pdf"));
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
            document.Add(new Paragraph("Reporte de Usuarios").SetTextAlignment(TextAlignment.CENTER).SetFontSize(16).SetFont(fontContenidoTitulo));
            document.Add(new Paragraph("\n"));
            // **Tabla de proveedores**

            // Definir fuentes
            PdfFont fontColumnas = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont fontContenido = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            // Columnas
            string[] columnas = {"#", "Nombre apellido", "Correo",  "Rol", "Estado" };

            // Tamaños de las columnas
            float[] tamanios = { 1,5, 4, 3, 1 };

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
            foreach (var usuario in usuarios)
            {
                tabla.AddCell(new Cell().Add(new Paragraph(usuario.IdUsuario.ToString()).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(usuario.NombreUsuario.ToString()).SetFont(fontContenido).SetFontSize(11)));
                tabla.AddCell(new Cell().Add(new Paragraph(usuario.Correo).SetFont(fontContenido).SetFontSize(11).SetMaxWidth(200f)));
                if (usuario.IdRolNavigation != null)
                {
                    tabla.AddCell(new Cell().Add(new Paragraph(usuario.IdRolNavigation.Nombre).SetFont(fontContenido).SetFontSize(11)));
                }
                else
                {
                    // Agregar una celda vacía o un valor predeterminado en caso de que IdRolNavigation sea nulo
                    tabla.AddCell(new Cell().Add(new Paragraph("Rol no disponible").SetFont(fontContenido).SetFontSize(11)));
                }
                string estadoTexto;
                switch (usuario.Estado)
                {
                    case 0:
                        estadoTexto = "Inactivo";
                        break;
                    case 1:
                        estadoTexto = "Activo";
                        break;
                    case 2:
                        estadoTexto = "Nuevo";
                        break;
                    default:
                        estadoTexto = "Desconocido";
                        break;
                }
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
