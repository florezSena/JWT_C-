document.getElementById('buscarVenta').addEventListener('input', function () {
    var input = this.value.trim().toLowerCase();
    var rows = document.querySelectorAll('.ventasPaginado');

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

    var rowsTodos = document.querySelectorAll('.ventasTodos');

    rowsTodos.forEach(function (row) {

        if (input === "") {
            row.style.display = 'none';
        } else {
            var idVenta = row.querySelector('td:nth-child(1)').textContent.trim().toLowerCase();
            var cliente = row.querySelector('td:nth-child(2)').textContent.trim().toLowerCase();
            var fecha = row.querySelector('td:nth-child(3)').textContent.trim().toLowerCase();
            var total = row.querySelector('td:nth-child(4)').textContent.trim().toLowerCase();
            var estado = row.querySelector('td:nth-child(5)').textContent.trim().toLowerCase();


            row.style.display = (idVenta.includes(input) || cliente.includes(input) || fecha.includes(input) || total.includes(input) || estado.includes(input)) ? 'table-row' : 'none';
        }
    });
});

function vaciarInput() {

    document.getElementById('buscarVenta').value = "";
    var icon = document.querySelector('#button-addon2 i');
    icon.className = 'fa-solid fa-magnifying-glass fa-2xl';
    icon.style.color = 'gray';

    var rows = document.querySelectorAll('.ventasPaginado');
    rows.forEach(function (row) {
        row.style.display = '';
    });

    var rowsTodos = document.querySelectorAll('.ventasTodos');

    rowsTodos.forEach(function (row) {
        row.style.display = 'none';
    });
}

function cambiarEstado(nuevoEstado, IdVenta) {
    Swal.fire({
        title: "¿Estás seguro de anular esta venta?",
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
                title: 'Venta anulada con éxito',
                confirmButtonColor: '#000000',
                showConfirmButton: true
            }).then((result) => {
                if (result.isConfirmed) {
                    var url = "/Ventums/CambiarEstado/" + IdVenta;
                    window.location.href = url;
                }
            });
        }
    });
}