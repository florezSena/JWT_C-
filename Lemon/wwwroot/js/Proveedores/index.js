document.getElementById('buscarProveedor').addEventListener('input', function () {
    var input = this.value.trim().toLowerCase();
    var rows = document.querySelectorAll('.proveedoresPaginado');

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

    var rowsTodos = document.querySelectorAll('.ProveedoresTodos');

    rowsTodos.forEach(function (row) {

        if (input === "") {
            row.style.display = 'none';
        } else {
            var TipoDocumento = row.querySelector('td:nth-child(1)').textContent.trim().toLowerCase();
            var documento = row.querySelector('td:nth-child(2)').textContent.trim().toLowerCase();
            var nombre = row.querySelector('td:nth-child(3)').textContent.trim().toLowerCase();
            var correo = row.querySelector('td:nth-child(4)').textContent.trim().toLowerCase();
            var telefono = row.querySelector('td:nth-child(5)').textContent.trim().toLowerCase();

            row.style.display = (TipoDocumento.includes(input) || documento.includes(input) || nombre.includes(input) || correo.includes(input) || telefono.includes(input)) ? 'table-row' : 'none';
        }
    });
});

function vaciarInput() {

    document.getElementById('buscarProveedor').value = "";
    var icon = document.querySelector('#button-addon2 i');
    icon.className = 'fa-solid fa-magnifying-glass fa-2xl';
    icon.style.color = 'gray';

    var rows = document.querySelectorAll('.proveedoresPaginado');
    rows.forEach(function (row) {
        row.style.display = '';
    });

    var rowsTodos = document.querySelectorAll('.ProveedoresTodos');

    rowsTodos.forEach(function (row) {
        row.style.display = 'none';
    });
}

function eliminarProveedor(proveedor) {
    Swal.fire({
        title: '¿Estas seguro de eliminar este proveedor?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            if (result.isConfirmed) {
                $.ajax({
                    type: "GET",
                    url: "/Proveedores/Delete?id=" + proveedor,// Reemplaza con la URL de tu controlador
                    contentType: "application/json; charset=utf-8",
                    data: { id: proveedor },
                    success: function (response) {
                        // Manejar la respuesta del servidor si es necesario
                        if (response.success) {
                            Swal.fire('Proveedor eliminado con éxito').then((result) => {
                                if (result.isConfirmed) {
                                    var url = "/Proveedores/Index";
                                    window.location.href = url;
                                }
                            });

                        } else {
                            Swal.fire('Un cliente con ventas no se puede eliminar');
                        }
                    },
                    error: function (error) {
                        console.error("Error al eliminar el detalle", error);
                    }
                });
            }
        }
    });

}
function cambiarEstado(checkbox) {
    var nuevoEstado = checkbox.checked ? 1 : 0;
    var titulo = (nuevoEstado === 1) ? '¿Estás seguro de activar a este Proveedor?' : '¿Estás seguro de desactivar a este Proveedor?';


    Swal.fire({
        title: titulo,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire('Estado cambiado con éxito').then((result) => {
                if (result.isConfirmed) {
                    var url = "/Proveedores/CambiarEstado/" + checkbox.id;
                    window.location.href = url;
                }
            });
        } else {
            //Si el usuario hace clic en "Cancelar", restaurar el estado original del checkbox
            checkbox.checked = !checkbox.checked;
        }
    });
}