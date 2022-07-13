var idProjectUpdate;
var idProjectDelete;
var methodsListLength = $('#authMethodsLength').val()

$(document).ready(() => {
    $('#1').prop('checked', true)
    $("#1").attr('disabled', 'disabled')
    $("#1").attr('readonly', 'readonly')

    $('#btnRegisterProject').on('click', function () {
        getMethodsList() 
        if (validateData().ok) {
            $('#viewsLoader').show()
            $('.modal').hide()
            var formData = $('#registerProjectForm').serialize()
            $.ajax({
                type: 'POST',
                url: siteurl + 'Project/Register',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    $('#viewsLoader').hide()
                    if (response.msg == "Error") {
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
        getMethodsList() 
        var formData = $('#updateProjectForm').serialize()
        $('#viewsLoader').show()
        $('.modal').hide()
        if (validateData().ok) {
            $.ajax({
                type: 'post',
                url: siteurl + 'Project/UpdateProject',
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                dataType: "json",
                data: formData,
                success: function (response) {
                    $('#viewsLoader').hide()
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
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/UpdateStatusPost',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            dataType: "json",
            data: formData,
            success: function (response) {
                $('#viewsLoader').hide()
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
        $('#viewsLoader').show()
        $('.modal').hide()
        $.ajax({
            type: 'post',
            url: siteurl + 'Project/DeleteProjectPost/',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: idProjectDelete,
            success: function (response) {
                $('#viewsLoader').hide()
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
    if ($('#Nombre').val() == '' || $('#Descripcion').val() == '') {
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


//Getting the authentication methods ID selected
function getMethodsList() {
    var listMetods = ""
    for (let i = 1; i <= methodsListLength; i++) {
        if ($(`#${i}`).is(':checked')) {
            listMetods += $(`#${i}`).val() + ','
        }
    }
    if (listMetods.charAt(listMetods.length - 1) == ',') {
        listMetods = listMetods.slice(0, -1)
    }
    $('#auth-method-list').val(listMetods)
}
