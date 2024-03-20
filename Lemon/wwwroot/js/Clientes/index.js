document.getElementById('buscarCliente').addEventListener('input', function () {
    var input = this.value.trim().toLowerCase();
    var rows = document.querySelectorAll('.clientesPaginado');

    if (input === "") {
        rows.forEach(function (row) {
            row.style.display = '';
        });
        var icon = document.querySelector('#button-addon2 i');
        icon.className = 'fa-solid fa-magnifying-glass fa-2xl';
        icon.style.color = 'gray';

    } else {
        rows.forEach(function (row) {
            row.style.display = 'none';
        });
        var icon = document.querySelector('#button-addon2 i');
        icon.className = 'fa-solid fa-x fa-2xl';
        icon.style.color = 'gray';
    }

    var rowsTodos = document.querySelectorAll('.clientesTodos');

    rowsTodos.forEach(function (row) {

        if (input === "") {
            row.style.display = 'none';
        } else {
            var tipoDoc = row.querySelector('td:nth-child(1)').textContent.trim().toLowerCase();
            var documento = row.querySelector('td:nth-child(2)').textContent.trim().toLowerCase();
            var nombre = row.querySelector('td:nth-child(3)').textContent.trim().toLowerCase();
            var correo = row.querySelector('td:nth-child(4)').textContent.trim().toLowerCase();
            var telefono = row.querySelector('td:nth-child(5)').textContent.trim().toLowerCase();
            row.style.display = (documento.includes(input) || nombre.includes(input) || tipoDoc.includes(input) || correo.includes(input) || telefono.includes(input)) ? 'table-row' : 'none';
        }
    });
});

function vaciarInput() {

    document.getElementById('buscarCliente').value = "";
    var icon = document.querySelector('#button-addon2 i');
    icon.className = 'fa-solid fa-magnifying-glass fa-2xl';
    icon.style.color = 'gray';

    var rows = document.querySelectorAll('.clientesPaginado');
    rows.forEach(function (row) {
        row.style.display = '';
    });

    var rowsTodos = document.querySelectorAll('.clientesTodos');

    rowsTodos.forEach(function (row) {
        row.style.display = 'none';
    });
}

function eliminarCliente(cliente) {
    Swal.fire({
        text: '¿Estás seguro de eliminar este cliente?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#077336',
        cancelButtonColor: '#000000',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            var urlEliminar = "/Clientes/Delete?id=" + cliente;
            $.ajax({
                type: "GET",
                url: urlEliminar, // Reemplaza con la URL de tu controlador
                contentType: "application/json; charset=utf-8",
                data: { id: cliente },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            text: 'Cliente eliminado con éxito',
                            confirmButtonColor: '#000000',
                            showConfirmButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                var url = "/Clientes/Index";
                                window.location.href = url;
                            }
                        });
                    } else {
                        if (response.message == "Cliente con ventas") {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error al eliminar el cliente',
                                text: 'Un cliente con ventas no se puede eliminar',
                                confirmButtonColor: '#000000',
                                showConfirmButton: true
                            })
                        } else if (response.message == "Cliente inhabilitado no se puede eliminar") {
                            Swal.fire({
                                icon: 'error',
                                text: 'Error al eliminar el cliente',
                                text: 'Estás intentando eliminar un cliente inhabilitado lo cual no se puede',
                                confirmButtonColor: '#000000',
                                showConfirmButton: true
                            })
                        }
                        
                    }
                },
                error: function (error) {
                    console.error("Error al eliminar el cliente", error);
                }
            });
        }
    });
}
function cambiarEstado(checkbox) {
    var nuevoEstado = checkbox.checked ? 1 : 0;
    var titulo = (nuevoEstado === 1) ? '¿Estás seguro de activar a este cliente?' : '¿Estás seguro de desactivar a este cliente?';

    Swal.fire({
        text: titulo,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#077336',
        cancelButtonColor: '#000000',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                icon: 'success',
                text: 'Estado cambiado con éxito',
                showConfirmButton: true,
                confirmButtonColor: '#000000'
            }).then((result) => {
                if (result.isConfirmed) {
                    var url = "/Clientes/CambiarEstado/" + checkbox.id;
                    window.location.href = url;
                }
            });
        } else {
            // Si el usuario hace clic en "Cancelar", restaurar el estado original del checkbox
            checkbox.checked = !checkbox.checked;
        }
    });
}