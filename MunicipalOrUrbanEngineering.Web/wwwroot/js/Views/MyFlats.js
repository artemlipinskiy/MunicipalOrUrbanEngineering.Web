$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();
    await ShowTableFlatItems();

});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

async function ShowTableFlatItems() {
    $('#flattablebody').empty();
    var flats = await GetFlats();
    if (flats == false) {
        return;
    }
    for (var i = 0; i < flats.length; i++) {
        var row = '<tr>'
            + ' <td>' + flats[i].fullAddress + ' </td> <td>'
            + flats[i].ownerFullname + ' </td><td>'
            + '<button type="button" class="btn btn-info btn-sm" id="' + flats[i].id + '" onclick="FlatTariffs(id)"> Тарифы </button>  '
            + '<button type="button" class="btn btn-info btn-sm" id="' + flats[i].id + '" onclick="FlatSheets(id)"> Счета </button>  '
            + '<button type="button" class="btn btn-info btn-sm" id="' + flats[i].id + '" onclick="FlatCounter(id)"> Счетчики </button> </td>' +
            '</tr>';
        $('#flattablebody').append(row);
    }
}

function FlatTariffs(id){
    location.href ='/Tariff/MyTariffs?FlatId='+id;
}

function FlatCounter(id) {
    location.href ='/MetersData/MyMetersData?flatId=' + id;
}
function FlatSheets(id) {
    location.href ='/paymentsheet/MySheets?flatId='+id;
}

async function GetFlats() {
    var result = await $.ajax({
        type: 'GET',
        url: '/building/GetMyFlats',
        data: {
        }
    });
    return result;
}


function Cancel() {
    //$('#CreateStreetDialog').modal('hide');
}