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

    $(".btn-imagem-excluir").click(function () {
        var CampoHidden = $(this).parent().find("input[name=imagem]");
        var Imagem = $(this).parent().find(".img-upload");
        var BtnExcluir = $(this).parent().find(".btn-imagem-excluir");
        var InputFile = $(this).parent().find(".input-file");

        $.ajax({
            type: "GET",
            url: "/Colaborador/Imagem/Deletar?caminho=" + CampoHidden.val(),
            error: function () {
                
            },
            success: function () {
                Imagem.attr("src", "/img/imagem-padrao.png");
                BtnExcluir.addClass("btn-ocultar");
                CampoHidden.val("");
                InputFile.val("");
            }
        })
    });

    $(".input-file").change(function () {
        // Formulário de dados via JavaScript
        var Binario = $(this)[0].files[0];
        var Formulario = new FormData();
        Formulario.append("file", Binario);

        var CampoHidden = $(this).parent().find("input[name=imagem]");
        var Imagem = $(this).parent().find(".img-upload");
        var BtnExcluir = $(this).parent().find(".btn-imagem-excluir");
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
                var caminho = data.caminho;
                Imagem.attr("src", caminho);
                CampoHidden.val(caminho);
                BtnExcluir.removeClass("btn-ocultar");
            }

        })
    });
}