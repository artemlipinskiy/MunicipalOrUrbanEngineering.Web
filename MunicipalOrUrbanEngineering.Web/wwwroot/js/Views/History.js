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
        url: '/History/List',
        data: {
            page: $('#page').val(),
            pageSize: $('#pageSize').val()
        }
    });
    EndLoading();
    return result;
    
}


async function fillTable() {
    var histories = await GetList();
    $('#historytablebody').empty();
    if (histories == null) {
        return;
    }
    for (var i = 0; i < histories.pageItems.length; i++) {
        var row = '<tr>'
            + ' <td>' + histories.pageItems[i].entityId + ' </td> <td>'
            + histories.pageItems[i].creationDate + ' </td><td>'
            + histories.pageItems[i].employee + ' </td><td>'
            + histories.pageItems[i].owner + ' </td><td>'
            + histories.pageItems[i].entity + ' </td><td>'
            + histories.pageItems[i].action + ' </td><td>'
            + histories.pageItems[i].details  + '</td>' +
            '</tr>';
        $('#historytablebody').append(row);
    }
    if ($('#page').val() <= 1) {
        $('#prev-btn').hide();
    } else {
        $('#prev-btn').show();
    }
    if ($('#page')>=histories.maxPage) {
        $('#next-btn').hide();
    } else {
        $('#next-btn').show();   
    }
    $('#labelpage').text(histories.currentPage);

    $('#labelmaxpage').text(histories.maxPage);
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