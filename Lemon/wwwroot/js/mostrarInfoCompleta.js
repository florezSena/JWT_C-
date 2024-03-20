var Correo
var NombreRS
function mostrarInformacionCompleta(td,span) {
    const contenidoCompleto = td.innerText;
    document.getElementById(span).innerText = contenidoCompleto;
    if (span == 'correoModal') {
        document.getElementById('tituloModal').innerText = "Correo:";
        var elemento = document.getElementById('nombreRSModal');
        if (elemento) {
            elemento.innerText = "";
        }
    } else {
        document.getElementById('tituloModal').innerText = "Nombre o Razón social:";
        var elemento = document.getElementById('correoModal');
        if (elemento) {
            elemento.innerText = "";
        }

    }
}