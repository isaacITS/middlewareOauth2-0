$('#sendEmailRestorePass').on('click', function () {
    $('#sendEmailRestorePass').prop('disabled', true)
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