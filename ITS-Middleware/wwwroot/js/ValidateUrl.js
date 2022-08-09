var baseUrl = window.location.origin;
var getUrl = window.location.origin + window.location.pathname;

window.onload = () => {
    if (getUrl == `${baseUrl}/Home` || getUrl == `${baseUrl}/Home/Projects` || getUrl == `${baseUrl}/Home/Home/Projects`) {
        window.location.href = `${baseUrl}/Home/Home`
    } else if (getUrl == `${baseUrl}/Home/Users` || getUrl == `${baseUrl}/Home/Home/Users`) {
        window.location.href = `${baseUrl}/Home/Home`
    } else if (getUrl == `${baseUrl}/Home/UsersByProject` || getUrl == `${baseUrl}/Home/Home/UsersByProject`) {
        window.location.href = `${baseUrl}/Home/Home`
    }
}