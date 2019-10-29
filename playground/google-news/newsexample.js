 var request = new XMLHttpRequest();

// Open a new connection, using the GET request on the URL endpoint
    request.open('GET', 'https://newsapi.org/v2/everything?q='+keyWord+'&apiKey=2aaa77be14d6496885ce7bb053b920cd', true);

    request.onload = function() {
        var data = this.response;
        data = JSON.parse(data);
        console.log(data)
    };
    request.send();