$(document).ready(function () {
    $(".btn-danger").click(function (e) {
        var resultado = confirm("Desejar excluir esse registro?");

        if (resultado == false) {
            e.preventDefault();
        }
    });
});