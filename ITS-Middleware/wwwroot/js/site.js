﻿const btnIconMenu = document.getElementById('btn-icon-menu')
const btnActiveMenu = document.getElementById('btn-active-menu')


function actionSlideMenu() {
    if (btnIconMenu.innerHTML == 'menu') {
        document.getElementById("sidenav").style.width = "250px";
        btnIconMenu.innerHTML = 'close'
        btnActiveMenu.setAttribute('title', 'Cerrar Menu')
    } else {
        document.getElementById("sidenav").style.width = "0";
        btnIconMenu.innerHTML = 'menu'
        btnActiveMenu.setAttribute('title', 'Abrir Menu')
    }
}

$('#btn-action-pass').on('click', () => {
    if ($('#btn-action-pass').text() == 'visibility_off') {
        $('#Pass').attr('type', 'text')
        $('#btn-action-pass').html('visibility')
    } else {
        $('#Pass').attr('type', 'password')
        $('#btn-action-pass').html('visibility_off')
    }
})



//Functions get tables content and views
function GetUsersList() {
    $("#sidenav").css('width',"0")
    btnIconMenu.innerHTML = 'menu'
    btnActiveMenu.setAttribute('title', 'Abrir Menu')

    $('#contentView').fadeOut(1)
    $('#viewsLoader').show()
    $.ajax({
        type: 'GET',
        url: siteurl + 'Home/Users',
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error';
                return;
            }
            $("#contentView").empty().append(resp).hide();
            $('#viewsLoader').hide();
            $("#contentView").fadeIn();
        },
        error: function (resp) {
            console.log(resp);
        }
    });
}

function GetProjectsList() {
    $("#sidenav").css('width', "0")
    btnIconMenu.innerHTML = 'menu'
    btnActiveMenu.setAttribute('title', 'Abrir Menu')

    $('#contentView').fadeOut(1)
    $('#viewsLoader').show()
    $.ajax({
        type: 'GET',
        url: siteurl + 'Home/Projects',
        success: function (resp) {
            if (resp == "Error") {
                window.location.href = '/Home/Error';
                return;
            }
            $("#contentView").empty().append(resp).hide();
            $('#viewsLoader').hide();
            $("#contentView").fadeIn();
        },
        error: function (resp) {
            console.log(resp);
        }
    })
}


function closeModal() {
    $('.btn-close-modal-view').click()
}


function ShowToastMessage(type, title_short_text, body_text) {
    if (type == 'success') {
        $('#toast-title-icon').text('done');
        $('#toast-title-text').text(' Listo');
        $('#toast-body-text').addClass('text-light');
        $('#liveToast').removeClass("bg-warning");
        $('#liveToast').removeClass("bg-danger");
        $('#liveToast').removeClass("bg-info");
        $('#liveToast').addClass("bg-success");
    } else if (type == 'warning') {
        $('#toast-title-icon').text('warning');
        $('#toast-title-text').text(' Advertencia');
        $('#liveToast').removeClass("bg-success");
        $('#liveToast').removeClass("bg-danger");
        $('#liveToast').removeClass("bg-info");
        $('#liveToast').addClass("bg-warning");
    } else if (type == 'error') {
        $('#toast-title-icon').text('error');
        $('#toast-title-text').text(' Error');
        $('#toast-body-text').addClass('text-light');
        $('#liveToast').removeClass("bg-success");
        $('#liveToast').removeClass("bg-warning");
        $('#liveToast').removeClass("bg-info");
        $('#liveToast').addClass("bg-danger");
    } else if (type == 'information') {
        $('#toast-title-text').text(' Información');
        $('#toast-title-icon').text('info');
        $('#liveToast').removeClass("bg-success");
        $('#liveToast').removeClass("bg-warning");
        $('#liveToast').removeClass("bg-danger");
        $('#liveToast').addClass("bg-info");
    }
    $('#toast-title-short-text').text(title_short_text);
    $('#toast-body-text').text(body_text);
    $('#liveToast').toast('show');
}

