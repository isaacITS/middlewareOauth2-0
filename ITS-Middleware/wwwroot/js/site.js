//Modal
var modal = document.getElementById("editModal");
const modalNew = document.getElementById("newUserModal")

function openModal() {
    modal.style.display = "block";
}

function openNewModal() {
    modalNew.style.display = "block";
}

function closeModal() {
    modal.style.display = "none";
    modalNew.style.display = "none"
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
    if (event.target == modalNew) {
        modalNew.style.display = "none"
    }
}


const btnIconMenu = document.getElementById('btn-icon-menu')
const btnActiveMenu = document.getElementById('btn-active-menu')


function actionSlideMenu() {
    if (btnIconMenu.innerHTML == 'menu') {
        document.getElementById("sidenav").style.width = "250px";
        btnIconMenu.innerHTML = 'close'
        btnActiveMenu.setAttribute('title', 'Cerrar Menu')
    } else {
        document.getElementById("sidenav").style.width = "0";
        btnIconMenu.innerHTML = 'menu'
        btnActiveMenu.setAttribute('title', 'Abrir Menu')
    }
}

