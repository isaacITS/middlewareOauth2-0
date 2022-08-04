var idProjectUpdate;
var idProjectDelete;
var methodsListLength = $('#authMethodsLength').val()
var imageFile;
var listMethodsAuth


$(document).ready(() => {
    $('#1').prop('checked', true)
    $("#1").attr('disabled', 'disabled')
    $("#1").attr('readonly', 'readonly')

    $('#imageSaved').hide()
    $('.image-saved').append('<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Cargando...</span></div>')
    $('#imageSaved').on('load', () => {
        $('.spinner-border').remove()
        $('#imageSaved').show()
    })

    listMethodsAuth = $('.form-check-input').map(function() {
        return this.id
    }).get()

    $('#btnRegisterProject').on('click', function() {
        getMethodsList()
        $('#viewsLoader').show()
        $('.modal').hide()
        var formData = new FormData($('#registerProjectForm')[0])
        formData.append('ImageFile', imageFile)
        $.ajax({
            type: 'POST',
            url: siteurl + 'Project/Register',
            contentType: false,
            processData: false,
            data: formData,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                if (response.status == 500) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    GetProjectsList()
                }
            },
            failure: function(response) {
                console.log(response.responseText)
                alert(response.responseText);
            },
            error: function(response) {
                console.log(response.responseText)
                alert(response.responseText);
            }
        });
    });

    $('#btnUpdateProject').on('click', function() {
        getMethodsList()
        var formData = new FormData($('#updateProjectForm')[0])
        formData.append('ImageFile', imageFile)
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/UpdateProject',
            contentType: false,
            processData: false,
            data: formData,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                if (response.status == 500) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    GetProjectsList()
                }
            },
            failure: function(response) {
                alert(response.responseText);
            },
            error: function(response) {
                alert(response.responseText);
            }
        });
    });

    $('#btnUpdateStatusProject').on('click', function() {
        var formData = $('#updateStatusProjectForm').serialize()
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/UpdateStatusPost',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                if (response.status == 500) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    GetProjectsList()
                }
            },
            failure: function(response) {
                alert(response.responseText);
            },
            error: function(response) {
                alert(response.responseText);
            }
        });
    });

    $('#btnDeleteProject').on('click', function() {
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/DeleteProjectPost/',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idProjectDelete,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                if (response.status == 500) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    GetProjectsList()
                }
            },
            failure: function(response) {
                alert(response.responseText);
            },
            error: function(response) {
                alert(response.responseText);
            }
        });
    });
})


var nameIsValid = false
var linkIsValid = false
var descIsValid = false
var urlValidate = /^[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/i
var urlValidate2 = /^(http|https|ftp):\/\/[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/i

//Validation Functions
$('#Nombre, #Descripcion, #Enlace, input[type="checkbox"], #ImageFile').on('change keyup paste', () => {
    if ($('#Nombre').val() == "" || $('#Nombre').val().length == 1) {
        $('#Nombre').css('background-color', '#f700000c')
        $('#messageNombre').html('Ingresa un nombre para el proyecto')
        nameIsValid = false
    } else {
        $('#Nombre').css('background', 'none')
        $('#messageNombre').html('')
        nameIsValid = true
    }
    if ($('#Descripcion').val() == "" || $('#Descripcion').val().length < 20) {
        $('#Descripcion').css('background-color', '#f700000c')
        $('#messageDescripcion').html('Ingresa una descripción para el proyecto (mínimo 20 caracteres)')
        descIsValid = false
    } else {
        $('#Descripcion').css('background', 'none')
        $('#messageDescripcion').html('')
        descIsValid = true
    }
    if ($('#Enlace').val() == "" || $('#Enlace').val().length < 5 || (!urlValidate.test($('#Enlace').val()) && !urlValidate2.test($('#Enlace').val()))) {
        $('#Enlace').css('background-color', '#f700000c')
        $('#messageEnlace').html('Ingresa un enlace válido (ej: https://www.link.com, http:www.link.com, www.link.com)')
        linkIsValid = false
    } else {
        $('#Enlace').css('background', 'none')
        $('#messageEnlace').html('')
        linkIsValid = true
    }
    if (nameIsValid && linkIsValid && descIsValid) {
        $('.btn-success').prop('disabled', false)
    } else {
        $('.btn-success').prop('disabled', true)
    }
})



//Getting the authentication methods ID selected
function getMethodsList() {
    var listMetods = ""
    listMethodsAuth.forEach(element => {
        if ($(`#${element}`).is(':checked')) {
            listMetods += $(`#${element}`).val() + ','
        }
    })
    if (listMetods.charAt(listMetods.length - 1) == ',') {
        listMetods = listMetods.slice(0, -1)
    }
    $('#auth-method-list').val(listMetods)
}


function readURL(input) {
    if (input.files && input.files[0]) {
        imageFile = $('#ImageFile').prop('files')[0]
        var reader = new FileReader();
        reader.onload = function(e) {
            $('.image-upload-wrap').hide();
            $('.file-upload-image').attr('src', e.target.result);
            $('.file-upload-content').show();
            $('.image-title').html(input.files[0].name);
        };
        reader.readAsDataURL(input.files[0]);
    } else {
        removeUpload();
    }
}

function removeUpload() {
    $('.file-upload-input').replaceWith($('.file-upload-input').clone());
    $('.file-upload-content').hide();
    $('.image-upload-wrap').show();
}
$('.image-upload-wrap').bind('dragover', function() {
    $('.image-upload-wrap').addClass('image-dropping');
});
$('.image-upload-wrap').bind('dragleave', function() {
    $('.image-upload-wrap').removeClass('image-dropping');
});