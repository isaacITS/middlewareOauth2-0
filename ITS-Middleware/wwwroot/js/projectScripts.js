var idProjectUpdate;
var idProjectDelete;

$(document).ready(() => {
    $('#btnRegisterProject').on('click', () => {
        if (validateData().ok) {
            var formData = $('#registerProjectForm').serialize()
            $.ajax({
                type: 'POST',
                url: siteurl + 'Project/Register',
                content: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(formData),
                success: function (response) {
                    if (response.msg == "Error") {
                        console.log(response)
                        window.location.href = '/Home/Error';
                        return;
                    } else if (response != null) {
                        if (!response.ok) {
                            closeModal()
                            ShowToastMessage('error', 'No se registro el proyecto', response.msg)
                        } else {
                            closeModal()
                            ShowToastMessage('success', 'Proyecto registrado', response.msg)
                            GetProjectsList()
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
            ShowToastMessage('error', 'Datos inválidos o incompletos', validateData().msg)
        }
    });

    $('#btnUpdateProject').on('click', function () {
        var formData = $('#updateProjectForm').serialize()
        if (validUpdateDataProject().ok) {
            $.ajax({
                type: 'post',
                url: siteurl + 'Project/UpdateProject/',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    if (!response.ok) {
                        window.location.href = '/Home/Error';
                        return;
                    } else if (response.ok) {
                        closeModal()
                        ShowToastMessage('success', 'Proyecto actualizado', response.msg);
                        GetProjectsList()
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

    $('#btnUpdateStatusProject').on('click', function () {
        var formData = $('#updateStatusProjectForm').serialize()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/UpdateStatus/',
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
                    GetProjectsList()
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

    $('#btnDeleteProject').on('click', function () {
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/DeleteProject/',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idProjectDelete,
            success: function (response) {
                if (!response.ok) {
                    window.location.href = '/Home/Error';
                    return;
                } else if (response.ok) {
                    closeModal()
                    ShowToastMessage('success', 'Proyecto eliminado', response.msg)
                    GetProjectsList()
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
    if ($('#Nombre').val() == '' || $('#Descripcion').val() == '' || $('#Usuario').val() == '' || $('#Pass').val() == '') {
        return {
            ok: false,
            msg: "Se deben llenar todos los campos"
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

function validUpdateDataProject() {
    if ($('#Nombre').val() == '' || $('#Descripcion').val() == '' || $('#Usuario').val() == '') {
        return {
            ok: false,
            msg: "Se deben llenar todos los campos"
        }
    }
    return {
        ok: true,
        msg: "OK"
    }
}