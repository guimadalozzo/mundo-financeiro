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
    }).bind('typeahead:select', buscarFundamentos);
});

const buscarFundamentos = (_, papel) => {
    if (papel === undefined || typeof papel.id !== 'number' || papel.id <= 0) {
        return toastr.error('Informe um papel válido para continuar.');
    }
    
    // TODO: Chamar a API de dados e preencher os containers 
    toastr.success(JSON.stringify(papel));
};