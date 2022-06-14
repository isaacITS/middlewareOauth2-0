var idUserDelete

//CRUD REQUEST
$(document).ready(() => {
    $('#btnRegisterUser').on('click', () => {
        if (validateData().ok) {
            var formData = $('#registerUserForm').serialize()
            $.ajax({
                type: 'POST',
                url: siteurl + 'User/Register',
                content: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(formData),
                success: function (response) {
                    if (response == "Error") {
                        console.log(response)
                        //window.location.href = '/Home/Error';
                        return;
                    } else if (response != null) {
                        if (response == "Email Existe") {
                            closeModal()
                            ShowToastMessage('warning', 'Correo no válido', `El correo ${$('#Email').val()} ya esta en uso`)
                        } else {
                            closeModal()
                            ShowToastMessage('success', 'Usuario registrado', "Se ha registrado el usuario correctamente")
                            GetUsersList()
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
        } else {
            ShowToastMessage('error', 'Datos no válidos', validateData().msg)
        }
    });

    $('#btnUpdateUser').on('click', function () {
        var formData = $('#updateUserForm').serialize()
        if (validUpdateData().ok) {
            $.ajax({
                type: 'post',
                url: siteurl + 'User/UpdateUser/',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    if (!response.ok) {
                        window.location.href = '/Home/Error';
                        return;
                    } else if (response.ok) {
                        closeModal()
                        ShowToastMessage('success', 'Usuario actualizado', response.msg);
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
        $.ajax({
            type: 'post',
            url: siteurl + 'User/UpdateStatus/',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function (response) {
                if (!response.ok) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Estatus actualizado', response.msg)
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
        $.ajax({
            type: 'post',
            url: siteurl + 'User/DeleteUser/',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idUserDelete,
            success: function (response) {
                if (!response.ok) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Usuario eliminado', response.msg)
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
    var validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$")
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
    var validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$")
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