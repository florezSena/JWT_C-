totalInicial = document.getElementById("inputTotal").value;
const totalFormateado = (parseFloat(totalInicial)).toLocaleString('es-ES', {
    minimumFractionDigits: 2,
    useGrouping: true,
    grouping: '.'
});
var totalFormatedoInicial= document.getElementById("inputTotalMostrar").value = "$"+totalFormateado;

function progresoTotal() {
    var descuento = document.getElementById("inputDescuento").value;
    const regex = /^\d+$/;
    if (descuento == 0 || descuento=="") {
        document.getElementById("inputTotal").value = totalInicial;
        document.getElementById("validarDescuento").innerText = "";
        document.getElementById("inputTotalMostrar").value = totalFormatedoInicial;
        return
    } else if (descuento < 0 || descuento > 100 || !regex.test(descuento)) {
        document.getElementById("inputTotal").value = totalInicial;
        document.getElementById("inputTotalMostrar").value = totalFormatedoInicial;
        document.getElementById("validarDescuento").innerText = "Descuento erroneo";
        return
    }
    document.getElementById("validarDescuento").innerText = "";
    totalDescontar = 1 - (descuento / 100)
    const totalFormateado = (parseFloat((totalInicial * totalDescontar).toFixed(2))).toLocaleString('es-ES', {
        minimumFractionDigits: 2,
        useGrouping: true,
        grouping: '.'
    });
    document.getElementById("inputTotalMostrar").value = "$"+totalFormateado;

    document.getElementById("inputTotal").value = (totalInicial * totalDescontar).toFixed(2);
}

function validarProgreso() {
    var valor = document.getElementById("inputTotal").value;
    if (valor == "") {
        document.getElementById('progreso2').style.background = "";
    } else {
        document.getElementById('progreso2').style.background = "#ebebebeb";
    }
}

/*Traer la fecha de hoy */
// Obtener la fecha y hora actual en la zona horaria de Colombia
const fechaHoraActual = new Date();
const timeZoneOffset = -5 * 60; // UTC-5 para Colombia
const fechaHoraColombia = new Date(fechaHoraActual.getTime() + timeZoneOffset * 60 * 1000);

// Formatear la fecha y hora en el formato requerido por el input de tipo datetime-local
const formatoFechaHora = fechaHoraColombia.toISOString().slice(0, 16); // Tomar solo los primeros 16 caracteres

// Asignar el valor al input
document.getElementById("fechaInput").value = formatoFechaHora;

function validarVenta() {
    var total = document.getElementById('inputTotal').value;
    var fecha = document.getElementById('fechaInput').value;
    var idCliente = document.getElementById('inputIdCliente').value;

    if (total.includes(',')) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo total',
            text: 'Se ha detectado una , como separador decimal en vez de un .',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    } else if (total == "" || total <= 0) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo total',
            text: 'El valor total no puede ser menor o igual a 0',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    }

    var venta = {
        IdCliente: idCliente,
        Fecha: fecha,
        Total: total,
        Estado: 1
    };

    $.ajax({
        type: "GET",
        url: "/Ventums/CreateAJAX",
        contentType: "application/json; charset=utf-8",
        data: venta,
        success: function (response) {
            console.log(response.message);
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Venta exitosa',
                    text: 'La venta se ha realizado exitosamente',
                    timer: 2000,
                    timerProgressBar: true,
                    showConfirmButton: false
                }).then((result) => {
                    window.location.href = "/Ventums/Index";
                });
            } else if (response.message == "No puedes crear una venta sin detalles") {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'No puedes crear una venta sin detalles',
                    timer: 2000,
                    timerProgressBar: true,
                    showConfirmButton: false
                }).then((result) => {
                    window.location.href = "/Ventums/Index";
                });
            } else if (response.message == "Error en el modelo" || response.message == "Error en la creacion de la venta") {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'No se pudo registrar la venta revisa tener informacion correcta en todos los campos',
                    timer: 2000,
                    timerProgressBar: true,
                    showConfirmButton: false
                })
            }

        },
        error: function (error) {
            console.error("Error en el metodo ajax", error);
        }
    });

}