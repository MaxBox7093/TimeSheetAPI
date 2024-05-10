var currentUrl = window.location.href;

var urlParams = new URLSearchParams(currentUrl.split('?')[1]);

var id = urlParams.get('usrid');

var usr = {};

var proj = [];

var task = [];

var pgslct = 1;

var tabselect = 1;

var ondel = -1;
var ondeltask = -1;

var projname = '';

var taskname = '';

window.onload = function () {
    fetchName(id);
    pgslct = 1;
    bind();
    chngpg(1, tabselect);
    if (tabselect == 1) {
        fetchProj(id, 1);
    }
    if (tabselect == 2) {
        fetchTask(id, 1);
    }
   
}

document.getElementById('mb1').addEventListener('click', function () {
    pgslct = 1;
    bind();
    chngpg(1, tabselect);
})

document.getElementById('mb2').addEventListener('click', function () {
    pgslct = 2;
    bind();
    chngpg(2, tabselect);
    
})

// Проект и всё, что с ним

document.getElementById('add').addEventListener('click', function () {
    document.getElementById('addproj').style.display = 'inline-flex';
})

document.getElementById('adb').addEventListener('click', function () {
    document.getElementById('addproj').style.display = 'none';
    let cd = document.getElementById('prjcd').value;
    let nm = document.getElementById('prjnm').value;
    fetchPostProj(id, cd, nm, 1);
    document.getElementById('prjcd').value = '';
    document.getElementById('prjnm').value = '';
})

document.getElementById('cdb').addEventListener('click', function () {
    document.getElementById('addproj').style.display = 'none';
})

document.getElementById('alt').addEventListener('click', function () {
    if (ondel != -1) {
        document.getElementById('chngproj').style.display = 'inline-flex';
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
    document.getElementById('chngproj').style.display = 'none';
    let cd = document.getElementById('cprjcd').value;
    let nm = document.getElementById('cprjnm').value;
    proj[ondel].name = nm;
    let cd0 = proj[ondel].code;
    proj[ondel].code = cd;
    if (document.getElementById('ctrue').checked) {
        proj[ondel].isActiveProject = true;
    } else if (document.getElementById('cfalse').checked = true) {
        proj[ondel].isActiveProject = false;
    }
    let act = proj[ondel].isActiveProject;
    fetchPatchProj(cd0, cd, nm, act, 1);
    document.getElementById('cprjcd').value = '';
    document.getElementById('cprjnm').value = '';
    document.getElementById('ctrue').checked = true;
    ondel = -1;
    for (let i = 0; i < proj.length; i++) {
        document.getElementById('pj' + i).style.backgroundColor = 'transparent';
    }
})

document.getElementById('ccdb').addEventListener('click', function () {
    document.getElementById('chngproj').style.display = 'none';
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
        console.log(data);
        for (let tmp in data) {
            let tp = {};
            tp.id = data[tmp].id;
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
                    location.reload();
                });
        }
    }
}

function fetchPatchProj(cd, cd2, nm, act, il) {

    if (!nm) {
        alert("Название проекта было пустым");
    } else {
        if (!cd) {
            alert("Код проекта был пустым");
        } else {
            let prj = { code: Number(cd2), projectName: nm, isActiveProject: act };
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
                    location.reload();
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
            location.reload();
    })
}

// Задача и всё, что с ней

document.getElementById('editback').addEventListener('click', function () {
    tabselect = -1;
    ondel = -1;
    location.reload();
})

document.getElementById('bback').addEventListener('click', function () {
    tabselect = -1;
    ondel = -1;
    location.reload();
})

document.getElementById('tadd').addEventListener('click', function () {
    document.getElementById('addtask').style.display = 'inline-flex';
})

document.getElementById('tadb').addEventListener('click', function () {
    document.getElementById('addtask').style.display = 'none';
    let nm = document.getElementById('tsknm').value;
    fetchPostTask(nm, 1);
    document.getElementById('tsknm').value = '';
})

document.getElementById('tcdb').addEventListener('click', function () {
    document.getElementById('addtask').style.display = 'none';
})

document.getElementById('talt').addEventListener('click', function () {
    if (ondeltask != -1) {
        document.getElementById('chngtask').style.display = 'inline-flex';
        let tmp = task[ondeltask];
        document.getElementById('ctsk').value = tmp.name;
        if (tmp.isActiveTask) {
            document.getElementById('ttrue').checked = true;
        } else {
            document.getElementById('tfalse').checked = true;
        }
    }

})

document.getElementById('tcngdb').addEventListener('click', function () {
    document.getElementById('chngtask').style.display = 'none';
    let nm = document.getElementById('ctsk').value;
    task[ondeltask].name = nm;
    let id0 = task[ondeltask].id;
    if (document.getElementById('ttrue').checked) {
        task[ondeltask].isActiveTask = true;
    } else if (document.getElementById('tfalse').checked = true) {
        task[ondeltask].isActiveTask = false;
    }
    let act = task[ondeltask].isActiveTask;
    fetchPatchTask(ondeltask, id0, nm, act, 1);
    document.getElementById('ctsk').value = '';
    document.getElementById('ttrue').checked = true;
    ondeltask = -1;
    for (let i = 0; i < task.length; i++) {
        document.getElementById('t' + i).style.backgroundColor = 'transparent';
    }
})

document.getElementById('tccdb').addEventListener('click', function () {
    document.getElementById('chngtask').style.display = 'none';
    ondeltask = -1;
    for (let i = 0; i < task.length; i++) {
        document.getElementById('t' + i).style.backgroundColor = 'transparent';
    }
})

document.getElementById('tdel').addEventListener('click', function () {

    if (ondeltask != -1) {
        if (confirm("Вы действительно хотите удалить эту задачу?")) {

            fetchDelTask(task[ondeltask].id, ondeltask);
        }
        else {
            for (let i = 0; i < task.length; i++) {
                document.getElementById('t' + i).style.backgroundColor = 'transparent';
            }
        }
    }
    ondeltask = -1;
})

document.getElementById('tdesel').addEventListener('click', function () {
    ondeltask = -1;
    for (let i = 0; i < task.length; i++) {
        document.getElementById('t' + i).style.backgroundColor = 'transparent';
    }
})

function fetchTask(pc, il) {
    console.log(pc);
    fetch(`https://localhost:7287/api/task?id_project=${pc}`,
        {
            mode: 'cors',
            method: 'GET'
        }
    ).then(function (response) {
        return response.json();
    }).then(function (data) {
        console.log(data);
        for (let tmp in data) {
            let tt = {};
            tt.id = data[tmp].id_task;
            tt.name = data[tmp].task_name;
            tt.isActiveTask = data[tmp].is_active_task;
            task.push(tt);
        }
        setTask(il);
    })
}

function fetchPostTask(nm, il) {
    if (!nm) {
        alert("Название задачи было пустым");
    } else {
            let prj = { task_name: nm, task_ref: proj[ondel].id, is_active_task: true};
            fetch(`https://localhost:7287/api/task`,
                {
                    mode: 'cors',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'
                    },
                    body: JSON.stringify(prj)
                }).then(function (response) {
                    let mxid = 0;
                    for (let i = 0; i < task.length; i++)
                    {
                        if (task[i].id >= mxid) {
                            mxid = task[i].id;
                        }
                    }
                    let tt = { id: mxid, name: nm, isActiveTask: true };
                    task.push(tt);
                    setTask(il);
                });
        }
    }

function fetchPatchTask(tidd, id0, nm, act, il) {

    if (!nm) {
        alert("Название задачи было пустым");
    } else {
            let tsk = {id_task: id0, task_name: nm, is_active_task: act};
            fetch(`https://localhost:7287/api/task`,
                {
                    mode: 'cors',
                    method: 'PATCH',
                    headers: {
                        'Content-Type': 'application/json;charset=utf-8'
                    },
                    body: JSON.stringify(tsk)
                }).then(function (response) {
                    let tt = { id: id0, name: nm, isActiveTask: act };
                    console.log(tt);
                    task.splice(tidd, 1, tt);
                    setTask(il);
                })
        }
    }

function fetchDelTask(tidd, id0) {
    fetch(` https://localhost:7287/api/task?id_task=${tidd}`,
        {
            mode: 'cors',
            method: 'DELETE'
        }).then(function (response) {
            task.splice(id0, 1);
            setTask(1);
        })
}


//Общие функции

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
        elements[j].addEventListener('dblclick', function () {
            tabselect = 2;
            projname = proj[Number(this.parentNode.id.replace('pj', ''))].name;
            fetchTask(proj[ondel].id, 1);
            loadtab(tabselect);
        });
    }
}

function loadtab(lt) {
    if (lt == 1) {
        ondel = -1;
        ondeltask = -1;
        document.getElementById('tpjnmmain').innerHTML = '';
        document.getElementById('mainproj').style.display = 'flex';
        document.getElementById('maintask').style.display = 'none';
    } if (lt == 2) {
        ondeltask = -1;
        document.getElementById('tpjnmmain').innerHTML = projname;
        document.getElementById('mainproj').style.display = 'none';
        document.getElementById('maintask').style.display = 'flex';
    } if (lt == 3) {

    }
}

function bindDelTab() {
    let elements = document.getElementsByClassName('ttxt')

    for (let j = 0; j < elements.length; j++) {
        elements[j].addEventListener('click', function () {
            let tid = this.parentNode.id.replace('t', '');
            ondeltask = Number(tid);
            for (let i = 0; i < task.length; i++) {
                console.log("t" + i);
                document.getElementById('t' + i).style.backgroundColor = 'transparent';
            }
            document.getElementById('t' + tid).style.backgroundColor = 'rgb(247,247,255)';
            
        });
        elements[j].addEventListener('dblclick', function () {
            tabselect = 3;
            taskname = task[Number(this.parentNode.id.replace('t', ''))].name;

            loadtab(tabselect);
        });
    }
}

function setTask(il) {
    document.getElementById('thlde').innerHTML = '';
    document.getElementById('thldb').innerHTML = '';
    console.log(task);
    for (let i = 0; i < task.length; i++) {
        let tmpdiv = document.createElement('div');
        tmpdiv.classList.add('tc');
        tmpdiv.id = `t${i}`;

        let td2 = document.createElement('div');
        let td3 = document.createElement('div');

        td2.classList.add('ttxt');
        td3.classList.add('ttxt');

        td2.innerHTML = task[i].name;
        if (task[i].isActiveTask)
            td3.innerHTML = "Активная";
        else
            td3.innerHTML = "Неактивная";

        tmpdiv.appendChild(td2);
        tmpdiv.appendChild(td3);

        if (il == 1) {
            document.getElementById('thlde').appendChild(tmpdiv);
        } else {
            document.getElementById('thldb').appendChild(tmpdiv);
        }
    }

    bindDelTab();
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

function chngpg(il, ts) {
    if (ts == 1) {
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
    if (ts == 2) {
        if (il == 1) {
            document.getElementById('tedit').style.display = 'flex';
            document.getElementById('tbrowse').style.display = 'none';
            setTask(il);
        } else {
            document.getElementById('tedit').style.display = 'none';
            document.getElementById('tbrowse').style.display = 'flex';
            setTask(il);
        }
    }
}