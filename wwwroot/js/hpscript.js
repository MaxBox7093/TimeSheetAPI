var currentUrl = window.location.href;

var urlParams = new URLSearchParams(currentUrl.split('?')[1]);

var id = urlParams.get('usrid');

var usr = {};

var proj = [];

var pgslct = 1;

var ondel = -1;

window.onload = function () {
    fetchName(id);
    pgslct = 1;
    bind();
    chngpg(1);
    fetchProj(id, 1);
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

document.getElementById('add').addEventListener('click', function () {
    document.getElementById('adddial').style.display = 'inline-flex';
})

document.getElementById('adb').addEventListener('click', function () {
    document.getElementById('adddial').style.display = 'none';
    let tmp = {};
    let cd = document.getElementById('prjcd').value;
    let nm = document.getElementById('prjnm').value;
    fetchPostProj(id, cd, nm, 1);
    tmp.name = nm;
    tmp.code = cd;
    tmp.isActiveProject = true;
    proj.push(tmp);
    document.getElementById('prjcd').value = '';
    document.getElementById('prjnm').value = '';
})

document.getElementById('cdb').addEventListener('click', function () {
    document.getElementById('adddial').style.display = 'none';
})

document.getElementById('alt').addEventListener('click', function () {
    if (ondel != -1) {
        document.getElementById('chngdial').style.display = 'inline-flex';
        let tmp = proj[ondel];
        document.getElementById('cprjcd').value = tmp.code;
        document.getElementById('cprjnm').value = tmp.name;
        if (tmp.isActiveProject) {
            document.getElementById('ctrue').checked = true;
        } else {
            document.getElementById('cfalse').checked = true;
        }
    }
   
})

document.getElementById('cngdb').addEventListener('click', function () {
    document.getElementById('chngdial').style.display = 'none';
    let cd = document.getElementById('cprjcd').value;
    let nm = document.getElementById('cprjnm').value;
    proj[ondel].name = nm;
    proj[ondel].code = cd;
    if (document.getElementById('ctrue').checked) {
        proj[ondel].isActiveProject = true;
    } else if (document.getElementById('cfalse').checked = true) {
        proj[ondel].isActiveProject = false;
    }
    let act = proj[ondel].isActiveProject;
    fetchPatchProj(cd, nm, act, 1);
    document.getElementById('cprjcd').value = '';
    document.getElementById('cprjnm').value = '';
    document.getElementById('ctrue').checked = true;
    ondel = -1;
    for (let i = 0; i < proj.length; i++) {
        document.getElementById('pj' + i).style.backgroundColor = 'transparent';
    }
})

document.getElementById('ccdb').addEventListener('click', function () {
    document.getElementById('chngdial').style.display = 'none';
    ondel = -1;
    for (let i = 0; i < proj.length; i++) {
        document.getElementById('pj' + i).style.backgroundColor = 'transparent';
    }
})

document.getElementById('del').addEventListener('click', function () {

    if (ondel != -1) {
        if (confirm("Вы действительно хотите удалить этот проект?")) {

            fetchDelProj(proj[ondel].code, ondel);
        }
        else {
            for (let i = 0; i < proj.length; i++) {
                document.getElementById('pj' + i).style.backgroundColor = 'transparent';
            }
        }
    }
    ondel = -1;
})

document.getElementById('desel').addEventListener('click', function () {
    ondel = -1;
    for (let i = 0; i < proj.length; i++) {
        document.getElementById('pj' + i).style.backgroundColor = 'transparent';
    }
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

function fetchProj(id2, il) {
    fetch(`https://localhost:7287/api/project?userId=${id2}`,
        {
            mode: 'cors',
            method: 'GET'
        }
    ).then(function (response) {
        return response.json();
    }).then(function (data) {
        for (let tmp in data) {
            let tp = {};
            tp.name = data[tmp].projectName;
            tp.code = data[tmp].code;
            tp.isActiveProject = data[tmp].isActiveProject;
            proj.push(tp);
        }
        setProjs(il);
    })
}

function fetchPostProj(id2, cd, nm, il) {
    if (!nm) {
        alert("Название проекта было пустым");
    } else {
        if (!cd) {
            alert("Код проекта был пустым");
        } else {
            let prj = { code: Number(cd), projectName: nm, isActiveProject: true };
            fetch(`https://localhost:7287/api/project?userId=${id2}`,
                {
                    mode: 'cors',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'
                    },
                    body: JSON.stringify(prj)
                }).then(function (response) {
                    setProjs(il);
                });
        }
    }
}

function fetchPatchProj(cd, nm, act, il) {

    if (!nm) {
        alert("Название проекта было пустым");
    } else {
        if (!cd) {
            alert("Код проекта был пустым");
        } else {
            let prj = { code: Number(cd), projectName: nm, isActiveProject: act };
            fetch(` https://localhost:7287/api/project?code=${cd}`,
                {
                    mode: 'cors',
                    method: 'PATCH',
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'
                    },
                    body: JSON.stringify(prj)
                }).then(function (response) {
                    setProjs(il);
                })
        }
    }
}

function fetchDelProj(cdpj, id0) {
    fetch(`https://localhost:7287/api/project?code=${cdpj}`,
       {
       mode: 'cors',
       method: 'DELETE'
        }).then(function (response) {
            proj.splice(id0, 1);
       setProjs(1);
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

function bindDel() {
    let elements = document.getElementsByClassName('pjtxt')

    for (let j = 0; j < elements.length; j++) {
        elements[j].addEventListener('click', function () {
            let tid = this.parentNode.id.replace('pj', '');
            ondel = Number(tid);
            for (let i = 0; i < proj.length; i++) {
                    document.getElementById('pj' + i).style.backgroundColor = 'transparent';
            }
            document.getElementById('pj' + tid).style.backgroundColor = 'rgb(247,247,255)';
        });
    }
}

function setProjs(il) {
    document.getElementById('prjhlde').innerHTML = '';
    document.getElementById('prjhldb').innerHTML = '';

    for (let i = 0; i < proj.length; i++) {
        let tmpdiv = document.createElement('div');
        tmpdiv.classList.add('pjc');
        tmpdiv.id = `pj${i}`;

        let td1 = document.createElement('div');
        let td2 = document.createElement('div');
        let td3 = document.createElement('div');

        td1.classList.add('pjtxt');
        td2.classList.add('pjtxt');
        td3.classList.add('pjtxt');

        td1.innerHTML = proj[i].code;
        td2.innerHTML = proj[i].name;
        if (proj[i].isActiveProject)
            td3.innerHTML = "Активный";
        else
            td3.innerHTML = "Неактивный";

        tmpdiv.appendChild(td1);
        tmpdiv.appendChild(td2);
        tmpdiv.appendChild(td3);

        if (il == 1) {
            document.getElementById('prjhlde').appendChild(tmpdiv);
        } else {
            document.getElementById('prjhldb').appendChild(tmpdiv);
        }
    }

    bindDel();
}

function chngpg(il) {
    if (il == 1) {
        document.getElementById('edit').style.display = 'flex';
        document.getElementById('browse').style.display = 'none';
        setProjs(il);
    } else {
        document.getElementById('edit').style.display = 'none';
        document.getElementById('browse').style.display = 'flex';
        setProjs(il);
    }
}