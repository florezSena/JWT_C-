document.getElementById('buscarRol').addEventListener('input', function () {
    var input = this.value.trim().toLowerCase();
    var rows = document.querySelectorAll('.RolesPaginado');

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

    var rowsTodos = document.querySelectorAll('.RolesTodos');

    rowsTodos.forEach(function (row) {

        if (input === "") {
            row.style.display = 'none';
        } else {
            var idRol = row.querySelector('td:nth-child(1)').textContent.trim().toLowerCase();
            var nombre = row.querySelector('td:nth-child(2)').textContent.trim().toLowerCase();
            var estado = row.querySelector('td:nth-child(3)').textContent.trim().toLowerCase();

            row.style.display = (idRol.includes(input) || nombre.includes(input) || estado.includes(input) ) ? 'table-row' : 'none';
        }
    });
});

function vaciarInput() {

    document.getElementById('buscarRol').value = "";
    var icon = document.querySelector('#button-addon2 i');
    icon.className = 'fa-solid fa-magnifying-glass fa-2xl';
    icon.style.color = 'gray';

    var rows = document.querySelectorAll('.RolesPaginado');
    rows.forEach(function (row) {
        row.style.display = '';
    });

    var rowsTodos = document.querySelectorAll('.RolesTodos');

    rowsTodos.forEach(function (row) {
        row.style.display = 'none';
    });
}

function eliminarRoles(roles) {
    Swal.fire({
        text: '¿Estas seguro de eliminar este rol?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#077336',
        cancelButtonColor: '#000000',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "GET",
                url: "/Roles/Delete?id=" + roles,// Reemplaza con la URL de tu controlador
                contentType: "application/json; charset=utf-8",
                data: { id: roles },
                success: function (response) {
                    // Manejar la respuesta del servidor si es necesario
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            text: 'Rol eliminado con exito',
                            confirmButtonColor: '#000000',
                            showConfirmButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                var url = "/Roles/Index";
                                window.location.href = url;
                            }
                        });
                    } else {
                        if (response.message == "Rol con permisos") {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error al eliminar el rol',
                                text: 'Un rol con permisos no se puede eliminar',
                                confirmButtonColor: '#000000',
                                showConfirmButton: true
                            })
                        } else if (response.message == "Rol inhabilitado no se puede eliminar") {
                            Swal.fire({
                                icon: 'error',
                                text: 'Error al eliminar el rol',
                                text: 'Estás intentando eliminar un rol inhabilitado lo cual no se puede',
                                confirmButtonColor: '#000000',
                                showConfirmButton: true
                            })
                        }
                    }
                },
                error: function (error) {
                    console.error("Error al eliminar el rol", error);
                }
            });
        }
    });

}
function cambiarEstado(checkbox) {
    var nuevoEstado = checkbox.checked ? 1 : 0;
    var titulo = (nuevoEstado === 1) ? '¿Estás seguro de activar a este Rol?' : '¿Estás seguro de desactivar a este Rol?';

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
                text: 'Estado cambiado con exito',
                showConfirmButton: true,
                confirmButtonColor: '#000000'
            }).then((result) => {
                if (result.isConfirmed) {
                    var url = "/Roles/CambiarEstado/" + checkbox.id;
                    window.location.href = url;
                }
            });
        } else {
            //Si el usuario hace clic en "Cancelar", restaurar el estado original del checkbox
            checkbox.checked = !checkbox.checked;
        }
    });
}