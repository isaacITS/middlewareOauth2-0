var idUserDelete
var isToUpdate = false

//CRUD REQUEST
$(document).ready(() => {
    $('#btn-action-pass').click()
    $('#btnRegisterUser').on('click', function() {
            $('#viewsLoader').show()
            $('.modal').hide()
            var formData = $('#registerUserForm').serialize()
            $.ajax({
                type: 'POST',
                url: siteurl + 'User/Register',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    $('#viewsLoader').hide()
                    if (response.status == 500) {
                        window.location.href = '/Home/Error';
                        return;
                    } else if (response.status == 410) {
                        closeModal()
                        ShowToastMessage('warning', 'Usuario NO registrado', response.msg)
                    } else {
                        closeModal()
                        ShowToastMessage('success', '!Usuario registrado¡', response.msg)
                        GetUsersList()
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

    $('#btnUpdateUser').on('click', function () {
        var formData = $('#updateUserForm').serialize()
            $('#viewsLoader').show()
            $('.modal').hide()
            $.ajax({
                type: 'post',
                url: siteurl + 'User/UpdateUser/',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    $('#viewsLoader').hide()
                    if (response.status == 500) {
                        window.location.href = '/Home/Error';
                        return;
                    } else if (response.ok) {
                        closeModal()
                        ShowToastMessage('success', 'Usuario actualizado', response.msg);
                        GetUsersList()
                    } else {
                        closeModal()
                        ShowToastMessage('error', 'No se actualizó el usuario', response.msg);
                        GetUsersList()
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

    $('#btnUpdateStatusUser').on('click', function () {
        var formData = $('#updateStatusUserForm').serialize()
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'User/UpdateStatus/',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function (response) {
                $('#viewsLoader').hide()
                if (response.status == 500) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Estatus de usaurio actualizado', response.msg)
                    GetUsersList()
                } else {
                    closeModal()
                    ShowToastMessage('error', 'No se actualizó el estatus', response.msg)
                    GetUsersList()
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

    $('#btnDeleteUser').on('click', function () {
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'User/DeleteUserPost/',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idUserDelete,
            success: function (response) {
                $('#viewsLoader').hide()
                if (response.status == 500) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Usuario eliminado', response.msg)
                    GetUsersList()
                } else {
                    closeModal()
                    ShowToastMessage('error', 'No se eliminó el usuario', response.msg)
                    GetUsersList()
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
var puestoIsValid = false
var emailIsValid = false
var passIsValid = false
var validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$")

//Validation Functions
$('#Nombre, #Puesto, #Email, #Pass').on('change keyup paste', () => {
    if (nameIsValid && puestoIsValid && emailIsValid && passIsValid) {
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
    if ($('#Puesto').val() == "" || $('#Puesto').val().length < 3) {
        $('#Puesto').css('background-color', '#f700000c')
        $('#messagePuesto').html('Ingresa el puesto del usuario a registrar')
        puestoIsValid = false
    } else {
        $('#Puesto').css('background', 'none')
        $('#messagePuesto').html('')
        puestoIsValid = true
    }
    if ($('#Pass').val() == "" && !isToUpdate) {
        $('#Pass').css('background-color', '#f700000c')
        $('#messagePass').html('Ingresa una contraseña para el usuario')
        passIsValid = false
    } else if ($('#Pass').val().length < 8 && !isToUpdate) {
        $('#Pass').css('background-color', '#f700000c')
        $('#messagePass').html('Ingresa una contraseña segura para el usuario (mayor o igual a 8 caracteres)')
        passIsValid = false
    } else if ($('#Pass').val().length < 8 && $('#Pass').val().length > 0 && isToUpdate) {
        $('#Pass').css('background-color', '#f700000c')
        $('#messagePass').html('Ingresa una contraseña segura (mayor o igual a 8 caracteres)')
        passIsValid = false
    }else {
        $('#Pass').css('background', 'none')
        $('#messagePass').html('')
        passIsValid = true
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
