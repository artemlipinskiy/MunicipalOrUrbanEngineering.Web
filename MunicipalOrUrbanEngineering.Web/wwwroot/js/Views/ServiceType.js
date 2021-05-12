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

async function CreateItem() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/ServiceType/Create',
        data: {
            Name: $('#create-name').val(),
            Description: $('#create-name').val(),
            UnitName: $('#create-unit-name').val(),
            IsCounterReadings: $("#create-is-counter").is(":checked")
        }
    });
    EndLoading();
    Cancel();
    location.reload();
}

async function GetList() {
    StartLoading();
    var result = await $.ajax({
        type: 'GET',
        url: '/ServiceType/List',
        data: {
        }
    });
    EndLoading();
    return result;
    
    Cancel();
}


async function fillTable() {
    var servicetypes = await GetList();
    $('#servicetypetablebody').empty();
    if (servicetypes == false) {
        return;
    }
    for (var i = 0; i < servicetypes.length; i++) {
        var btns = '<button type="button" class="btn btn-danger btn-sm" id="' +
            servicetypes[i].id +
            '" onclick="Delete(id)">Удалить</button>';
        var row = '<tr>'
            + ' <td>' + servicetypes[i].name + ' </td> <td>'
            + servicetypes[i].description + ' </td><td>'
            + servicetypes[i].unitName + ' </td><td>'
            + servicetypes[i].isCounterReadings + ' </td><td>'
            + btns+ '</td>' +
            '</tr>';
        $('#servicetypetablebody').append(row);
    }
}

function Delete(id) {

    $('#DeleteDialog').modal('show');
    $('#delete-id').val(id);
}
async function DeleteConfirm() {
    StartLoading();
    Cancel();
    var result = await $.ajax({
        type: 'POST',
        url: '/ServiceType/Delete',
        data: {
            id: $('#delete-id').val()
        }
    });

    await fillTable();
    EndLoading();
}

function Cancel() {
    $('#CreateDialog').modal('hide');
    $('#DeleteDialog').modal('hide');
}