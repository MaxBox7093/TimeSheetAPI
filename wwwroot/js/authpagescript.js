var id = 0;

document.getElementById('sub').addEventListener('click', function (event) {
    event.preventDefault();

    var lg = document.getElementById('login').value;
    var ps = document.getElementById('pswd').value;
    var res = document.getElementById('reslt');


    var xhr = new XMLHttpRequest();
    xhr.open('GET', `https://localhost:7287/api/login?login=${lg}&password=${ps}`, true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            var data = JSON.parse(xhr.responseText);
            id = data.userId;
            console.log(id)
        } else {
            res.value = "Ошибка: " + xhr.statusText;
        }

        if (id != 0) {
            //переходим на другую страницу
        }
    };
    xhr.onerror = function () {
        res.value = "Ошибка сети";
    };
    xhr.send();
});
