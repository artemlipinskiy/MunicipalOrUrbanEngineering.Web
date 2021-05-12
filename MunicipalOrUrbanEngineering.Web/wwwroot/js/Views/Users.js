$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();
    await fillTable();

});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

function ShowCreate() {
    $('#CreateDialog').modal('show');
}

async function GetList() {
    StartLoading();
    var result = await $.ajax({
        type: 'GET',
        url: '/Account/List',
        data: {
            page: $('#page').val(),
            pageSize: $('#pageSize').val()
        }
    });
    EndLoading();
    return result;
    
}


async function fillTable() {
    var users = await GetList();
    $('#userstablebody').empty();
    if (users == null) {
        return;
    }
    for (var i = 0; i < users.pageItems.length; i++) {
        var row = '<tr>'
            + ' <td>' + users.pageItems[i].id + ' </td> <td>'
            + users.pageItems[i].login + ' </td><td>'
            + users.pageItems[i].role.name + ' </td><td>'
            + users.pageItems[i].fullName + '</td>' +
            '</tr>';
        $('#userstablebody').append(row);
    }
    if ($('#page').val() <= 1) {
        $('#prev-btn').hide();
    } else {
        $('#prev-btn').show();
    }
    if ($('#page') >= users.maxPage) {
        $('#next-btn').hide();
    } else {
        $('#next-btn').show();   
    }
    $('#labelpage').text(users.currentPage);

    $('#labelmaxpage').text(users.maxPage);
}

async function next() {
    var page = $('#page').val();
    page++;
    $('#page').val(page);
    await fillTable();
}
async function prev() {
    var page = $('#page').val();
    page--;
    $('#page').val(page);
    await fillTable();
}

function Cancel() {
    $('#CreateDialog').modal('hide');
    $('#DeleteDialog').modal('hide');
}