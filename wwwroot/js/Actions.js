$(document).ready(function () {
    $(".btn-danger").click(function (e) {
        var resultado = confirm("Desejar realizar essa operação?");

        if (resultado == false) {
            e.preventDefault();
        }
    });
    $('.dinheiro').mask('000.000.000.000.000,00', { reverse: true });
});