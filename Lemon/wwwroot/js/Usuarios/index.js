var userId

function mostrarOcultarContraseña(input,icono) {
    inputContraseña = document.getElementById(input);

    if (inputContraseña.type === 'password') {
        inputContraseña.type = "text";
        document.getElementById(icono).classList = "fa-solid fa-eye-slash";
    } else {
        inputContraseña.type = "password";
        document.getElementById(icono).classList = "fa-solid fa-eye";
    }
}

function vaciarDatos() {
    document.getElementById('inputContraseñaNueva').value="";
    document.getElementById('inputConfirmarContraseña').value="";
}

function obtenerDatos(id,correo) {
    userId = id;
    userCorreo = correo;

    $('#staticBackdropLabel').text('Usuario id: '+id);
    $('#correoCambiar').text("Correo: "+userCorreo);

    // Puedes hacer más acciones aquí para cargar otros datos del usuario si es necesario
}
function mostrarOcultarValidacionNueva() {
    texto = document.getElementById('inputContraseñaNueva').value;
    var regexContraseña = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9\s])(?!.*\s).{8,15}$/;
    if (regexContraseña.test(texto)) {
        document.getElementById('textoValidarContraseñaNueva').innerHTML = "";
    }
}
function mostrarOcultarValidacionConfirmacion() {
    contraseña = document.getElementById('inputContraseñaNueva').value;
    texto = document.getElementById('inputConfirmarContraseña').value;
    
    if (contraseña==texto) {
        document.getElementById('textoValidarConfirmarContraseña').innerHTML = "";
    }
}
function validarCambiarContraseña() {
    var contraseñaNueva = document.getElementById('inputContraseñaNueva').value;
    var confirmarContraseña = document.getElementById('inputConfirmarContraseña').value;
    var regexContraseña = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9\s])(?!.*\s).{8,15}$/;

    if (!regexContraseña.test(contraseñaNueva)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo contraseña',
            text: 'La contraseña ingresada no es válida',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        document.getElementById('textoValidarContraseñaNueva').innerHTML = "No debe contener espacios debe tener entre 8 y 15 caracteres con al menos 1 mayuscula 1 numero y 1 caracter especial";
        return;
    } else if (contraseñaNueva != confirmarContraseña) {
        Swal.fire({
            icon: 'error',
            title: 'Error en las contraseñas',
            text: 'Las contraseñas no coinciden',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        document.getElementById('textoValidarConfirmarContraseña').innerHTML = "Las contrasñeas no coinciden";

        return;
    }

    $.ajax({
        type: "GET",
        url: "Usuarios/CambiarContraseña", 
        contentType: "application/json; charset=utf-8",
        data: { id: userId, password: contraseñaNueva},
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    icon: 'success',
                    text: 'Contraseña cambiada con éxito',
                    confirmButtonColor: '#000000',
                    showConfirmButton: true
                })
                document.getElementById('btnCancelarModal').click()
            } else {
                if (response.message == "Usuario inhabilitado no se puede modificar") {
                    Swal.fire({
                        icon: 'error',
                        text: 'Error al cambiar la contraseña',
                        text: 'Estás intentando cambiarle la contraseña a un usuario inhabilitado lo cual no se puede',
                        confirmButtonColor: '#000000',
                        showConfirmButton: true
                    })
                    document.getElementById('btnCancelarModal').click()
                }

            }
        },
        error: function (error) {
            console.error("Error al cambiar la contraseña del usuario", error);
        }
    });
}

//Buscador en tiempo real
document.getElementById('buscarUsuario').addEventListener('input', function () {
    var input = this.value.trim().toLowerCase();
    var rows = document.querySelectorAll('.usuariosPaginado');

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

    var rowsTodos = document.querySelectorAll('.usuariosTodos');

    rowsTodos.forEach(function (row) {

        if (input === "") {
            row.style.display = 'none';
        } else {
            var idUsuario = row.querySelector('td:nth-child(1)').textContent.trim().toLowerCase();
            var correo = row.querySelector('td:nth-child(2)').textContent.trim().toLowerCase();
            var nombreU = row.querySelector('td:nth-child(3)').textContent.trim().toLowerCase();
            var rol = row.querySelector('td:nth-child(4)').textContent.trim().toLowerCase();
            var estado = row.querySelector('td:nth-child(5)').textContent.trim().toLowerCase();
            row.style.display = (idUsuario.includes(input) || correo.includes(input) || nombreU.includes(input) || rol.includes(input) || estado.includes(input)) ? 'table-row' : 'none';
        }
    });
});

function vaciarInput() {

    document.getElementById('buscarUsuario').value = "";
    var icon = document.querySelector('#button-addon2 i');
    icon.className = 'fa-solid fa-magnifying-glass fa-2xl';
    icon.style.color = 'gray';

    var rows = document.querySelectorAll('.usuariosPaginado');
    rows.forEach(function (row) {
        row.style.display = '';
    });

    var rowsTodos = document.querySelectorAll('.usuariosTodos');

    rowsTodos.forEach(function (row) {
        row.style.display = 'none';
    });
}

function eliminarUsuario(usuario) {
    Swal.fire({
        text: '¿Estás seguro de eliminar este usuario?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#077336',
        cancelButtonColor: '#000000',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            var urlEliminar = "/Usuarios/Delete?id=" + usuario;
            $.ajax({
                type: "GET",
                url: urlEliminar, // Reemplaza con la URL de tu controlador
                contentType: "application/json; charset=utf-8",
                data: { id: usuario },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            text: 'Usuario eliminado con éxito',
                            confirmButtonColor: '#000000',
                            showConfirmButton: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                var url = "/Usuarios/Index";
                                window.location.href = url;
                            }
                        });
                    } else {
                        //if (response.message == "Cliente con ventas") {
                        //    Swal.fire({
                        //        icon: 'error',
                        //        title: 'Error al eliminar el cliente',
                        //        text: 'Un cliente con ventas no se puede eliminar',
                        //        confirmButtonColor: '#000000',
                        //        showConfirmButton: true
                        //    })
                        //} else
                            if (response.message == "Usuario inhabilitado no se puede eliminar") {
                            Swal.fire({
                                icon: 'error',
                                text: 'Error al eliminar el usuario',
                                text: 'Estás intentando eliminar un usuario inhabilitado lo cual no se puede',
                                confirmButtonColor: '#000000',
                                showConfirmButton: true
                            })
                        }
                        
                    }
                },
                error: function (error) {
                    console.error("Error al eliminar el usuario", error);
                }
            });
        }
    });
}
function cambiarEstado(checkbox) {
    var nuevoEstado = checkbox.checked ? 1 : 0;
    var titulo = (nuevoEstado === 1) ? '¿Estás seguro de activar a este usuario?' : '¿Estás seguro de desactivar a este usuario?';

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
                    var url = "/Usuarios/CambiarEstado/" + checkbox.id;
                    window.location.href = url;
                }
            });
        } else {
            // Si el usuario hace clic en "Cancelar", restaurar el estado original del checkbox
            checkbox.checked = !checkbox.checked;
        }
    });
}