$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();

    StartLoading();
    await FillTypeServices();
    await fillTable();
    EndLoading();
});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

async function ShowCreateRequest() {
    $('#CreateRequestDialog').modal('show');
}

async function FillTypeServices() {
    var types = await GetTypes();
    for (var i = 0; i < types.length; i++) {
        $('#create-request-type').append('<option value="' + types[i].id + '">' + types[i].name + '</option>');
    }
}

async function fillTable() {
    var requests = await GetRequestList();
    $('#requesttablebody').empty();
    if (requests == false) {
        return;
    }
    for (var i = 0; i < requests.length; i++) {
        var btns = '';
        if (requests[i].enableCancel) {
            btns += '<button type="button" class="btn btn-danger btn-sm" id="' +
                requests[i].id +
                '" onclick="CancelRequest(id)">Отменить</button>';
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
            + btns + '</td>' +
            '</tr>';
        $('#requesttablebody').append(row);
    }
}

async function GetTypes() {
    var result = await $.ajax({
        type: 'GET',
        url: '/request/TypeRequestList',
        data: {

        }
    });
    return result;
};

async function GetRequestList() {
    var result = await $.ajax({
        type: 'GET',
        url: '/request/requestlist',
        data: {

        }
    });
    return result;
}

async function CreateRequestItem() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/request/Create',
        data: {
            Name: $('#create-request-name').val(),
            Description: $('#create-request-description').val(),
            RequestTypeId: $('#create-request-type').val()
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

function CancelRequest(id) {
    $('#cancel-id').val(id);
    $('#CancelRequestDialog').modal('show');
}

async function CancelRequestItem() {
    var result = await $.ajax({
        type: 'POST',
        url: '/request/Cancel',
        data: {
            requestId: $('#cancel-id').val(),
        }
    });

    location.reload();
}

function Cancel() {
    $('#CreateRequestDialog').modal('hide');
    $('#GetResponseDialog').modal('hide');
    $('#CancelRequestDialog').modal('hide');
}