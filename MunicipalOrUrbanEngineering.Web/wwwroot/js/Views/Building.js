$(document).ready(async function () {
    $('#loading').addClass('hide');
    $('#erorr-message').hide();
    $('#success-message').hide();
    await ShowStreetList();
    await ShowListBuildings();
    await ShowTableFlatItems();
});
function StartLoading() {
    $('#loading').removeClass('hide');
}

function EndLoading() {
    $('#loading').addClass('hide');
}

async function GetStreets() {
    var result = await $.ajax({
        type: 'GET',
        url: '/building/ListStreet',
        data: {
        }
    });
    return result;
}
async function ShowStreetList() {
    StartLoading();
    var streets = await GetStreets();
    for (var i = 0; i < streets.length; i++) {
        $('#street-selector').append('<option value="' + streets[i].id + '">' + streets[i].name + '</option>');
    }
    EndLoading();
}
async function CreateStreetItem() {
    StartLoading();
    await $.ajax({
        type: "POST",
        url: "/building/CreateStreet",
        data: {
            Name: $("#create-street-name").val(),
            Description: $('#create-street-description').val()
        }
    });
    EndLoading();
    Cancel();
    location.reload();
}
async function ShowCreateStreet() {
    StartLoading();
    $('#CreateStreetDialog').modal('show');
    EndLoading();
}

$("#street-selector")
    .change(async function () {
        $("select option:selected").each(function () {
        });
        $('#flattablebody').empty();
        await ShowListBuildings();
        await ShowTableFlatItems();
    })
    .change();

async function ShowListBuildings() {
    StartLoading();
    $('#building-selector').empty();
    var streetId = $('#street-selector').val();
    var buildings = await  GetBuildings(streetId);
    for (var i = 0; i < buildings.length; i++) {
        $('#building-selector').append('<option value="' + buildings[i].id + '">' + buildings[i].name + '</option>');
    }
    EndLoading();
}
async function GetBuildings(streetId) {
    var result = await $.ajax({
        type: 'GET',
        url: '/building/ListBuilding',
        data: {
            streetId: streetId
        }
    });
    return result;
}

async function ShowDeleteStreet() {
    StartLoading();
    $('#DeleteStreetDialog').modal('show');
    EndLoading();
}
async function DeleteStreetConfirm() {
    var streetId = $('#street-selector').val();
    await $.ajax({
        type: "POST",
        url: "/building/DeleteStreet",
        data: {
            Id: streetId
        }
    });
    Cancel();
    location.reload();
}

async function ShowUpdateStreet() {
    var streetId = $('#street-selector').val();
    var street = await GetStreet(streetId);

    $('#edit-street-id').val(street.id);
    $('#edit-street-description').val(street.description);
    $('#edit-street-name').val(street.name);

    $('#EditStreetDialog').modal('show');

}
async function UpdateStreet() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/building/UpdateStreet',
        data: {
            Id: $('#edit-street-id').val(),
            Name: $('#edit-street-name').val(),
            Description: $('#edit-street-description').val()
        }
    });
    EndLoading();
    Cancel();
    location.reload();
}

async function GetStreet(streetId) {
    var result = await $.ajax({
        type: 'GET',
        url: '/building/GetStreet',
        data: {
            streetId: streetId
        }
    });
    return result;
}

async function ShowCreateBuiilding() {
    StartLoading();
    $('#CreateBuildingDialog').modal('show');
    EndLoading();
}
async function CreateBuildingItem() {
    StartLoading();
    var result = await  $.ajax({
        type: "POST",
        url: "/building/CreateBuilding",
        data: {
            StreetId: $('#street-selector').val(),
            Name: $("#create-building-name").val(),
            Description: $('#create-building-description').val()
        }
    });
    Cancel();
    EndLoading();
    location.reload();
}

async function ShowDeleteBuilding() {
    StartLoading();
    $('#DeleteBuildingDialog').modal('show');
    EndLoading();
}
async function DeleteBuildingConfirm() {
    var buildingId = $('#building-selector').val();
    await $.ajax({
        type: "POST",
        url: "/building/DeleteBuilding",
        data: {
            Id: buildingId
        }
    });
    Cancel();
    location.reload();
}

async function ShowUpdateBuilding() {
    StartLoading();
    var buildingId = $('#building-selector').val();
    var building = await GetBuilding(buildingId);
    if (building == null) {
        EndLoading();
        return;
    }
    $('#edit-building-id').val(building.id);

    $('#edit-building-street').val(building.street);
    $('#edit-building-name').val(building.name);
    $('#edit-building-description').val(building.description);

    $('#EditBuildingDialog').modal('show');
    EndLoading();
}
async function GetBuilding(buildingId) {
    var result = await $.ajax({
        type: 'GET',
        url: '/building/GetBuilding',
        data: {
            buildingId: buildingId
        }
    });
    return result;
}

$("#building-selector")
    .change(async function () {
        $("select option:selected").each(function () {
           
        });
        await ShowTableFlatItems();
    })
    .change();

/**
 *  <button type="button" class="btn btn-danger btn-sm" id="" onclick="Delete(id)">
                Удалить квартиру
            </button>
            <button type="button" class="btn btn-warning btn-sm" id="" onclick="Delete(id)">
                Обновить квартиру
            </button>
            <br /><br />
            <button type="button" class="btn btn-info btn-sm">Тарифы</button>
            <br /><br />
            <button type="button" class="btn btn-danger btn-sm" id="" onclick="Delete(id)">
                Удалить владельца
            </button>
            <button type="button" class="btn btn-warning btn-sm" id="" onclick="Delete(id)">
                Обновить владельца
            </button>
 */

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
                + '<button type="button" class="btn btn-danger btn-sm" id="' + flats[i].id + '" onclick="ShowDeleteFlatDialog(id)"> Удалить квартиру </button>  '
                + '<button type="button" class="btn btn-warning btn-sm" id="' + flats[i].id + '" onclick="ShowEditFlat(id)"> Обновить квартиру </button>'
                + '<br/><br/>'
                + '<button type="button" class="btn btn-info btn-sm" id="' + flats[i].id + '" onclick="FlatTariffs(id)"> Тарифы </button>  '
                + '<button type="button" class="btn btn-info btn-sm" id="' + flats[i].id + '" onclick="FlatSheets(id)"> Счета </button>'
                + '<br/><br/>'
                + '<button type="button" class="btn btn-danger btn-sm" id="' + flats[i].id + '" onclick="ShowDeleteOwnerDialog(id)"> Удалить владельца </button>  '
                + '<button type="button" class="btn btn-warning btn-sm" id="' + flats[i].id + '" onclick="ShowUpdateOwnerDialog(id)"> Обновить владельца </button> </td>' +
            '</tr>';
        $('#flattablebody').append(row);
    }
}
async function GetFlats() {
    var buildingId = $('#building-selector').val();
    if (buildingId == null) {
        return false;
    }
    var result = await $.ajax({
        type: 'GET',
        url: '/building/ListFlat',
        data: {
            buildingId: buildingId
        }
    });
    return result;
}

async function ShowCreateFlat() {
    StartLoading();
    var streetId = $('#street-selector').val();
    var street = await GetStreet(streetId);
    if (street == null) {
        EndLoading();
        return;
    }
    var buildingId = $('#building-selector').val();
    var building = await GetBuilding(buildingId);
    if (building == null) {
        EndLoading();
        return; 
    }


    $('#create-flat-street').val(street.name);
    $('#create-flat-building').val(building.name);

    $('#CreateFlatDialog').modal('show');
    EndLoading();
}
async function CreateFlatItem() {
    StartLoading();
    var buildingId = $('#building-selector').val();
    var result = await $.ajax({
        type: 'POST',
        url: '/building/CreateFlat',
        data: {
            buildingId: buildingId,
            apartmentNumber: $('#create-flat-apartamentnumber').val()
        }
    });

    EndLoading();
    Cancel();
    location.reload();
}

async function ShowDeleteFlatDialog(id) {
    StartLoading();
    $('#delete-flat-id').val(id);
    $('#DeleteFlatDialog').modal('show');
    EndLoading();
}
async function DeleteFlatConfirm() {
    StartLoading();
    var id = $('#delete-flat-id').val();
    var result = await  $.ajax({
        type: 'POST',
        url: '/building/DeleteFlat',
        data: {
            Id: id
        }
    });
    $('#delete-flat-id').empty();
    EndLoading();
    Cancel();
    location.reload();
    return result;
}

async function ShowEditFlat(id) {
    StartLoading();
    $('#edit-flat-id').val(id);
    $('#EditFlatDialog').modal('show');
    var flat = await GetFlat(id);
    $('#edit-flat-apartament').val(flat.apartamentNumber);
    $('#edit-flat-address').val(flat.fullAddress);
    EndLoading();
}
async function GetFlat(id) {
    var result = await $.ajax({
        type: 'GET',
        url: '/building/GetFlat',
        data: {
            flatId : id
        }
    });
    return result;
}
async function UpdateFlat() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/building/UpdateFlat',
        data: {
            Id: $('#edit-flat-id').val(),
            ApartmentNumber: $('#edit-flat-apartament').val()
        }
    });
    await ShowTableFlatItems();
    Cancel();
    EndLoading();
}

async function ShowUpdateOwnerDialog(id) {
    StartLoading();
    $('#EditFlatOwner').modal('show');
    var owners = await GetOwners();
    $('#edit-assigment-flatid').val(id);
    var flat = await GetFlat(id);
    $('#edit-assigment-address').val(flat.fullAddress);
    FillOwnerSelector(owners);
    EndLoading();
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
function FillOwnerSelector(owners) {
    $('#edit-assigment-owners').empty();
    for (var i = 0; i < owners.length; i++) {
        $('#edit-assigment-owners').append('<option value="' +
            owners[i].id +
            '">' +
            owners[i].fullname +
            '(' +
            owners[i].id +
            ')' +
            '</option>');
    }
}
async function AssignOwnerToFlat() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/building/AssignFlatToOwner',
        data: {
            ownerId: $('#edit-assigment-owners').val(),
            flatId: $('#edit-assigment-flatid').val()
        }
    });

    Cancel();
    location.reload();
    EndLoading();
}

async function ShowDeleteOwnerDialog(id) {
    StartLoading();
    $('#delete-flatowner-flatid').empty();
    $('#delete-flatowner-ownerid').empty();
    var flat = await GetFlat(id);
    if (flat.ownerId == null) {
        EndLoading();
        Cancel();
        return;
    }

    $('#delete-flatowner-flatid').val(id);
    $('#delete-flatowner-ownerid').val(flat.ownerId);
    $('#DeleteFlatOwnerDialog').modal('show');

    EndLoading();
}
async function DeleteFlatOwnerConfirm() {
    StartLoading();
    var result = await $.ajax({
        type: 'POST',
        url: '/building/RemoveAssignFlatToOwner',
        data: {
            flatId: $('#delete-flatowner-flatid').val(),
            ownerId: $('#delete-flatowner-ownerid').val()
        }
    });
    Cancel();
    location.reload();
    EndLoading();
}

async function FlatTariffs(id) {
    location.href ='/tariff/index?FlatId='+id;
}
async function FlatSheets(id) {
    location.href ='/paymentsheet/index?FlatId='+id;
}

function Cancel() {
    $('#CreateStreetDialog').modal('hide');
    $('#CreateBuildingDialog').modal('hide');

    $('#DeleteStreetDialog').modal('hide');
    $('#DeleteBuildingDialog').modal('hide');

    $('#EditStreetDialog').modal('hide');
    $('#EditBuildingDialog').modal('hide');


    $('#CreateFlatDialog').modal('hide');
    $('#DeleteFlatDialog').modal('hide');
    $('#EditFlatDialog').modal('hide');
    $('#EditFlatOwner').modal('hide');
    $('#DeleteFlatOwnerDialog').modal('hide');
}