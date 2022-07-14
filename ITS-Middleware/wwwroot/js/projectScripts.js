var idProjectUpdate;
var idProjectDelete;
var methodsListLength = $('#authMethodsLength').val()


$(document).ready(() => {
    $('#1').prop('checked', true)
    $("#1").attr('disabled', 'disabled')
    $("#1").attr('readonly', 'readonly')

    $('#btnRegisterProject').on('click', function () {
        getMethodsList() 
            $('#viewsLoader').show()
            $('.modal').hide()
            var formData = $('#registerProjectForm').serialize()
            $.ajax({
                type: 'POST',
                url: siteurl + 'Project/Register',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    $('#viewsLoader').hide()
                    if (response.msg == "Error") {
                        window.location.href = '/Home/Error';
                        return;
                    } else if (response != null) {
                        if (!response.ok) {
                            closeModal()
                            ShowToastMessage('error', 'No se registro el proyecto', response.msg)
                        } else {
                            closeModal()
                            ShowToastMessage('success', 'Proyecto registrado', response.msg)
                            GetProjectsList()
                        }
                    }
                },
                failure: function (response) {
                    console.log(response.responseText)
                    alert(response.responseText);
                },
                error: function (response) {
                    console.log(response.responseText)
                    alert(response.responseText);
                }
            });
    });

    $('#btnUpdateProject').on('click', function () {
        getMethodsList() 
        var formData = $('#updateProjectForm').serialize()
        $('#viewsLoader').show()
        $('.modal').hide()
            $.ajax({
                type: 'post',
                url: siteurl + 'Project/UpdateProject',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    $('#viewsLoader').hide()
                    if (!response.ok) {
                        window.location.href = '/Home/Error';
                        return;
                    } else if (response.ok) {
                        closeModal()
                        ShowToastMessage('success', 'Proyecto actualizado', response.msg);
                        GetProjectsList()
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
    });

    $('#btnUpdateStatusProject').on('click', function () {
        var formData = $('#updateStatusProjectForm').serialize()
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/UpdateStatusPost',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function (response) {
                $('#viewsLoader').hide()
                if (!response.ok) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Estatus actualizado', response.msg)
                    GetProjectsList()
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });

    $('#btnDeleteProject').on('click', function () {
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/DeleteProjectPost/',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idProjectDelete,
            success: function (response) {
                $('#viewsLoader').hide()
                if (!response.ok) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Proyecto eliminado', response.msg)
                    GetProjectsList()
                }
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
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
$('#Nombre, #Descripcion, #Enlace').on('change keyup paste', () => {
    if (nameIsValid && linkIsValid && descIsValid) {
        $('.btn-success').prop('disabled', false)
    } else {
        $('.btn-success').prop('disabled', true)
    }
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
    if ($('#Enlace').val() == "" || $('#Enlace').val().length < 5 || (!urlValidate.test($('#Enlace').val()) && !urlValidate2.test($('#Enlace').val())) ) {
        $('#Enlace').css('background-color', '#f700000c')
        $('#messageEnlace').html('Ingresa un enlace válido (ej: https://www.link.com, http:www.link.com, www.link.com)')
        linkIsValid = false
    } else {
        $('#Enlace').css('background', 'none')
        $('#messageEnlace').html('')
        linkIsValid = true
    }
})



//Getting the authentication methods ID selected
function getMethodsList() {
    var listMetods = ""
    for (let i = 1; i <= methodsListLength; i++) {
        if ($(`#${i}`).is(':checked')) {
            listMetods += $(`#${i}`).val() + ','
        }
    }
    if (listMetods.charAt(listMetods.length - 1) == ',') {
        listMetods = listMetods.slice(0, -1)
    }
    $('#auth-method-list').val(listMetods)
}
