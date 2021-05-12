$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();

    StartLoading();
    //await FillTypeServices();
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
    var sheets = await GetList();
    $('#sheettablebody').empty();
    if (sheets == null) {
        return;
    }
    for (var i = 0; i < sheets.length; i++) {
        var btns = '';
        if (sheets[i].enableDetails) {
            btns += '<button type="button" class="btn btn-info btn-sm" id="' +
                sheets[i].id +
                '" onclick="Details(id)">Детали</button>';
        }
        var row = '<tr>'
            + ' <td>' + sheets[i].paymentPeriod + ' </td> <td>'
            + sheets[i].flat + ' </td><td>'
            + sheets[i].name + ' </td><td>'
            + sheets[i].comment + ' </td><td>'
            + sheets[i].status + ' </td><td>'
            + sheets[i].amount + ' </td><td>'
            + btns + '</td>' +
            '</tr>';
        $('#sheettablebody').append(row);
    }
}

async function GetList() {
    var result = await $.ajax({
        type: 'GET',
        url: '/paymentsheet/List',
        data: {
            flatId : $('#flat-id').val()
        }
    });
    return result;
};

async function Details(id) {
    StartLoading();
    var details = await GetDetails(id);
    $('#DetailsDialog').modal('show');
    $('#detailstablebody').empty();
    if (details == null) {
        return;
    }
    for (var i = 0; i < details.length; i++) {

        var row = '<tr>'
            + ' <td>' + details[i].paymentSheet + ' </td> <td>'
            + details[i].status + ' </td><td>'
            + details[i].comment + ' </td><td>'
            + details[i].serviceType + ' </td><td>'
            + details[i].hasCounter + ' </td><td>'
            + details[i].consume + ' </td><td>'
            + details[i].unitName + ' </td><td>'
            + details[i].valueUnit + ' </td><td>'
            + details[i].value + ' </td>' +
            '</tr>';
        $('#detailstablebody').append(row);
    }
    EndLoading();
}

async function GetDetails(id) {
    var result = await $.ajax({
        type: 'GET',
        url: '/paymentsheet/GetDetails',
        data: {
            sheetId: id
        }
    });
    return result;
}


function Cancel() {
    $('#DetailsDialog').modal('hide');
}