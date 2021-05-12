$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();
    await ShowPeriodList();
    await fillTable();
    await fillTableMetersData();

});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

async function ShowPeriodList() {
    StartLoading();
    var periods = await GetPeriods();
    for (var i = 0; i < periods.length; i++) {
        $('#period-selector').append('<option value="' + periods[i].id + '">' + periods[i].name + '</option>');
    }
   
    var period = await GetCurrentPeriod();
    $('#period-selector option[value=' + period.id + ']').prop('selected', true);
    EndLoading();
}

async function GetPeriods() {
    var result = await $.ajax({
        type: 'GET',
        url: '/paymentperiod/List',
        data: {
        }
    });
    return result;
}

$('#period-selector')
    .change(async function () {
        $("select option:selected").each(function () {

        });
        await fillTable();
        await fillTableMetersData();
    })
    .change();

async function fillTable() {
    var flatid = $('#flat-id').val();
    var tariffs = await GetTariffs(flatid);
    $('#tarifftablebody').empty();
    if (tariffs == null) {
        return;
    }
   
    for (var i = 0; i < tariffs.length; i++) {
        var btns = '';
        if (true) {
            btns += '<button type="button" class="btn btn-success btn-sm" id="' +
                tariffs[i].id +
                '" onclick="PostMetersData(id)"> Подать показания </button>';
        }
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
            + end + '</td><td>'
            + btns + '</td>' +
            '</tr>';
        $('#tarifftablebody').append(row);
    }
}

function PostMetersData(tariffid)
{
    $('#CreateDialog').modal('show');
    $('#tariff-id').val(tariffid);

}

async function CreateItem() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/MetersData/Create',
        data: {
            tariffId: $('#tariff-id').val(),
            value: $('#create-value').val(),
            PaymentPeriodId : $('#period-selector').val()
        }
    });
    EndLoading();
    Cancel();
    location.reload();
}

async function GetTariffs(flatid) {
    StartLoading();
    var result = await $.ajax({
        type: 'GET',
        url: '/MetersData/GetTariffsForTakingMeterReadings',
        data: {
            flatId: $('#flat-id').val(),
            periodId: $('#period-selector').val()
        }
    });
    EndLoading();
    return result;
}


async function fillTableMetersData() {
    var flatid = $('#flat-id').val();
    var metersdatas = await GetMetersDatas(flatid);
    $('#metersdatatablebody').empty();
    if (metersdatas == null) {
        return;
    }

    for (var i = 0; i < metersdatas.length; i++) {
        var row = '<tr>'
            + ' <td>' + metersdatas[i].paymentPeriod + ' </td> <td>'
            + metersdatas[i].serviceType + ' </td><td>'
            + metersdatas[i].value + ' </td></td>' +
            '</tr>';
        $('#metersdatatablebody').append(row);
    }
}

async function GetMetersDatas(flatid) {
    StartLoading();
    var result = await $.ajax({
        type: 'GET',
        url: '/MetersData/GetMetersData',
        data: {
            flatId: $('#flat-id').val(),
            periodId: $('#period-selector').val()
        }
    });
    EndLoading();
    return result;
}


async function GetCurrentPeriod() {
    StartLoading();
    var result = await $.ajax({
        type: 'GET',
        url: '/PaymentPeriod/GetPrev',
        data: {
            
        }
    });
    EndLoading();
    return result;
}


function Cancel() {
    $('#CreateDialog').modal('hide');
    //$('#EndDialog').modal('hide');
    //$('#DeleteDialog').modal('hide');
}