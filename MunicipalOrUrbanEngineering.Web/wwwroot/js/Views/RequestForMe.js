$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();

    StartLoading();
    await fillTable();
    EndLoading();
});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

async function fillTable() {
    var requests = await GetRequestList();
    $('#requesttablebody').empty();
    if (requests == false) {
        return;
    }
    for (var i = 0; i < requests.length; i++) {
        var btns = '';
        if (requests[i].enableResponse) {
            btns += '<button type="button" class="btn btn-success btn-sm" id="' +
                requests[i].id +
                '" onclick="Response(id)">Ответить</button>';
        }
        if (requests[i].enableGetResponse) {
            btns += '<button type="button" class="btn btn-info btn-sm" id="' +
                requests[i].id +
                '" onclick="ShowResponse(id)">Прочитать ответ</button>';
        }
        var row = '<tr>'
            + ' <td>' + requests[i].name + ' </td> <td>'
            + requests[i].description + ' </td><td>'
            + requests[i].requestType + ' </td><td>'
            + requests[i].status + ' </td><td>'
            + requests[i].creationDate + ' </td><td>'
            + requests[i].requestor + ' </td><td>'
            + btns + '</td>' +
            '</tr>';
        $('#requesttablebody').append(row);
    }
}

async function GetRequestList() {
    var result = await $.ajax({
        type: 'GET',
        url: '/request/requestlist',
        data: {

        }
    });
    return result;
}

function Response(id) {
    $('#request-id').val(id);
    $('#CreateResponseDialog').modal('show');
}


async function CreateItem() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/request/CreateResponse',
        data: {
            Name: $('#create-response-name').val(),
            Description: $('#create-response-description').val(),
            ServiceRequestId: $('#request-id').val()
        }
    });
    EndLoading();
    Cancel();
    location.reload();
}


async function GetResponse(requestid) {
    var result = await $.ajax({
        type: 'GET',
        url: '/request/GetResponse',
        data: {
            requestId: requestid
        }
    });
    return result;
};

async function ShowResponse(id) {
    StartLoading();
    var response = await GetResponse(id);
    $('#get-response-name').val(response.name);
    $('#get-response-description').val(response.description);
    $('#GetResponseDialog').modal('show');
    EndLoading();
}


function Cancel() {
    $('#CreateResponseDialog').modal('hide');

    $('#GetResponseDialog').modal('hide');

}