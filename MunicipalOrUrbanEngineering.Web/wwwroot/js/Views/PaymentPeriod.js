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
        url: '/paymentperiod/Create',
        data: {
            Month: $('#create-month').val()
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
        url: '/paymentperiod/List',
        data: {
        }
    });
    EndLoading();
    return result;
    
    Cancel();
}


async function fillTable() {
    var periods = await GetList();
    $('#periodtablebody').empty();
    if (periods == false) {
        return;
    }
    for (var i = 0; i < periods.length; i++) {
        var btns = '';
       
        if (periods[i].enableCollectingReadings == true) {
            btns += '<button type="button" class="btn btn-danger btn-sm" id="' +
                periods[i].id +
                '" onclick="Collecting(id)">Начать сбор показаний счетчиков</button>';
        }
        if (periods[i].enableFormationOfReceipts == true) {
            btns += '<button type="button" class="btn btn-danger btn-sm" id="' +
                periods[i].id +
                '" onclick="Formation(id)">Начать формирование квитанций</button>';
        }
        if (periods[i].enablePaymentOfReceipts == true) {
            btns += '<button type="button" class="btn btn-danger btn-sm" id="' +
                periods[i].id +
                '" onclick="Payment(id)">Начать прием оплаты</button>';
        }
        if (periods[i].enableClose == true) {
            btns += '<button type="button" class="btn btn-danger btn-sm" id="' +
                periods[i].id +
                '" onclick="Close(id)">Завершить</button>';
        }
        if (true) {
            btns += '    <br /><br /><button type="button" class="btn btn-info btn-sm" id="' +
                periods[i].id +
                '" onclick="SheetList(id)">Список счетов</button>';
        }
        var row = '<tr>'
            + ' <td>' + periods[i].name + ' </td> <td>'
            + periods[i].status + ' </td><td>'
            + btns+ '</td>' +
            '</tr>';
        $('#periodtablebody').append(row);
    }
}

async function Collecting(id) {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/paymentperiod/SetStatus',
        data: {
            periodId: id,
            statusId:'DB8EC517-8841-47AC-A9E6-421DF83F5C71'
        }
    });
    EndLoading();
    location.reload();
    return result;
}
async function Formation(id) {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/paymentperiod/SetStatus',
        data: {
            periodId: id,
            statusId: '2176460C-A6A1-4536-9809-2E417EBE5A54'
        }
    });
    EndLoading();
    location.reload();
    return result;
}
async function Payment(id) {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/paymentperiod/SetStatus',
        data: {
            periodId: id,
            statusId: 'D93E31A6-DE36-4AEB-9AE4-19305D635657'
        }
    });
    EndLoading();
    location.reload();
    return result;
}
async function Close(id) {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/paymentperiod/SetStatus',
        data: {
            periodId: id,
            statusId: '1A0C8FDB-2E2A-46AF-8F64-01B67258291A'
        }
    });
    EndLoading();
    location.reload();
    return result;
}

function SheetList(id) {
    location.href = '/PaymentSheet/Index?PeriodId=' + id;
}


function Cancel() {
    $('#CreateDialog').modal('hide');
    
}