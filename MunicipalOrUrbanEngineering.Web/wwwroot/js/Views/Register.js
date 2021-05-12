$(document).ready(function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();
});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}
async function CreateAccount() {
    StartLoading();
    $('#erorr-message').hide();
    $('#success-message').hide();
    if (await CheckPassword() == false) {
        $('#erorr-message').text("Password mismatch");
        $('#erorr-message').show();
        EndLoading();
        return;
    }
    if (!(await CheckLogin())) {
        $('#erorr-message').text("User with this login already exists.");
        $('#erorr-message').show();
        EndLoading();
        return;
    }
    var result;
    if ($('#registration-roleid').val() == 'A8013028-E382-4672-AD44-43DF699DFD1E') {
        result = await $.ajax({
            type: 'POST',
            url: '/Account/RegisterEmployee',
            data: {
                Login: $('#registration-login').val(),
                Password: $('#registration-password').val(),
                RepeatPassword: $('#registration-repeat-password').val(),
                FirstName: $('#registration-firstname').val(),
                LastName: $('#registration-lastname').val(),
                MiddleName: $('#registration-middlename').val(),
                RoleId: $('#registration-roleid').val()
            }
        });
    }
    if ($('#registration-roleid').val() == '4E3C33E5-7FFE-4515-9867-90E11A20DF04') {
        result = await $.ajax({
            type: 'POST',
            url: '/Account/Register',
            data: {
                Login: $('#registration-login').val(),
                Password: $('#registration-password').val(),
                RepeatPassword: $('#registration-repeat-password').val(),
                FirstName: $('#registration-firstname').val(),
                LastName: $('#registration-lastname').val(),
                MiddleName: $('#registration-middlename').val(),
                RoleId: $('#registration-roleid').val()
            }
        });
    }
    if ($('#registration-roleid').val() == '9B7DAB16-E2B8-47AD-B639-D6D76AC97AFC') {
        result = await $.ajax({
            type: 'POST',
            url: '/Account/RegisterOwner',
            data: {
                Login: $('#registration-login').val(),
                Password: $('#registration-password').val(),
                RepeatPassword: $('#registration-repeat-password').val(),
                FirstName: $('#registration-firstname').val(),
                LastName: $('#registration-lastname').val(),
                MiddleName: $('#registration-middlename').val(),
                RoleId: $('#registration-roleid').val()
            }
        });
    }
    if (result) {
        $('#success-message').show();
        $('#registration-login').val('');
        $('#registration-password').val('');
        $('#registration-repeat-password').val('');
        $('#registration-firstname').val('');
        $('#registration-lastname').val('');
        $('#registration-middlename').val('');
    }
    EndLoading();
}

async function CheckPassword() {
    if ($('#registration-password').val() == $('#registration-repeat-password').val()) {
        return true;
    } else return false;
}

async function CheckLogin() {
    var result = await $.ajax({
        type: 'GET',
        url: '/account/LoginExist',
        data: {
            login: $('#registration-login').val()
        }
    });
    return !result;
}

async function Login() {
    $("#erorr-message").hide();
    var result = await $.ajax({
        type: 'POST',
        url: '/account/Login',
        data: {
            Login: $('#login-login').val(),
            Password: $('#login-password').val()
        }
    });
    if (result) {
        window.location.replace('/home/index');
    } else {
        $("#erorr-message").text("Login failed");
        $("#erorr-message").show();
    }
}