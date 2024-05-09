var currentUrl = window.location.href;

var urlParams = new URLSearchParams(currentUrl.split('?')[1]);

var id = urlParams.get('usrid');

var usr = {};

var proj = [];

var pgslct = 1;

window.onload = function () {
    fetchName(id);
    pgslct = 1;
    bind();
    chngpg(1);
    fetchProj(id);
}

document.getElementById('mb1').addEventListener('click', function () {
    pgslct = 1;
    bind();
    chngpg(1);
})

document.getElementById('mb2').addEventListener('click', function () {
    pgslct = 2;
    bind();
    chngpg(2);
    
})

function fetchName(id2) {
    fetch(`https://localhost:7287/api/getUserInfo?id=${id2}`,
        {
            mode: 'cors',
            method: 'GET'
        }
    ).then(function (response) {
        return response.json();
    }).then(function (data) {
        usr['name'] = data.getName;
        usr['surname'] = data.getLastname;
        document.getElementById('usrnm').innerHTML = usr['name'] + "  " + usr['surname'];
    })
}

function fetchProj(id2) {
    fetch(`https://localhost:7287/api/project?userId=${id2}`,
        {
            mode: 'cors',
            method: 'GET'
        }
    ).then(function (response) {
        return response.json();
    }).then(function (data) {
        var i = 0;
        for (var tmp in data) {
            proj[i].code = tmp.code;
            proj[i].isActiveProject = tmp.isActiveProject;
            i++;
        }
    })
}

function bind() {
    if (pgslct == 1) {
        document.getElementById('mb1').style.backgroundColor = 'rgb(111, 111, 142)';
        document.getElementById('mbt1').style.color = 'aliceblue';
        document.getElementById('mb2').style.backgroundColor = 'rgb(230,230,250)';
        document.getElementById('mbt2').style.color = 'rgb(111, 111, 142)';
    } else {
        document.getElementById('mb2').style.backgroundColor = 'rgb(111, 111, 142)';
        document.getElementById('mbt2').style.color = 'aliceblue';
        document.getElementById('mb1').style.backgroundColor = 'rgb(230,230,250)';
        document.getElementById('mbt1').style.color = 'rgb(111, 111, 142)';
    }
}

function chngpg(il) {
    if (il == 1) {
        document.getElementById('edit').style.display = 'flex';
        document.getElementById('browse').style.display = 'none';
    } else {
        document.getElementById('edit').style.display = 'none';
        document.getElementById('browse').style.display = 'flex';
    }
}