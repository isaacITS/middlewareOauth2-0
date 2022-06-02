//Modal
var modal = document.getElementById("editModal");
const modalNew = document.getElementById("newUserModal")

function openModal () {
    modal.style.display = "block";
}

function openNewModal() {
    modalNew.style.display = "block";
}

function closeModal () {
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

