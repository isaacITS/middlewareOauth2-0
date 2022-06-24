$('#btn-action-pass').on('click', () => {
    if ($('#btn-action-pass').text() == 'visibility_off') {
        $('#Pass').attr('type', 'text')
        $('#btn-action-pass').html('visibility')
    } else {
        $('#Pass').attr('type', 'password')
        $('#btn-action-pass').html('visibility_off')
    }
})

function getPassword() {
    document.getElementById('Pass').value = autoCreate(12);
}
function autoCreate(plength) {
    var chars = "abcdefghijklmnopqrstubwsyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890#$%&()=?¡!@*+{}-_";
    var password = '';
    for (i = 0; i < plength; i++) {
        password += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return password;
}