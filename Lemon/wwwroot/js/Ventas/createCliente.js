function validarCliente() {
    clienteSeleccionado = document.getElementById("inputDocumento").value;
    if (clienteSeleccionado == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error no se agrego el cliente',
            text: 'No haz agregado un cliente para seguir con la venta',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    }
    window.location.href = '/Ventums/Create';
}

/*Buscador en tiempo real */
$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var searchText = $(this).val().toLowerCase();
        $('#clientesTable tbody tr').each(function () {
            var idCliente = $(this).find('td:eq(0)').text().toLowerCase();
            var documento = $(this).find('td:eq(1)').text().toLowerCase();
            var nombreRS = $(this).find('td:eq(2)').text().toLowerCase();
            var correo = $(this).find('td:eq(3)').text().toLowerCase();
            var telefono = $(this).find('td:eq(4)').text().toLowerCase();

            if (idCliente.includes(searchText) || documento.includes(searchText) || nombreRS.includes(searchText) || correo.includes(searchText) || telefono.includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});

/*Agregar la informacion del cliente de la tabla a los input */
var botonesSeleccionar = document.querySelectorAll('.btn-seleccionar');
botonesSeleccionar.forEach(function (boton) {
    boton.addEventListener('click', function () {
        // Obtener la fila actual del cliente
        var fila = this.closest('tr');
        // Obtener los valores de la fila actual
        var idCliente = fila.cells[0].innerText;
        var documento = fila.cells[1].innerText;
        var nombre = fila.cells[2].innerText;
        var correo = fila.cells[3].innerText;
        var telefono = fila.cells[4].innerText;


        // Asignar los valores a los inputs en infoCliente
        document.getElementById('inputNombre').value = nombre;
        document.getElementById('inputDocumento').value = documento;
        document.getElementById('inputCorreo').value = correo;
        document.getElementById('inputTelefono').value = telefono;

        $.ajax({
            type: "GET",
            url: "/Ventums/GuardarCliente",
            contentType: "application/json; charset=utf-8",
            data: { id: idCliente },
            success: function (response) {
                console.log(response.message);
            },
            error: function (error) {
                console.error("Error en el metodo ajax", error);
            }
        });
        document.getElementById('progreso').style.background = "#ebebebeb";

        // Cerrar el modal
        var modal = document.getElementById('staticBackdrop');
        var modalBootstrap = bootstrap.Modal.getInstance(modal);
        modalBootstrap.hide();
    });
});