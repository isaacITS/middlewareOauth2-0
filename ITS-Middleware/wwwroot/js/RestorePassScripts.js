var email;

$(document).ready(() => {
    $('#btnSendEmailRestorePass').on('click', () => {
        if (checkDataSendEmail().ok) {
            email = $('#email').val()
            $.ajax({
                type: 'POST',
                url: `${siteurl}Auth/GenerateToken/`,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(email),
                success: function (response) {
                    if (response.status == 500) {
                        window.location.href = '/Home/Error'
                        console.log(response)
                        return;
                    } else if (response.status == 205) {
                        $('.go-to-login-page').click()
                        ShowToastMessage('warning', 'Usuario deshabilitado', response.msg)
                    } else if (response.status == 200) {
                        $('.go-to-login-page').click()
                        ShowToastMessage('success', `Correo de recuperación enviado`, response.msg)
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


var validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$");

$('#email').change(function () {
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