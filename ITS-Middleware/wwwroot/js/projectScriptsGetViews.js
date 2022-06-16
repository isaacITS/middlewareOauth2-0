//GET PARTIALS VIEWS PROJECTS (CRUD VIEWS)
$('#modalRegisterProjectView').on('click', () => {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Registrar proyecto')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnRegisterProject' type='button'><span class='align-middle material-icons'>save</span>&nbsp;Registrar</button>")
    $.ajax({
        type: 'GET',
        url: siteurl + 'Project/Register/',
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

$('.modalUpdateProjectView').on('click', function () {
    getUpdateProjectView($(this).attr('data-idProject'))
})

$('.modalDeleteProjectView').on('click', function () {
    getDeleteProjecctView($(this).attr('data-idProject'))
    idProjectDelete = $(this).attr('data-idProject')
})

$('.modalStatusProjectView').on('click', function () {
    getUpdateStatusProjectView($(this).attr('data-idProject'))
})

function getUpdateProjectView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('Actualizar información de proyecto')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnUpdateProject' type='button'><span class='align-middle material-icons'>save</span>&nbsp;Actualizar</button>")
    $.ajax({
        type: 'GET',
        url: `${siteurl}Project/EditProject/${id}`,
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista editar proyecto")
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

function getDeleteProjecctView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('¿Eliminar Proyecto?')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-danger' id='btnDeleteProject' type='button'><span class='align-middle material-icons'>delete_forever</span>&nbsp;Eliminar</button>")
    $.ajax({
        type: 'GET',
        url: `${siteurl}Project/DeleteProject/${id}`,
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista eliminar proyecto")
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

function getUpdateStatusProjectView(id) {
    $('.modal-dialog').fadeOut(1)
    $('#viewsLoader').show()
    $('.modal-title').html('¿Actualizar estatus de proyecto?')
    $('.modal-footer').html("<button type='button' class='btn btn-outline-secondary btn-close-modal-view' data-bs-dismiss='modal'><span class='align-middle material-icons'>close</span>&nbsp;Cancelar</button><button class='btn btn-success' id='btnUpdateStatusProject' type='button'><span class='align-middle material-icons'>update</span>&nbsp;Actualizar</button>")
    $.ajax({
        type: 'GET',
        url: `${siteurl}Project/UpdateStatus/${id}`,
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error'
                console.log("Error al obtener vista cambiar estatus de proyecto")
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