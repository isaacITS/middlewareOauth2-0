//GET PARTIALS VIEWS (CRUD VIEWS)
$('#modalRegisterUserView').on('click', () => {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Registrar usuario para proyecto')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnRegisterUserByProject' type='button' disabled><span class='align-middle material-icons'>save</span>&nbsp;Registrar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + 'UserByProject/Register',
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
            $('.btn-generate-pass').click()
        },
        error: function(resp) {
            alert(resp)
        }
    })
})

$('.modalUpdateUserView').on('click', function() {
    idUserUpdate = $(this).attr('data-idUser')
    getUpdateUserView(idUserUpdate)
})

$('.modalStatusUserView').on('click', function() {
    idUserUpdateStatus = $(this).attr('data-idUser')
    getUpdateStatusUserView($(this).attr('data-idUser'))
})

$('.modalDeleteUserView').on('click', function() {
    idUserDeleteUser = $(this).attr('data-idUser')
    getDeleteUserView($(this).attr('data-idUser'))
})


function getUpdateUserView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Actualizar información de usuario')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnUpdateUserByProject' type='button' disabled><span class='align-middle material-icons'>save</span>&nbsp;Actualizar</button>")
    $.ajax({
        type: 'GET',
        url: `${siteurl}UserByProject/Update?id=${id}`,
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function(resp) {
            alert(resp)
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
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function(resp) {
            alert(resp)
        }
    })
}

function getUpdateStatusUserView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('¿Actualizar estatus de usuario?')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnUpdateStatusUserByProject' type='button'><span class='align-middle material-icons'>update</span>&nbsp;Actualizar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + `UserByProject/ChangeStatus/${id}`,
        success: function(resp) {
            if (resp.status == 500) {
                window.location.href = '/Home/Error'
                return;
            }
            $(".modal-body").empty().append(resp)
            $('#viewsLoader').hide()
            $('.modal-dialog').fadeIn()
        },
        error: function(resp) {
            alert(resp)
        }
    })
}