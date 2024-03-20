function validarCampos(formulario) {
    documento = document.getElementById('inputDocumento').value;

    nombreRS = document.getElementById('inputNombreRS').value;
    var regexNombreRS = /^[A-Za-zñÑ]+(?:\s[A-Za-zñÑ]+)*$/;

    correo = document.getElementById('inputCorreo').value;
    var regexCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    telefono = document.getElementById('inputTelefono').value;
    var regexTelefono = /^(?!(?:[^0]*0){4,})[1-9][0-9]{6,9}$/;
    
    if (documento == "" || nombreRS == "" || correo == "" || telefono == "") {
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
    } else if (documento.length < 6 || documento.length > 10) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo documento',
            text: 'El campo documento debe tener más de 5 dígitos y menos de 11 dígitos',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return;

    } else if (nombreRS.length < 5 || nombreRS.length > 100) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo nombre o razón social',
            text: 'El campo nombre o razón social debe tener más de 4 caracteres y menos de 100 caracteres',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return;

    } else if (!regexNombreRS.test(nombreRS)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo nombre o razón social',
            text: 'El campo nombre o razón social no cuenta con el formato correcto',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return;
    } else if (!regexCorreo.test(correo)) {
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

    } else if (!regexTelefono.test(telefono)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo teléfono',
            text: 'Error en el campo teléfono (no espacios ni caracteres especiales)',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return;

    }
    
    document.getElementById(formulario).submit();
}
