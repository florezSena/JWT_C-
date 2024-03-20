function mostrarOcultarContraseña() {
    inputContraseña = document.getElementById('inputContraseña');
    if (inputContraseña.type === 'password') {
        inputContraseña.type = "text";
        document.getElementById('iconoEye').classList = "fa-solid fa-eye-slash";
    } else {
        inputContraseña.type = "password";
        document.getElementById('iconoEye').classList = "fa-solid fa-eye";
    }
}

function validarCampos(formulario) {
    correo = document.getElementById('inputCorreo').value;
    var regexCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    nombreU = document.getElementById('inputNombreU').value;
    var regexNombreU = /^[A-Za-zñÑ]+(?:\s[A-Za-zñÑ]+)*$/;

    
    contraseña = document.getElementById('inputContraseña').value;
    var regexContraseña = /^(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9\s])(?!.*\s).{8,15}$/;

    if (correo == "" || nombreU == "" || contraseña == "") {
        Swal.fire({
            icon: 'error',
            title: 'Campo vacio',
            text: 'Todos los campos son necesarios',
            timer: 2000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true,
            didClose: () => {
                document.getElementById(formulario).submit();
            }
        })
        return;
    }else if (!regexCorreo.test(correo)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo correo',
            text: 'El correo ingresado no es válido (no debe contener espacios)',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return;

    } else if (nombreU.length < 5 || nombreU.length > 30) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo nombre apellido',
            text: 'El campo nombre apellido debe tener más de 4 caracteres y menos de 31 caracteres',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return;

    } else if (!regexNombreU.test(nombreU)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo nombre apellido',
            text: 'El campo nombre apellido no cuenta con el formato correcto',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return;
    } else if (formulario == "formCreate") {
        if (!regexContraseña.test(contraseña)) {
            Swal.fire({
                icon: 'error',
                title: 'Error en el campo contraseña',
                text: 'La contraseña ingresada no es válida (no debe contener espacios)',
                timer: 3000,
                timerProgressBar: true,
                confirmButtonColor: '#000000',
                showConfirmButton: true
            })
            return;
        }
    }
    
    document.getElementById(formulario).submit();
}
