var tokenService
var isEmailValid = false
var validEmail = new RegExp("^[_A-Za-z0-9-]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9-]+)*(\\.[A-Za-z]{2,})$")
var projectImage
var projectId = $('#projectId').val()

var ui = new firebaseui.auth.AuthUI(firebase.auth())
var auth = firebase.auth()
auth.language = 'es'

var CLIENT_ID = '591728715836-keb38fgj9oe8cbvim4to5qlqhn929j35.apps.googleusercontent.com'
var googleProvider = new firebase.auth.GoogleAuthProvider();
var facebookProvider = new firebase.auth.FacebookAuthProvider();
var twitterProvider = new firebase.auth.TwitterAuthProvider();
var githubProvider = new firebase.auth.GithubAuthProvider();
var yahooProvider = new firebase.auth.OAuthProvider('yahoo.com');
var microsoftProvider = new firebase.auth.OAuthProvider('microsoft.com');

$(document).ready(() => {
    projectImage = $('#projectImageUrl').val()
    if (projectImage != "") {
        $('body').css('background', 'no-repeat center center fixed url(https://director.cl/wp-content/themes/Director_Theme/img/PageLoader/loading.gif)')
        $('body').css('background-size', 'cover')
        $('<img/>').attr('src', projectImage).on('load', () => {
            $(this).remove()
            $('body').css('background', `no-repeat center center fixed url(${projectImage})`)
            $('body').css('background-size', 'cover')
        })
    } else {
        $('body').css('background', '#fff')
    }
})

$('#email, #pass').on('change keyup paste', () => {
    if (isEmailValid && $('#pass').val().length > 2) {
        $('#sign-in-email-pass').prop('disabled', false)
    } else {
        $('#sign-in-email-pass').prop('disabled', true)
    }
    if ($('#email').val() == "" || $('#email').val().length < 3) {
        $('#messageEmail').html('Ingresa un correo electrónico para continuar.')
        isEmailValid = false
    } else if (validEmail.test($('#email').val())) {
        $('#messageEmail').html('')
        isEmailValid = true

    } else {
        $('#messageEmail').html('Ingresa un correo electrónico válido (ej: username@domain.ext).')
        isEmailValid = false
    }
})

$('#Facebook').on('click', async () => {
    signInWithPopup(facebookProvider)
})

$('#Twitter').on('click', async () => {
    signInWithPopup(twitterProvider)
})

$('#Google').on('click', async () => {
    signInWithPopup(googleProvider)
})

$('#Yahoo').on('click', async () => {
    signInWithPopup(yahooProvider)
})

$('#Github').on('click', async () => {
    signInWithPopup(githubProvider)
})

$('#Microsoft').on('click', async () => {
    signInWithPopup(microsoftProvider)
})

$('#Numero-de-telefono').on('click', async () => {
    ui.reset()
    ui.start('#firebaseui-auth-container', getUiConfig())
})



const signInWithPopup = async (provider) => {
    var currentUser = auth.currentUser
    if (currentUser != null) {
        deleteAccount()
    }
    await auth.signInWithPopup(provider).then(result => {
        var token = result.credential.accessToken
        var user = result.user
        if (user != null) {
            var data = `email=${user.email}&projectId=${projectId}&userUid=${user.uid}`
            tokenService = token
            signIn(data)
        } else {
            ShowToastMessage('error', 'No se pudo acceder', 'No se pudo obtener la información para el inicio de sesión, intenta de nuevo')
        }
    }).catch(error => {
        if (error.code == "auth/internal-error") {
            auth.signInWithPopup(provider).then((result) => {
                var token = result.credential.accessToken
                var user = result.user
                var data = `email=${user.email}&projectId=${projectId}&userUid=${user.uid}`
                tokenService = token
                signIn(data)
            }).catch((error) => {
                alert(error)
            });
        }
    })
}

function getUiConfig() {
    return {
        callbacks: {
            signInSuccess: function (response) {
                console.log(response.phoneNumber)
                if (response != null && response.phoneNumber != null) {
                    var data = `phoneNumber=${response.phoneNumber.slice(-10)}&projectId=${projectId}&userUid=${response.uid}`
                    tokenService = response.refreshToken
                    signIn(data)
                    $('.btn-close').click()
                } else {
                    ShowToastMessage('error', 'No se pudo acceder', 'No se pudo obtener la información para el inicio de sesión, intenta de nuevo')
                }
            }
        },
        signInOptions: [
            {
                provider: firebase.auth.PhoneAuthProvider.PROVIDER_ID,
                recaptchaParameters: {
                    type: 'image',
                    size: 'normal',
                }
            }
        ],
    }
}


function signOutFb() {
    auth.signOut().then(() => {
        console.log("SignOut...")
    })
}

function deleteAccount() {
    firebase.auth().currentUser.delete().catch(function (error) {
        if (error.code == 'auth/requires-recent-login') {
            firebase.auth().signOut().then(function () {
                console.log("SignOut...")
            });
        }
    });
};


function signIn(data) {
    $('.loader-form').show()
    $.ajax({
        type: 'POST',
        url: siteurl + 'Home/SignIn',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        data: data,
        success: function (response) {
            $('.loader-form').hide()
            if (response.status == 500) {
                window.location.href = '/Home/Error';
                return;
            } else if (response.status == 400) {
                ShowToastMessage('error', response.msgHeader, response.msg)
            } else {
                ShowToastMessage('success', "Ingresando...", "En un momento serás redireccionado a la pantalla principal")
                window.location.href = `${redirectToUrl}/signIn?token=${tokenService}`
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
}

$('#sign-in-email-pass').on('click', function () {
    var data = `email=${$('#email').val()}&pass=${$('#pass').val()}&projectId=${projectId}`
    signIn(data)
})


$('#emailResPass').on('change keyup paste input', () => {
    $('#sendEmailRestorePass').prop('disabled', true)
    if ($('#emailResPass').val() == "") {
        $('#messageInvalidEmail').html('<i class="bi bi-exclamation-circle-fill"></i><small> Ingresa un correo electrónico para continuar</small>')
        $('#messageInvalidEmail').show()
    } else if (!validEmail.test($('#emailResPass').val())) {
        $('#messageInvalidEmail').html('<i class="bi bi-exclamation-circle-fill"></i><small> Debes ingresar un correo electrónico valido (ej: user@mail.com)</small>')
        $('#messageInvalidEmail').show()
    } else {
        $('#sendEmailRestorePass').prop('disabled', false)
        $('#messageInvalidEmail').hide()
    }
})


function ShowToastMessage(type, title_short_text, body_text) {
    if (type == 'warning') {
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
    } else if (type == 'success') {
        $('#toast-title-icon').text('done');
        $('#toast-title-text').text(' Listo');
        $('#toast-body-text').addClass('text-light');
        $('#liveToast').removeClass("bg-warning");
        $('#liveToast').removeClass("bg-danger");
        $('#liveToast').removeClass("bg-info");
        $('#liveToast').addClass("bg-success");
    }
    $('#toast-title-short-text').text(title_short_text);
    $('#toast-body-text').text(body_text);
    $('#liveToast').toast('show');
}
