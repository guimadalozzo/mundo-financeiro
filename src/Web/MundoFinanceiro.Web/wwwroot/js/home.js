const objetosPagina = {
    graficoHistorico: {},
    containerNoticias: {},
    containerFundamentos: {}
};

$(document).ready(() => {
    const papeisDisponiveis = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('nome'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: 'api/papeis/disponiveis?query=%QUERY%',
            wildcard: '%QUERY%'
        }
    });

$('#papeis').typeahead({ minLength: 1, highlight: true }, {
    name: 'papeis',
    display: 'nome',
    source: papeisDisponiveis,
    limit: 10
}).bind('typeahead:select', buscarDadosPapel);

inicializarPagina();
});

function inicializarPagina() {
    objetosPagina.graficoHistorico = new ApexCharts(document.querySelector("#grafico-historico"), {
        chart: {
            height: 350,
            type: 'candlestick'
        },
        series: [{
            data: []
        }],
        xaxis: {
            type: 'datetime'
        },
        yaxis: {
            tooltip: {
                enabled: true
            }
        },
        noData: {
            text: 'Nenhum dado disponível',
            align: 'center',
            verticalAlign: 'middle'
        },
        animations: {
            enabled: false
        }
    });
    objetosPagina.graficoHistorico.render();

    // Inicializa os demais containers
    objetosPagina.containerNoticias = $('#noticias-papel');
    objetosPagina.containerFundamentos = $('#fundamentos-papel');
}

function buscarDadosPapel(_, papel) {
    if (papel === undefined || typeof papel.id !== 'number' || papel.id <= 0) {
        return toastr.error('Informe um papel válido para continuar.');
    }

    $.ajax({
        url: `https://localhost:5005/api/v1/Papeis/${papel.id}`,
        success: callbackSucessoDadosPapel,
        error: callbackErrorDadosPapel
    });
}

function callbackErrorDadosPapel(response) {
    if (response.message !== undefined) {
        toastr.error(response.message);
    }

    console.error(response);
}

function callbackSucessoDadosPapel(response) {
    const { papel, historico, noticias, fundamentos } = response;
    console.log(response);

    renderizarHistoricoPapel(historico);

    renderizarNoticiasPapel(noticias);

    renderizarFundamentosPapel(fundamentos);
}

function renderizarHistoricoPapel(historico) {
    let dadosHistorico = historico;
    if (historico.length > 180) {
        dadosHistorico = historico.slice(historico.length - 180, historico.length);
    }

    const dadosGrafico = dadosHistorico.map(itemHistorico => ({
        x: new Date(itemHistorico.date),
        y: [
            itemHistorico.open,
            itemHistorico.max,
            itemHistorico.min,
            itemHistorico.close
        ]
    }));

    objetosPagina.graficoHistorico.updateSeries([{
        data: dadosGrafico
    }]);
}

function renderizarNoticiasPapel(noticias) {
    if (!Array.isArray(noticias) || noticias.length === 0) {
        $('#noticias-papel').children().remove();
        return toastr.info('Nenhuma notícia encontrada para este papel.');
    }
    
    var str = "";
    for (var i = 0; i < 10; i++) {
        var urlImage = noticias[i]['urlToImage'];
        var url = noticias[i]['url'];
        var title = noticias[i]['title'];
        var author = noticias[i]['author'];
        author = author ? author : 'Autor Desconhecido!';
        title = title.trim();
        var titlespan = title.length > 60 ? title.substr(0, 60)+'...' : title;
        author = author ? author : '';
        str += "<div style='display: flex; margin-bottom: 10px; cursor: pointer' title='"+title+"'>";
        str += "<img height='98px !important' width='168px !important' style='border-radius: 5px' src='"+urlImage+"'>";
        str += "<div style='display: flow-root'>";
        str += "<div style='margin-top: 5px; font-family: serif; font-weight: bold; margin-right: 5%; margin-left: 5%;'><a target='_blank' href='"+url+"'><span style='text-align: center; color:black'>"+titlespan+"</span></a></div>";
        str += "<div style='color: #7b7777; font-style: italic; margin-top: 7%;'><span>"+author+"</span></div>";
        str += "</div>";
        str += "</div>";
    }
    $('#noticias-papel').html(str);
}

function renderizarFundamentosPapel(fundamentos) {
    var lpa = FormataNumeroMoeda(fundamentos['lpa']);
    var vpa = FormataNumeroMoeda(fundamentos['vpa']);
    var roe = FormataNumeroMoeda(fundamentos['roe']);
    var roic = FormataNumeroMoeda(fundamentos['roic']);
    var valorMercado = FormataNumeroMoeda(fundamentos['valorMercado']);
    var str = "";
    
    str += "<div style='display: inline-flex; width: 100%; padding-left: 3%; margin-top: 5%'>";
    str += montaHtmlCard('LPA', lpa);
    str += montaHtmlCard('VPA', vpa);
    str += montaHtmlCard('ROE', roe);
    str += montaHtmlCard('ROIC', roic);
    str += montaHtmlCard('Valor de Mercado', valorMercado);
    str += "</div>";
    $('#fundamentos-papel').html(str);
    
}

function montaHtmlCard(title, value) {
    const strCard = "<div class='card bg-light' style='max-width: 18rem; margin-right: 3%; min-width: 8em'>\n" +
        "<div class='card-header' style='text-align: center'><strong>"+title+"</strong></div>\n" +
        " <div class='card-body'>\n" +
        "<div style='min-height: 75%; margin-bottom: 10%'><p style='margin-bottom: 0; text-align: justify;'>"+buscaTextTitle(title)+"</p></div>\n"+
        "<div><p class='card-text' style='text-align: center'>"+value+"</p></div>\n" +
        "</div>\n" +
        "</div>";
    return strCard;
}

function buscaTextTitle(title) {
    const arrText = {
                'LPA': 'LPA (sigla para Lucro Por Ação) é o lucro líquido da companhia em determinado período dividido pelo total corrente de ações',
                'ROE': 'ROE, ou Retorno sobre o Patrimônio corresponde ao  lucro líquido acumulado nos últimos 12 meses dividido pelo patrimônio líquido.',
                'ROIC': 'ROIC, ou  Retorno Sobre o Capital Investido corresponde ao NOPLAT(Lucro Operacional Líquido depois dos Impostos) dividido pelo capital investido.',
                'Valor de Mercado': 'O valor de mercado é o resultado da multiplicação do valor atual da ação de uma companhia pelo número de ações existentes.',
                'VPA': 'VPA, ou Valor Patrimonial por Ação, corresponde ao valor do patrimônio líquido de uma empresa dividido pela sua quantidade de ações.'
                };
    return arrText[title];
}

function FormataNumeroMoeda(num) {
    x = 0;
    if (num<0) {
        num = Math.abs(num);
        x = 1;
    }
    if(isNaN(num)) 
        num = "0";
    cents = Math.floor((num*100+0.5)%100);
    num = Math.floor((num*100+0.5)/100).toString();
    if(cents < 10) cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
        num = num.substring(0,num.length-(4*i+3))+'.'
            +num.substring(num.length-(4*i+3));
    ret = num + ',' + cents;
    if (x == 1) 
        ret = ' -' + ret;
    return ret;
}
