$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();

    $('#create-bulletin-formrecipient').addClass('hide');
    $('#create-bulletin-recipient').addClass('hide');
    GetTypesRecipients();
    await ShowTableBulletinBoardItems();
});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

function GetTypesRecipients() {
    StartLoading();
    var TypeRecipient = { id: 1, name: 'Все пользователи' };
    $('#create-bulletin-typerecipient').append('<option value="' + TypeRecipient.id + '">' + TypeRecipient.name + '</option>');
    TypeRecipient = { id: 2, name: 'Улица' };
    $('#create-bulletin-typerecipient').append('<option value="' + TypeRecipient.id + '">' + TypeRecipient.name + '</option>');
    TypeRecipient = { id: 3, name: 'Дом' };
    $('#create-bulletin-typerecipient').append('<option value="' + TypeRecipient.id + '">' + TypeRecipient.name + '</option>');
    TypeRecipient = { id: 4, name: 'Квартира' };
    $('#create-bulletin-typerecipient').append('<option value="' + TypeRecipient.id + '">' + TypeRecipient.name + '</option>');
    TypeRecipient = { id: 5, name: 'Владелец' };
    $('#create-bulletin-typerecipient').append('<option value="' + TypeRecipient.id + '">' + TypeRecipient.name + '</option>');

    EndLoading();
    
}
async function ShowCreateBulletin() {
    $('#CreateBulletinDialog').modal('show');
}

async function GetStreets() {
    var result = await  $.ajax({
        type: 'GET',
        url: '/building/ListStreet',
        data: {

        }
    });
    return result;
}
async function GetBuildings() {
    var result = await  $.ajax({
        type: 'GET',
        url: '/building/ListAllBuilding',
        data: {

        }
    });
    return result;
}
async function GetFlats() {
    var result = await  $.ajax({
        type: 'GET',
        url: '/building/ListAllFlat',
        data: {

        }
    });
    return result;
}
async function GetOwners() {
    var result = await $.ajax({
        type: 'GET',
        url: '/owner/List',
        data: {

        }
    });
    return result;
}



async function CreateBulletinItem() {
    if ($('#create-bulletin-typerecipient').val() == 1) {
        StartLoading();
        var result = await $.ajax({
            type: 'POST',
            url: '/BulletinBoard/Create',
            data: {
                Title: $('#create-bulletin-title').val(),
                Description: $('#create-bulletin-description').val(),
                ShortDescription: $('#create-bulletin-shortdescription').val(),
                ToAllUsers: true,
                ShowStart: $('#create-bulletin-start').val(),
                ShowEnd: $('#create-bulletin-end').val()
            }
        });
        EndLoading();
    }
    if ($('#create-bulletin-typerecipient').val() == 2) {
        StartLoading();
        var result = await $.ajax({
            type: 'POST',
            url: '/BulletinBoard/Create',
            data: {
                Title: $('#create-bulletin-title').val(),
                Description: $('#create-bulletin-description').val(),
                ShortDescription: $('#create-bulletin-shortdescription').val(),
                ToAllUsers: false,
                ToStreetId: $('#create-bulletin-recipient').val(),
                ShowStart: $('#create-bulletin-start').val(),
                ShowEnd: $('#create-bulletin-end').val()
            }
        });
        EndLoading();
    }
    if ($('#create-bulletin-typerecipient').val() == 3) {
        StartLoading();
        var result = await $.ajax({
            type: 'POST',
            url: '/BulletinBoard/Create',
            data: {
                Title: $('#create-bulletin-title').val(),
                Description: $('#create-bulletin-description').val(),
                ShortDescription: $('#create-bulletin-shortdescription').val(),
                ToAllUsers: false,
                ToBuildingId: $('#create-bulletin-recipient').val(),
                ShowStart: $('#create-bulletin-start').val(),
                ShowEnd: $('#create-bulletin-end').val()
            }
        });
        EndLoading();
    }
    if ($('#create-bulletin-typerecipient').val() == 4) {
        StartLoading();
        var result = await $.ajax({
            type: 'POST',
            url: '/BulletinBoard/Create',
            data: {
                Title: $('#create-bulletin-title').val(),
                Description: $('#create-bulletin-description').val(),
                ShortDescription: $('#create-bulletin-shortdescription').val(),
                ToAllUsers: false,
                ToFlatId: $('#create-bulletin-recipient').val(),
                ShowStart: $('#create-bulletin-start').val(),
                ShowEnd: $('#create-bulletin-end').val()
            }
        });
        EndLoading();
    }
    if ($('#create-bulletin-typerecipient').val() == 5) {
        StartLoading();
        var result = await $.ajax({
            type: 'POST',
            url: '/BulletinBoard/Create',
            data: {
                Title: $('#create-bulletin-title').val(),
                Description: $('#create-bulletin-description').val(),
                ShortDescription: $('#create-bulletin-shortdescription').val(),
                ToAllUsers: false,
                ToOwnerId: $('#create-bulletin-recipient').val(),
                ShowStart: $('#create-bulletin-start').val(),
                ShowEnd: $('#create-bulletin-end').val()
            }
        });
        EndLoading();
    }
    Cancel();
    location.reload();
}


async function ShowTableBulletinBoardItems() {
    StartLoading();
    $('#bulletintablebody').empty();
    var BulletinBoards = await GetBulletinBoards();
    if (BulletinBoards == false) {
        EndLoading();
        return;
    }
    for (var i = 0; i < BulletinBoards.length; i++) {
        var row = '<tr>'
            + ' <td>' + BulletinBoards[i].title + ' </td> <td>'
            + BulletinBoards[i].shortDescription + ' </td><td>'
            + BulletinBoards[i].description + ' </td><td>'
            + BulletinBoards[i].creationDate + ' </td><td>'
            + 'От ' + BulletinBoards[i].start + ' до ' + BulletinBoards[i].end + ' </td><td>'
            + BulletinBoards[i].creatorFullName + ' </td><td>'
            + BulletinBoards[i].recipient + ' </td><td>'
            + '<button type="button" class="btn btn-danger btn-sm" id="' + BulletinBoards[i].id + '" onclick="ShowDeleteDialog(id)"> Удалить </button>'
            + '</td>' +
            '</tr>';
        $('#bulletintablebody').append(row);
    }
    EndLoading();
}
async function GetBulletinBoards() {
    var result = await $.ajax({
        type: 'GET',
        url: '/BulletinBoard/List',
        data: {

        }
    });
    return result;
}

$("#create-bulletin-typerecipient")
    .change(async function () {
        $("select option:selected").each(function () {
        });
        if ($('#create-bulletin-typerecipient').val() == 1) {
            $('#create-bulletin-formrecipient').addClass('hide');
            $('#create-bulletin-recipient').addClass('hide');
            $('#create-bulletin-recipient').empty();
        }
        if ($('#create-bulletin-typerecipient').val() == 2) {
            StartLoading();
            $('#create-bulletin-recipient').empty();
            $('#create-bulletin-formrecipient').removeClass('hide');
            $('#create-bulletin-recipient').removeClass('hide');
            var streets = await GetStreets();
            for (var i = 0; i < streets.length; i++) {
                $('#create-bulletin-recipient')
                    .append('<option value="' + streets[i].id + '">' + streets[i].name + '</option>');
            }
            EndLoading();
        }
        if ($('#create-bulletin-typerecipient').val() == 3) {
            StartLoading();
            $('#create-bulletin-recipient').empty();
            $('#create-bulletin-formrecipient').removeClass('hide');
            $('#create-bulletin-recipient').removeClass('hide');
            var buildings = await GetBuildings();
            for (var i = 0; i < buildings.length; i++) {
                $('#create-bulletin-recipient').append('<option value="' +
                    buildings[i].id +
                    '">' +
                    buildings[i].street +
                    ' ' +
                    buildings[i].name +
                    '</option>');
            }
            EndLoading();
        }
        if ($('#create-bulletin-typerecipient').val() == 4) {
            StartLoading();
            $('#create-bulletin-recipient').empty();
            $('#create-bulletin-formrecipient').removeClass('hide');
            $('#create-bulletin-recipient').removeClass('hide');
            var flats = await GetFlats();
            for (var i = 0; i < flats.length; i++) {
                $('#create-bulletin-recipient').append('<option value="' +
                    flats[i].id +
                    '">' +
                    flats[i].fullAddress +
                    '</option>');
            }
            EndLoading();
        }
        if ($('#create-bulletin-typerecipient').val() == 5) {
            StartLoading();
            $('#create-bulletin-recipient').empty();
            $('#create-bulletin-formrecipient').removeClass('hide');
            $('#create-bulletin-recipient').removeClass('hide');
            var owners = await GetOwners();
            for (var i = 0; i < owners.length; i++) {
                $('#create-bulletin-recipient').append('<option value="' +
                    owners[i].id +
                    '">' +
                    owners[i].fullname +
                    '(' +
                    owners[i].id +
                    ')' +
                    '</option>');
            }
            EndLoading();
        }
    })
    .change();

async function ShowDeleteDialog(id) {
    $('#DeleteBulletinDialog').modal('show');
    $('#delete-bulletin-id').val(id);
}
async function DeleteConfirm() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/bulletinboard/Delete',
        data: {
             Id: $('#delete-bulletin-id').val()
        }
    });
    Cancel();
    EndLoading();
    location.reload();
}


function Cancel() {
    $('#CreateBulletinDialog').modal('hide');

    $('#DeleteBuildingDialog').modal('Delete');
}