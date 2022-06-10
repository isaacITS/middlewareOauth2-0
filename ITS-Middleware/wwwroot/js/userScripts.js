
$(document).ready(() => {
    GetUsersList();
    $('#btnRegisterUser').on('click', () => {
        var formData = $('#RegisterUserForm').serialize();
        $.ajax({
            type: 'post',
            url: '/User/Register/',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function (response) {
                if (response == "Error") {
                    window.location.href = '/Error';
                    return;
                } else if (response != null) {
                    console.log(response)
                    $('#registerUserForm').remove()
                    ShowToastMessage('success', 'Ahora', "Usuario registrado correctamente");
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
    $('#usersTableContent').fadeOut(1);
    $.ajax({
        type: 'GET',
        url: siteurl + '/Home/Users',
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Error';
                return;
            }
            $("#usersTableContent").empty().append(resp).hide();
            $("#usersTableContent").fadeIn();
        },
        error: function (resp) {
            console.log(resp);
        }
    });
}