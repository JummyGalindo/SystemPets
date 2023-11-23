
function updateRows(select) {
    var fila = select.closest('tr');

    var precioIn = fila.querySelector('#Precio');
    var descripcionIn = fila.querySelector('#Descripcion');
    var total = fila.querySelector('#Total-item');
    var cantidadIn = fila.querySelector('#Cantidad');
    var dropDown = fila.querySelector('#productos');

    var cantidad = parseFloat(cantidadIn.value);

    if (!cantidad) {
        dropDown.disabled = false;
    }

    var selectedOption = select.options[select.selectedIndex];

    if (selectedOption.value === "") {
        precioInput.value = "";
        descripcionInput.value = "";
    } else {
        var precio = parseFloat(selectedOption.getAttribute('data-precio'));
        var descripcion = selectedOption.getAttribute('data-descripcion');
        precioIn.value = precio;

        totalItem = precio * cantidad;

        descripcionIn.value = descripcion;
        total.value = totalItem;
    }

    var tabla = document.getElementById("detalleTabla");
    var totalSaleIn = document.getElementById("Total");
    var totalArticulos = document.getElementById("CantidadArticulos");

    
    var filas = tabla.getElementsByTagName("tr");
    var suma = 0;

    totalArticulos.value = parseInt(filas.length - 1);

    for (var i = 1; i < filas.length; i++) {
        var celda = filas[i].getElementsByTagName("td")[4];

        if (celda) {
            var inputTotal = celda.getElementsByTagName("input")[0];
            var valor = parseFloat(inputTotal.value);
            if (!isNaN(valor)) {
                suma += valor;
                totalSaleIn.value = parseFloat(suma);
            }
        }
    }
}

function enabledDropdown() {
    var dropdown = document.getElementById('IdProducto');
    var dropdown = document.getElementById('productos');



}
