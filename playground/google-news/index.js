function BuscaNoticia(keyWord) {
    var request = new XMLHttpRequest();

// Open a new connection, using the GET request on the URL endpoint
    request.open('GET', 'https://newsapi.org/v2/everything?q='+keyWord+'&apiKey=2aaa77be14d6496885ce7bb053b920cd', true);

    request.onload = function() {
        var data = this.response;
        data = JSON.parse(data);

        var str = "";
        for (var i = 0; i < 10; i++) {
            console.log(i);
            var urlImage = data['articles'][i]['urlToImage'];
            var url = data['articles'][i]['url'];
            var title = data['articles'][i]['title'];
            var author = data['articles'][i]['author'];
            author = author ? author : '';
            str += "<div style='display: flex; margin-bottom: 10px; cursor: pointer' href="+url+">";
            str += "<img height='98px' width='168px' src="+urlImage+">";
            str += "<span style='text-align: center'>"+title+"</span>";
            str += "<span>"+author+"</span>";
            str += "</div>";
        }
        $('#teste').html(str); // trocar dps pra onde vai setar o html da chamada
    };
    request.send();
}