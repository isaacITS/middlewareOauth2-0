var userIdUpdate;

$(document).ready(function () {
    $('#ButtonUpdateUser').on('click', function () {
        if ($('#name').val() == '') {
            ShowToastMessage('error', 'Ahora', "Nombre de usuario requerido");
            return;
        }
        var formData = $('#EditUserForm').serialize();
        $.ajax({
            type: 'post',
            url: siteurl + 'Home/UpdateUser/',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function (response) {
                if (response == "Error") {
                    window.location.href = '/Main/Error';
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

function GetUsersList() {
    $('#contentView').fadeOut(1);
    $('#viewsLoader').show();
    $.ajax({
        type: 'GET',
        url: siteurl + 'Home/Users',
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Main/Error';
                return;
            }
        },
        error: function (resp) {
            console.log(resp);
        }
    });
}