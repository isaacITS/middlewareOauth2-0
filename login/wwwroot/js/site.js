var tokenService

var firebaseConfig = {
    apiKey: "AIzaSyCW_1wI9EevsL80zDvedYdACdepFy6S9zY",
    authDomain: "middlewareoauth20.firebaseapp.com",
    projectId: "middlewareoauth20",
    storageBucket: "middlewareoauth20.appspot.com",
    messagingSenderId: "591728715836",
    appId: "1:591728715836:web:8b3f6902fbedffec813b39"
}
firebase.initializeApp(firebaseConfig)


$('#Facebook').on('click', () => {
    var provider = new firebase.auth.FacebookAuthProvider()
    firebase.auth().signInWithPopup(provider).then(function (result) {
        var token = result.credential.accessToken
        var user = result.user
        if (user != null) {
            var data = { email: user.email, pass: "" }
            tokenService = token
            signIn(data)
        } else {
            ShowToastMessage('error', 'No se pudo acceder', 'No se pudo obtener la información para el inicio de sesión, intenta de nuevo')
        }
    }).catch(function (error) {
        var errorCode = error.code;
        var errorMessage = error.message;
        var email = error.email;
        var credential = error.credential;
        console.log(errorMessage);
    })
})

$('#Twitter').on('click', () => {
    var provider = new firebase.auth.TwitterAuthProvider()
    firebase.auth().signInWithPopup(provider).then(function (result) {
        var token = result.credential.accessToken;
        var user = result.user;
        if (user != null) {
            var data = { email: user.email, pass: "" }
            tokenService = token
            signIn(data)
        } else {
            ShowToastMessage('error', 'No se pudo acceder', 'No se pudo obtener la información para el inicio de sesión, intenta de nuevo')
        }
    }).catch(function (error) {
        var errorCode = error.code;
        var errorMessage = error.message;
        var email = error.email;
        var credential = error.credential;
        console.log(errorMessage);
    });
})



$('#Google').on('click', () => {
    var provider = new firebase.auth.GoogleAuthProvider();
    firebase.auth().signInWithPopup(provider).then(function (result) {
        var token = result.credential.accessToken
        var user = result.user
        if (user != null) {
            var data = { email: user.email, pass: "" }
            tokenService = token
            signIn(data)
        } else {
            ShowToastMessage('error', 'No se pudo acceder', 'No se pudo obtener la información para el inicio de sesión, intenta de nuevo')
        }
        //console.log("TOKEN => ", token)
        //updateUser(user)
    }).catch(function (error) {
        var errorCode = error.code
        var errorMessage = error.message;
        console.log("ERROR: "+errorCode, errorMessage);
    });
})

function signIn(data) {
    $.ajax({
        type: 'POST',
        url: siteurl + 'Home/SignIn',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        dataType: "json",
        data: data,
        success: function (response) {
            if (response.status == 500) {
                window.location.href = '/Home/Error';
                return;
            } else if (response.status == 400) {
                ShowToastMessage('error', response.msgHeader, response.msg)
            } else {
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
    var data = $('#signInForm').serialize()
    signIn(data)
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
    }
    $('#toast-title-short-text').text(title_short_text);
    $('#toast-body-text').text(body_text);
    $('#liveToast').toast('show');
}