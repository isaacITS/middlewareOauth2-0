var idUserDelete

//CRUD REQUEST
$(document).ready(() => {
    $('#btnRegisterUser').on('click', function() {
        if (validateData().ok) {
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
        } else {
            ShowToastMessage('error', 'Datos no válidos', validateData().msg)
        }
    });

    $('#btnUpdateUser').on('click', function () {
        var formData = $('#updateUserForm').serialize()
        if (validUpdateData().ok) {
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
        } else {
            ShowToastMessage('error', 'Datos no válidos', validateData().msg)
        }
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


//Validation Functions
function validateData() {
    const validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$");
    if ($('#Nombre').val() == '' || $('#Email').val() == '' || $('#Puesto').val() == '' || $('#Pass').val() == '') {
        return {
            ok: false,
            msg: "Se deben llenar todos los campos"
        }
    }
    if (!validEmail.test($('#Email').val())) {
        $('#Email').css('background-color', 'rgba(209, 0, 0, 0.26)')
        return {
            ok: false,
            msg: "Ingresa un correo válido"
        }
    }
    if ($('#Pass').val().length < 8) {
        $('#Pass').css('background-color', 'rgba(209, 0, 0, 0.26)')
        return {
            ok: false,
            msg: "Ingresa una contraseña segura"
        }
    }
    return {
        ok: true,
        msg: "OK"
    }
}

function validUpdateData() {
    const validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$");
    if ($('#Nombre').val() == '' || $('#Email').val() == '' || $('#Puesto').val() == '') {
        return {
            ok: false,
            msg: "Se deben llenar todos los campos"
        }
    }
    if (!validEmail.test($('#Email').val())) {
        $('#Email').css('background-color', 'rgba(209, 0, 0, 0.26)')
        return {
            ok: false,
            msg: "Ingresa un correo válido"
        }
    }
    return {
        ok: true,
        msg: "OK"
    }
}