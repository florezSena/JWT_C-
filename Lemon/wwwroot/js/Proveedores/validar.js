function validarCampos(formulario) {
    documento = document.getElementById('Documento').value;

    nombreRS = document.getElementById('Nombre').value;

    correo = document.getElementById('Correo').value;
    var regexCorreo = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    telefono = document.getElementById('Telefono').value;
    var regexTelefono = /^[0-9]{7,10}$/;

    if (documento == "" || nombreRS == "" || correo == "" || telefono == "") {

        Swal.fire({
            icon: 'error',
            title: 'Campo vacio',
            text: 'Todos los campos son necesarios',
            timer: 2000,
            timerProgressBar: true,
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
            text: 'El campo documento debe tener mas de 5 digitos y menos de 11 digitos',
            timer: 3000,
            timerProgressBar: true,
            showConfirmButton: true
        })
        return;

    } else if (nombreRS.length < 5 || nombreRS.length > 20) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo nombre o razon social',
            text: 'El campo nombre o razon social debe tener mas de 4 digitos y menos de 20 digitos',
            timer: 3000,
            timerProgressBar: true,
            showConfirmButton: true
        })
        return;

    } else if (!regexCorreo.test(correo)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo correo',
            text: 'El correo ingresado no es valido',
            timer: 3000,
            timerProgressBar: true,
            showConfirmButton: true
        })
        return;

    } else if (!regexTelefono.test(telefono)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo telefono',
            text: 'El telefono debe contener solo numeros y tener entre 7 y 10 digitos',
            timer: 3000,
            timerProgressBar: true,
            showConfirmButton: true
        })
        return;

    }

    document.getElementById(formulario).submit();
}