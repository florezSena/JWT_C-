/*Buscador en tiempo real */
var costoInicial;
$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var searchText = $(this).val().toLowerCase();
        $('#productosTable tbody tr').each(function () {
            var idProducto = $(this).find('td:eq(0)').text().toLowerCase();
            var nombre = $(this).find('td:eq(1)').text().toLowerCase();
            var cantidad = $(this).find('td:eq(2)').text().toLowerCase();
            var descripcion = $(this).find('td:eq(3)').text().toLowerCase();
            var costo = $(this).find('td:eq(4)').text().toLowerCase();

            if (idProducto.includes(searchText) || nombre.includes(searchText) || cantidad.includes(searchText) || descripcion.includes(searchText) || costo.includes(searchText)) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });
});

function validarAgregarDetalles() {
    if (productosAgregados.size === 0) {
        Swal.fire({
            icon: 'error',
            title: 'Error: no se agregaron detalles',
            text: 'No has agregado productos para seguir con la venta',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    }
    window.location.href = '/Ventums/CreateCliente';
}
function validarCancelar() {
    Swal.fire({
        title: '¿Estás seguro de cancelar la venta?',
        text: 'Se perderán los detalles agregados si das en aceptar',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#077336',
        cancelButtonColor: '#000000',
        confirmButtonText: 'Aceptar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = '/Ventums/Index';
        }
    });

}

var botonesSeleccionar = document.querySelectorAll('.btn-seleccionar');
botonesSeleccionar.forEach(function (boton) {
    boton.addEventListener('click', function () {
        // Obtener la fila actual del producto
        var fila = this.closest('tr');
        // Obtener los valores de la fila actual
        var idProducto = fila.cells[0].innerText;
        var nombre = fila.cells[1].innerText;
        var cantidad = fila.cells[2].innerText.replace(',', '.');
        var costo = parseFloat(fila.cells[4].textContent.replace('$', '').replace('.', '').replace(',', '.'));
        costoInicial = (costo).toFixed(2);
        // Asignar los valores a los inputs en infoProducto
        document.getElementById('inputIdProducto').value = idProducto;
        document.getElementById('inputNombre').value = nombre;
        document.getElementById('inputCosto').value = costoInicial;
        document.getElementById('inputCantidadD').value = cantidad;
        // Cerrar el modal
        var modal = document.getElementById('staticBackdrop');
        var modalBootstrap = bootstrap.Modal.getInstance(modal);
        modalBootstrap.hide();
    });
});

var productosAgregados = new Set();
var totalVenta = 0;
function actualizarTotal() {

    const totalFormated = (totalVenta).toLocaleString('es-ES', {
        minimumFractionDigits: 2,
        useGrouping: true,
        grouping: '.'
    });
    document.getElementById('totalVenta').textContent = 'Total: $' + totalFormated;
}

function limpiarCampos() {
    document.getElementById('inputCosto').value = "";
    document.getElementById('inputCantidadC').value = "";
}

document.getElementById('btnAgregar').addEventListener('click', function () {
    // Obtener el id del producto
    var idProducto = document.getElementById('inputIdProducto').value;

    // Verificar si el id del producto ya ha sido agregado
    if (productosAgregados.has(idProducto)) {
        Swal.fire({
            icon: 'error',
            title: 'Error con el producto',
            text: 'Este producto ya ha sido agregado a la lista de venta',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return; // Salir de la función si el producto ya ha sido agregado
    } else if (idProducto == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error con el producto',
            text: 'Debes seleccionar un producto para agregarlo',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    }

    // Obtener los valores de los otros campos de texto
    var nombre = document.getElementById('inputNombre').value;
    var costoTxt = document.getElementById('inputCosto').value;
    var costo = parseFloat(document.getElementById('inputCosto').value);
    var cantidadDisponible = parseFloat(document.getElementById('inputCantidadD').value);
    var cantidadAComprarTxt = document.getElementById('inputCantidadC').value;
    var cantidadAComprar = parseFloat(document.getElementById('inputCantidadC').value);

    if (cantidadAComprarTxt.includes(',')) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo cantidad a comprar',
            text: 'No se puede utilizar , como separador decimal',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    }else if (costoTxt.includes(',')) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo costo',
            text: 'No se puede utilizar , como separador decimal',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    }else if (isNaN(costo)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo costo',
            text: 'Es necesario poner el costo al producto',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    } else if (costo < costoInicial) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo costo',
            text: 'El costo no puede ser menor que el costo inicial: ' + costoInicial,
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    } else if (isNaN(cantidadAComprar)) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo cantidad a comprar',
            text: 'Es necesario poner la cantidad a comprar',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    } else if (cantidadAComprar <= 0) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo cantidad a comprar',
            text: 'La cantidad a comprar no puede ser menor o igual a 0',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    } else if (cantidadAComprar > cantidadDisponible) {
        Swal.fire({
            icon: 'error',
            title: 'Error en el campo cantidad a comprar',
            text: 'La cantidad a comprar es mayor a la disponible',
            timer: 3000,
            timerProgressBar: true,
            confirmButtonColor: '#000000',
            showConfirmButton: true
        })
        return
    } else {
        // El producto no está en el array, puedes agregarlo
        // Crear un objeto de detalle
        var detalle = {
            IdProducto: idProducto,
            Nombre: nombre,
            Cantidad: cantidadAComprar,
            Descripcion: null,
            Costo: costo,
            Estado: 1
        };
        $.ajax({
            type: "GET",
            url: "/Ventums/AlmacenarDetalles",
            contentType: "application/json; charset=utf-8",
            data: detalle,
            success: function (response) {
                // Manejar la respuesta del servidor si es necesario
                console.log(response.message);

            },
            error: function (error) {
                console.error("Error al guardar detalles", error);
            }
        });
        // Limpiar los campos después de agregar el detalle
        limpiarCampos();
    }

    var subtotal = (costo * cantidadAComprar).toFixed(2);
    totalVenta += parseFloat(subtotal);


    const subtotalFormatted = (cantidadAComprar*costo).toLocaleString('es-ES', {
        minimumFractionDigits: 2,
        useGrouping: true,
        grouping: '.'
    });
    
    const costoFormatted = (costo).toLocaleString('es-ES', {
        minimumFractionDigits: 2,
        useGrouping: true,
        grouping: '.'
    });

    const cantidadFormatted = (cantidadAComprar).toLocaleString('es-ES', {
        minimumFractionDigits: 2,
        useGrouping: true,
        grouping: '.'
    });
    // Crear una nueva fila en la tabla
    var newRow = document.createElement('tr');
    newRow.dataset.id = idProducto; // Usar el id del producto como identificador
    newRow.classList.add('filaProducto');
    newRow.innerHTML = `
            <td>${idProducto}</td>
            <td>${nombre}</td>
            <td>$${(costoFormatted)}</td>
            <td>${(cantidadFormatted)}</td>
            <td>$${(subtotalFormatted)}</td>
            <td><button class="btn btn-dark">Eliminar</button></td>
        `;
    // Agregar la nueva fila a la tabla
    document.getElementById('productosAgregados').getElementsByTagName('tbody')[0].appendChild(newRow);

    // Agregar el id del producto al set de productos agregados
    productosAgregados.add(idProducto);
    actualizarTotal();
    cambiarProgreso();


});

document.getElementById('productosAgregados').addEventListener('click', function (event) {
    if (event.target.classList.contains('btn-dark')) {
        // Obtener el id del producto a eliminar
        var idProductoEliminar = event.target.closest('tr').dataset.id;

        // Obtener el subtotal del producto a eliminar
        var subtotal = event.target.closest('tr').querySelector('td:nth-child(5)').textContent;
        subtotal = parseFloat(subtotal.replace('$', '').replace('.', '').replace(',', '.')).toFixed(2); // Convertir el subtotal a decimal

        // Eliminar la fila de la tabla
        event.target.closest('tr').remove();

        $.ajax({
            type: "GET",
            url: "/Ventums/EliminarDetalle",
            contentType: "application/json; charset=utf-8",
            data: { id: idProductoEliminar },
            success: function (response) {
                // Manejar la respuesta del servidor si es necesario
                console.log(response.message);
            },
            error: function (error) {
                console.error("Error al eliminar el detalle", error);
            }
        });

        // Eliminar el id del producto del set de productos agregados
        productosAgregados.delete(idProductoEliminar);

        totalVenta -= subtotal;
        actualizarTotal();
        cambiarProgreso();

    }
});

function cambiarProgreso() {
    if (productosAgregados.size === 0) {
        document.getElementById("progreso").style.background = "";
    } else {
        document.getElementById("progreso").style.background = "#ebebebeb";
    }
}
/*Si se devuelve a la vista de detalles pueda ver los productos ya agregados */
window.addEventListener('DOMContentLoaded', (event) => {
    var filas = document.querySelectorAll('#tablaProductos tbody tr');
    filas.forEach((fila) => {
        var idProducto = fila.dataset.id;
        var subtotal = parseFloat(fila.cells[4].textContent.replace('$', '').replace('.', '').replace(',', '.')).toFixed(2);
        productosAgregados.add(idProducto);
        totalVenta += parseFloat(subtotal);
    });
    actualizarTotal();
    cambiarProgreso();
});