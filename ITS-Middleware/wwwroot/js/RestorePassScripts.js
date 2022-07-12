var email;

$(document).ready(() => {
    $('#btnSendEmailRestorePass').on('click', () => {
        if (checkDataSendEmail().ok) {
            $('#viewsLoader').show()
            $('.form-restore-pass').hide()
            email = $('#email').val()
            $.ajax({
                type: 'POST',
                url: `${siteurl}Auth/GenerateToken`,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(email),
                success: function (response) {
                    $('#viewsLoader').hide()
                    $('.form-restore-pass').show()
                    if (response.status == 500) {
                        window.location.href = '/Home/Error'
                        console.log(response)
                        return;
                    } else if (response.status == 205) {
                        ShowToastMessage('warning', 'Usuario deshabilitado', response.msg)
                    } else if (response.status == 200) {
                        ShowToastMessage('success', `Correo de recuperación enviado`, response.msg)
                        redirectToLogin()
                    } else {
                        ShowToastMessage('error', `No se encontro el usuario`, response.msg)
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
            ShowToastMessage('error', 'Datos no válidos', checkDataSendEmail().msg)
        }
    })

    $('#btnUpdatePassword').on('click', function () {
        $('.update-password-form').hide()
        $('#viewsLoader').show()
        var formData = $('#restorePasswordForm').serialize()
            $.ajax({
                type: 'post',
                url: siteurl + 'Auth/UpdatePass',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    $('#viewsLoader').hide()
                    $('.update-password-form').show()
                    if (response.status == 500) {
                        window.location.href = '/Home/Error'
                        console.log(response)
                        return;
                    } else if (response.status == 200) {
                        ShowToastMessage('success', 'Contraseña actualizada', response.msg)
                        redirectToLogin()
                    } else {
                        ShowToastMessage('error', `Ocurrió un error con los datos`, response.msg)
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
    })
})


function checkDataSendEmail() {
    if ($('#email').val() == '') {
        return {
            ok: false,
            msg: "Debe ingresar un correo electrónico"
        }
    }
    return {
        ok: true,
        msg: "OK"
    }
}


$('#email').change(function () {
    const validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$");
    if (validEmail.test($('#email').val())) {
        $('#email').css('background', 'none')
        $('#email').css('color', '#202020')
        $('small').css('display', 'none')
        $('.btn-login-portal').prop('disabled', false)
        $('.btn-send-email').prop('disabled', false)
        $('#icon-status-email').text('check')
        $('#icon-status-email').css('color', "#12b100")
    } else {
        $('#email').css('background-color', 'rgba(209, 0, 0, 0.26)')
        $('#email').css('color', '#911A00')
        $('small').css('display', 'block')
        $('.btn-login-portal').prop('disabled', true)
        $('.btn-send-email').prop('disabled', true)
        $('#icon-status-email').text('clear')
        $('#icon-status-email').css('color', "#C12300")
    }
})