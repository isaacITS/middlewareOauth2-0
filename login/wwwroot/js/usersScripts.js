$('#sendEmailRestorePass').on('click', function () {
    $('#sendEmailRestorePass').prop('disabled', true)
    $('.closeModalButton').prop('disabled', true)
    $('#sendEmailRestorePass').html('<span class="spinner-grow spinner-grow-sm" role="status" aria-hidden="true"></span> Enviando...')
    var email = $('#emailResPass').val()
    var projectName = $('#projectName').val()
    var projectImage = $('#projectImageUrl').val()
    var data = `email=${email}&projectName=${projectName}&projectImage=${projectImage}`
    $.ajax({
        type: 'post',
        url: siteurl + 'Home/SendEmailRes/',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        data: data,
        success: function (response) {
            $('.closeModalButton').prop('disabled', false)
            $('.btn-close-modal').click()
            $('#sendEmailRestorePass').html('<i class="bi bi-send-check-fill"></i>&nbsp;Enviar correo')
            if (response.status == 500) {
                window.location.href = '/Home/Error';
                return;
            } else if (response.status == 400) {
                ShowToastMessage('error', response.msgHeader, response.msg)
            } else {
                ShowToastMessage('success', response.msgHeader, response.msg)
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

$('#buttonUpdatePass').on('click', function () {
    $('#box-update-pass').removeClass('visually-hidden')
    var email = $('#email-restore').val()
    var token = $('#token-restore').val()
    var tokenJwt = $('#tokenJwt-restore').val()
    var newPass = $('#newPassword').val()
    var data = `email=${email}&tokenJwt=${tokenJwt}&token=${token}&newPass=${newPass}`
    $.ajax({
        type: 'post',
        url: siteurl + 'Home/UpdatePassword/',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        data: data,
        success: function (response) {
            $('#newPassword').val('')
            $('#passwordConfirm').val('')
            $('#box-update-pass').addClass('visually-hidden')
            if (response.status == 500) {
                window.location.href = '/Home/Error';
                return;
            } else if (response.status == 400) {
                ShowToastMessage('error', response.msgHeader, response.msg)
            } else {
                ShowToastMessage('success', response.msgHeader, response.msg)
                setTimeout(function () {
                    window.location.href = `${siteurl}?project=${$('#projectName-restore').val()}`
                }, 5000);
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

var showPass = true
function showHidePass() {
    if (showPass) {
        $('#pass1, #pass2').attr('class', 'bi bi-eye-fill icon-password')
        $('#newPassword, #passwordConfirm').attr('type', 'text')
        showPass = false
    } else {
        $('#pass1, #pass2').attr('class', 'bi bi-eye-slash-fill icon-password')
        $('#newPassword, #passwordConfirm').attr('type', 'password')
        showPass = true
    }
}

$('#newPassword, #passwordConfirm').on('change keyup paste input', () => {
    if ($('#newPassword').val().length == 0 || $('#newPassword').val() == '') {
        $('#warning-message').html('<i class="bi bi-exclamation-circle-fill"></i>&nbsp;Ingresa una nueva contraseña para tu cuenta')
        $('#buttonUpdatePass').prop('disabled', true)
    } else if ($('#newPassword').val().length < 8) {
        $('#buttonUpdatePass').prop('disabled', true)
        $('#warning-message').html('<i class="bi bi-exclamation-circle-fill"></i>&nbsp;Ingresa una contraseña segura (al menos 8 caracteres)')
    } else if ($('#newPassword').val() != $('#passwordConfirm').val()) {
        $('#buttonUpdatePass').prop('disabled', true)
        $('#warning-message').html('<i class="bi bi-exclamation-circle-fill"></i>&nbsp;Las contraseñas ingresadas no coinciden, verificalas')
    } else {
        $('#buttonUpdatePass').prop('disabled', false)
        $('#warning-message').html('')
    }
})