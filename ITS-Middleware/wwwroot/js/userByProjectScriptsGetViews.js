//GET PARTIALS VIEWS (CRUD VIEWS)
$('#modalRegisterUserView').on('click', () => {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Registrar usuario para proyecto')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnRegisterUserByProject' type='button'><span class='align-middle material-icons'>save</span>&nbsp;Registrar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + 'UserByProject/Register',
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista registro")
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function (resp) {
            console.log(resp)
        }
    })
})

$('.modalUpdateUserView').on('click', function () {
    idUserUpdate = $(this).attr('data-idUser')
    getUpdateUserView(idUserUpdate)
})

$('.modalDeleteUserView').on('click', function () {
    getDeleteUserView($(this).attr('data-idUser'))
    idUserDeleteUser = $(this).attr('data-idUser')
})


function getUpdateUserView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Actualizar información de usuario')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnUpdateUserByProject' type='button'><span class='align-middle material-icons'>save</span>&nbsp;Actualizar</button>")
    $.ajax({
        type: 'GET',
        url: `${siteurl}UserByProject/Update?id=${id}`,
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista editar usuario")
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function (resp) {
            console.log(resp)
        }
    })
}

function getDeleteUserView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('¿Eliminar Usuario?')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-danger' id='btnDeleteUserByProject' type='button'><span class='align-middle material-icons'>delete_forever</span>&nbsp;Eliminar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + `UserByProject/Delete?id=${id}`,
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista eliminar usuario")
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function (resp) {
            console.log(resp)
        }
    })
}

