var idUserDeleteUser
var idUserUpdate

//CRUD REQUEST
$(document).ready(() => {
    $('#btnRegisterUserByProject').on('click', function () {
        if (validateData().ok) {
            $('#viewsLoader').show()
            var formData = $('#registerUserByProjectForm').serialize()
            $.ajax({
                type: 'POST',
                url: siteurl + 'UserByProject/Register',
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
                        ShowToastMessage('warning', 'Usuario no registrado', response.msg)
                    } else {
                        closeModal()
                        ShowToastMessage('success', '!Usuario registrado¡', response.msg)
                        getUserByProjectList()
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

    $('#btnUpdateUserByProject').on('click', function () {
        var formData = $('#updateUserByProjectForm').serialize()
        if (validateDataUpdate().ok) {
            $('#viewsLoader').show()
            $.ajax({
                type: 'post',
                url: siteurl + 'UserByProject/Update',
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
                        ShowToastMessage('success', 'Información de usaurio actualizada', response.msg);
                        getUserByProjectList()
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
                    getUserByProjectList()
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

    $('#btnDeleteUserByProject').on('click', function () { 
        $('#viewsLoader').show()
        $.ajax({
            type: 'post',
            url: siteurl + 'UserByProject/Delete',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idUserDeleteUser,
            success: function (response) {
                $('#viewsLoader').hide()
                if (response.status == 500) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Usuario eliminado', response.msg)
                    getUserByProjectList()
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
    if ($('#Nombre').val() == '' || $('#Email').val() == '' || $('#Pass').val() == '') {
        return {
            ok: false,
            msg: "Se deben llenar todos los campos"
        }
    }
    if (!validEmail.test($('#Email').val())) {
        $('#Email').css('background-color', 'rgba(209, 0, 0, 0.26)')
        return {
            ok: false,
            msg: "Debe ingresar un correo válido"
        }
    }
    if ($('#Pass').val().length < 8) {
        $('#Pass').css('background-color', 'rgba(209, 0, 0, 0.26)')
        return {
            ok: false,
            msg: "Se recomienda ingresar una contraseña segura"
        }
    }
    return {
        ok: true,
        msg: "OK"
    }
}

function validateDataUpdate() {
    const validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$");
    if ($('#Nombre').val() == '' || $('#Email').val() == '') {
        return {
            ok: false,
            msg: "Se deben llenar todos los campos"
        }
    }
    if (!validEmail.test($('#Email').val())) {
        $('#Email').css('background-color', 'rgba(209, 0, 0, 0.26)')
        return {
            ok: false,
            msg: "Debe ingresar un correo válido"
        }
    }
    if ($('#Pass').val() != '0000000000') {
        if ($('#Pass').val().length < 8) {
            $('#Pass').css('background-color', 'rgba(209, 0, 0, 0.26)')
            return {
                ok: false,
                msg: "Se recomienda ingresar una contraseña segura"
            }
        }
    }
    return {
        ok: true,
        msg: "OK"
    }
}
