// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function Logout() {
    $("#erorr-message").hide();
    var result = await $.ajax({
        type: 'POST',
        url: '/account/Logout',
        data: null
    });
    if (result) {
        window.location.replace('/home/index');
    } else {
        $("#erorr-message").text("Logout failed");
        $("#erorr-message").show();
    }
}
async function Login() {
    window.location.replace('/account/Login');
}
