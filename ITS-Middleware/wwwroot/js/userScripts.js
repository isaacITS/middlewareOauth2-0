﻿//CRUD REQUEST
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
                            ShowToastMessage('warning', 'Correo no válido', `El correo ${$('#Email').val()} ya esta en uso`)
                        } else {
                            $('.btn-close-modal-views').trigger('click')
                            ShowToastMessage('success', 'Usuario registrado', "Se ha registrado el usuario correctamente")
                            GetUsersList();
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

    $('#ButtonUpdateUser').on('click', function () {
        var formData = $('#updateUserForm').serialize()
        $.ajax({
            type: 'post',
            url: siteurl + 'User/EditUser/',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function (response) {
                if (response == "Error") {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response != null) {
                    ShowToastMessage('success', 'Ahora', "Usuario actualizado correctamente");
                    GetUsersList();
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