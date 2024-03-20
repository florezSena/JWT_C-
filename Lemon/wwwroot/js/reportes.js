function generarPdf(controlador, modulo) {
    var ruta = "/" + controlador + "/GenerarPdf"
    var rutaPdf = '/Reporte'+modulo+'.pdf'
    $.ajax({
        type: "GET",
        url: ruta,
        contentType: "application/json; charset=utf-8",
        success: function (response) {
            if (response.success) {
                window.open(rutaPdf, '_blank');
            } 
        },
        error: function (error) {
            console.error("Error en el metodo ajax", error);
        }
    });
}