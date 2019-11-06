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
    // Inicializa o gráfico
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
    // Desmembra os dados do papel
    const { papel, historico, noticias, fundamentos } = response;    
    console.log(response);
    
    // Renderiza o histórico do papel
    renderizarHistoricoPapel(historico);
    
    // Renderiza as notícias do papel
    renderizarNoticiasPapel(noticias);
    
    // Renderiza os fundamentos do papel
    renderizarFundamentosPapel(fundamentos);
}

function renderizarHistoricoPapel(historico) {
    // Pega apenas os 180 últimos dias por questões de performance
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
    // Utilizar o objeto jQuery objetosPagina.containerNoticias que já está apontando para o container de notícias
    console.log(noticias);
}

function renderizarFundamentosPapel(fundamentos) {
    // Utilizar o objeto jQuery objetosPagina.containerFundamentos que já está apontando para o container de fundamentos
    console.log(fundamentos);
}
