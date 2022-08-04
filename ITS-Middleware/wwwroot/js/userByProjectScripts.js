var idUserDeleteUser
var idUserUpdate
var idUserUpdateStatus
var isToUpdate = false

//CRUD REQUEST
$(document).ready(() => {
    $('#btn-action-pass').click()
    $('#btnRegisterUserByProject').on('click', function() {
        $('#viewsLoader').show()
        $('.modal').hide()
        var formData = $('#registerUserByProjectForm').serialize()
        $.ajax({
            type: 'POST',
            url: siteurl + 'UserByProject/Register',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                if (response.status == 500) {
                    window.location.href = '/Home/Error'
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    getUserByProjectList()
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

    $('#btnUpdateUserByProject').on('click', function() {
        var formData = $('#updateUserByProjectForm').serialize()
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'UserByProject/Update',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                console.log(response)
                if (response.status == 500) {
                    window.location.href = '/Home/Error'
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    getUserByProjectList()
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

    $('#btnUpdateStatusUserByProject').on('click', function() {
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'UserByProject/UpdateStatus',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idUserUpdateStatus,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                if (response.status == 500) {
                    window.location.href = '/Home/Error'
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    getUserByProjectList()
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

    $('#btnDeleteUserByProject').on('click', function() {
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'UserByProject/DeletePost',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idUserDeleteUser,
            success: function(response) {
                $('#viewsLoader').hide()
                closeModal()
                if (response.status == 500) {
                    window.location.href = '/Home/Error'
                    return;
                } else if (response.status == 400) {
                    ShowToastMessage('error', response.msgHeader, response.msg)
                } else {
                    ShowToastMessage('success', response.msgHeader, response.msg)
                    getUserByProjectList()
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


//Validation Functions
var nameIsValid = false
var emailIsValid = false
var passIsValid = false
var validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$")

//Validation Functions
$('#Nombre, #Email, #Pass, #Telefono').on('change keyup paste', () => {
    if (nameIsValid && emailIsValid && passIsValid) {
        $('.btn-success').prop('disabled', false)
    } else {
        $('.btn-success').prop('disabled', true)
    }
    if ($('#Nombre').val() == "" || $('#Nombre').val().length < 3) {
        $('#Nombre').css('background-color', '#f700000c')
        $('#messageNombre').html('Ingresa un nombre para el usuario')
        nameIsValid = false
    } else {
        $('#Nombre').css('background', 'none')
        $('#messageNombre').html('')
        nameIsValid = true
    }
    if ($('#Pass').val() == "" && !isToUpdate) {
        $('#Pass').css('background-color', '#f700000c')
        $('#messagePass').html('Ingresa una contraseña para el usuario')
        passIsValid = false
    } else if ($('#Pass').val().length < 8 && !isToUpdate) {
        $('#Pass').css('background-color', '#f700000c')
        $('#messagePass').html('Ingresa una contraseña segura para el usuario (mayor o igual a 8 caracteres)')
        passIsValid = false
    } else if ($('#Pass').val().length > 0 && $('#Pass').val().length < 8 && isToUpdate) {
        $('#Pass').css('background-color', '#f700000c')
        $('#messagePass').html('Ingresa una contraseña segura (mayor o igual a 8 caracteres)')
        passIsValid = false
    } else {
        $('#Pass').css('background', 'none')
        $('#messagePass').html('')
        passIsValid = true
    }
    if ($('#Telefono').val().length > 0) {
        $('#Telefono').val($('#Telefono').val().replace(/\D/g, ''))
    }
    if ($('#Email').val() == "" || $('#Email').val().length < 5 || !validEmail.test($('#Email').val())) {
        $('#Email').css('background-color', '#f700000c')
        $('#messageEmail').html('Ingresa un correo electrónico válido (ej: username@domain.ext)')
        emailIsValid = false
    } else {
        $('#Email').css('background', 'none')
        $('#messageEmail').html('')
        emailIsValid = true
    }
})