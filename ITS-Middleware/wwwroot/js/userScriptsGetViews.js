//GET PARTIALS VIEWS (CRUD VIEWS)
$('#modalRegisterView').on('click', () => {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Registrar usuario')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnRegisterUser' type='button' disabled><span class='align-middle material-icons'>save</span>&nbsp;Registrar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + 'User/Register/',
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista registro")
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
            $('.btn-generate-pass').click()
        },
        error: function(resp) {
            console.log(resp)
        }
    })
})

$('.modalUpdateView').on('click', function() {
    getUpdateUserView($(this).attr('data-idUser'))
    idUserUpdate = $(this).attr('data-idUser')
})

$('.modalDeleteView').on('click', function() {
    getDeleteUserView($(this).attr('data-idUser'))
    idUserDelete = $(this).attr('data-idUser')
})

$('.modalStatusView').on('click', function() {
    getUpdateStatusUserView($(this).attr('data-idUser'))
    idUserUpdateStatus = $(this).attr('data-idUser')
})

function getUpdateUserView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Actualizar información de usuario')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnUpdateUser' type='button' disabled><span class='align-middle material-icons'>save</span>&nbsp;Actualizar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + `User/EditUser/${id}`,
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista editar usuario")
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function(resp) {
            console.log(resp)
        }
    })
}

function getDeleteUserView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('¿Eliminar Usuario?')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-danger' id='btnDeleteUser' type='button'><span class='align-middle material-icons'>delete_forever</span>&nbsp;Eliminar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + `User/DeleteUser/${id}`,
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista eliminar usuario")
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function(resp) {
            console.log(resp)
        }
    })
}

function getUpdateStatusUserView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('¿Actualizar estatus de usuario?')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnUpdateStatusUser' type='button'><span class='align-middle material-icons'>update</span>&nbsp;Actualizar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + `User/ChangeStatus/${id}`,
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista cambiar estatus de usuario")
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function(resp) {
            console.log(resp)
        }
    })
}