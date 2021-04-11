$(document).ready(function () {
    $(".btn-danger").click(function (e) {
        var resultado = confirm("Desejar realizar essa operação?");

        if (resultado == false) {
            e.preventDefault();
        }
    });
    $('.dinheiro').mask('000.000.000.000.000,00', { reverse: true });

    AjaxUpdaloadImagemProduto();
});

function AjaxUpdaloadImagemProduto() {
    $(".img-upload").click(function () {
        $(this).parent().find(".input-file").click();
    });

    $(".input-file").change(function () {
        // Formulário de dados via JavaScript
        var Binario = $(this)[0].files[0];
        var Formulario = new FormData();
        Formulario.append("file", Binario);

        // Requisição Ajax enviando o formulário (imagem)
        $.ajax({
            type: "POST",
            url: "/Colaborador/Imagem/Armazenar",
            data: Formulario,
            contentType: false,
            processData: false, // desabilitar validação do formulário.
            error: function () {
                alert("Erro no envio do arquivo!");
            },
            success: function (data) {
                alert("Arquivo enviado com sucesso! " + data.caminho);
            }

        })
    });
}