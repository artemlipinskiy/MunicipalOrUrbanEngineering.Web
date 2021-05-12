$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();
    await fillTable();
    await ShowServiceList();

});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

async function ShowServiceList() {
    var services = await GetServices();
    for (var i = 0; i < services.length; i++) {
        $('#create-service-types').append('<option value="' + services[i].id + '">' + services[i].name + '  Счетчик (' + services[i].isCounterReadings+ ')</option>');
    }
    await ShowUnitName();
}

async function GetServices()
{
    var result = await $.ajax({
        type: 'GET',
        url: '/ServiceType/List',
        data: {
        }
    });
    return result;
}

async function GetService() {
    var result = await $.ajax({
        type: 'GET',
        url: '/ServiceType/Get',
        data: {
            id: $('#create-service-types').val()
        }
    });
    return result;
}
async function ShowUnitName() {
    var service = await GetService();
    if (service == null) {
        return;
    }
    $('#create-unit-name').val(service.unitName);
}

$("#create-service-types")
    .change(async function () {
        $("select option:selected").each(function () {
        });
        await ShowUnitName();
    })
    .change();

function ShowCreate() {
    $('#CreateDialog').modal('show');
}

async function CreateItem() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/Tariff/Create',
        data: {
            FlatId: $('#flat-id').val(),
            ServiceTypeId: $('#create-service-types').val(),
            StartTariff: $('#create-start-tariff').val(),
            EndTariff: $('#create-end-tariff').val(),
            Value: $('#create-value').val(),
            DefaultData: $('#create-default-data').val()
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
        url: '/Tariff/List',
        data: {
            FlatId: $('#flat-id').val()
        }
    });
    EndLoading();
    return result;
    
    Cancel();
}


async function fillTable() {
    var tariffs = await GetList();
    $('#tarifftablebody').empty();
    if (tariffs == false) {
        return;
    }
    for (var i = 0; i < tariffs.length; i++) {
        var end = '';
        if (tariffs[i].endTariff != null) {
            end = tariffs[i].endTariff.substring(0, 10);
        }
        var row = '<tr>'
            + ' <td>' + tariffs[i].serviceType + ' </td> <td>'
            + tariffs[i].value + ' </td><td>'
            + tariffs[i].defaultData + ' </td><td>'
            + tariffs[i].unitName + ' </td><td>'
            + tariffs[i].startTariff.substring(0, 10) + ' </td><td>'
            + end + '</td>' +
            '</tr>';
        $('#tarifftablebody').append(row);
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
        url: '/Tariff/Delete',
        data: {
            id: $('#delete-id').val()
        }
    });

    await fillTable();
    EndLoading();
}

function End(id) {

    $('#EndDialog').modal('show');
    $('#delete-id').val(id);
}
async function EndConfirm() {
    StartLoading();
    Cancel();
    var result = await $.ajax({
        type: 'POST',
        url: '/Tariff/End',
        data: {
            id: $('#delete-id').val()
        }
    });

    await fillTable();
    EndLoading();
}


function Cancel() {
    $('#CreateDialog').modal('hide');
    $('#EndDialog').modal('hide');
    $('#DeleteDialog').modal('hide');
}