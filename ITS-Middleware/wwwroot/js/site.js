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

//GET PARTIALS VIEWS (CRUD VIEWS)
$('#btnViewRegUser').on('click', () => {
    $('#registerModalBody').fadeOut(1)
    $.ajax({
        type: 'GET',
        url: '/User/Register',
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Error'
                console.log("Error al obtener vista registro")
                return;
            }
            $("#registerModalBody").append(resp).hide();
            $('#registerModalBody').fadeIn();
        },
        error: function (resp) {
            console.log(resp)
        }
    })
})




//Remove forms modal content
$('.btn-close-modal-views').on('click', () => {
    $('#registerUserForm').remove()
})

