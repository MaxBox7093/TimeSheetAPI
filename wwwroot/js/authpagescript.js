var id = 0;

document.getElementById('sub').addEventListener('click', function (event) {
    event.preventDefault();

    var lg = document.getElementById('login').value;
    var ps = document.getElementById('pswd').value;


    var xhr = new XMLHttpRequest();
    xhr.open('GET', `https://localhost:7287/api/login?login=${lg}&password=${ps}`, true);
    xhr.onload = function () {
        if (xhr.status === 200) {
            var data = JSON.parse(xhr.responseText);
            id = data.userId;
            console.log(id)
        } else {
            id = 0;
            alert("Произошла ошибка при запросе на сервер");
        }

        if (id != 0) {
            window.location.href = `/Home/HP?usrid=${id}`;
        }
        else {
            alert("Пользователь с такими данными не найден");
        }
    };
    xhr.onerror = function () {
        id = 0;
        alert("Произошла ошибка при запросе на сервер");
    };
    xhr.send();
});
