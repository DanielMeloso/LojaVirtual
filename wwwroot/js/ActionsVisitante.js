$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
});

function MoverScrollOrdenacao() {
    if (window.location.hash.length > 0) {
        var hash = window.location.hash;
        if (hash == "#posicao-produto") {
            window.scrollBy(0, 473);
        }
    }
}

function MudarOrdenacao() {
    $("#ordenacao").change(function () {
        // Redirecionar para a tela Home/Index passando a QueryString de ordenação e mantendo a Página e a Pesquisa
        var Pagina = 1;
        var Pesquisa = "";
        var Ordenacao = $(this).val(); //pega a ordenação do campo com id ordenacao

        var QueryString = new URLSearchParams(window.location.search);
        if (QueryString.has("pagina")) {
            Pagina = QueryString.get("pagina");
        }
        if (QueryString.has("pesquisa")) {
            Pesquisa = QueryString.get("pesquisa");
        }

        // buscando a URL atual da página
        var url = window.location.protocol + "//" + window.location.host + window.location.pathname;

        // criar nova URL (Home/Index?pagina=&pesquisa=&ordenacao=)
        var urlComParametros = url + "?pagina=" + Pagina + "&pesquisa=" + Pesquisa + "&ordenacao=" + Ordenacao + "#posicao-produto";

        window.location.href = urlComParametros;

    });
}