const validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$");
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

$(document).ready(() => {
    if ($('#alertMessage').val().length > 0) {
        ShowToastMessage('error', 'Credenciales incorrectas', $('#alertMessage').val())
    }
})